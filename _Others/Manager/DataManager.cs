using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using Firebase.Firestore;
using System.Collections.Generic;
using EnumCollection;
using System.Threading.Tasks;
using System.Linq;
using System;

public class DataManager : MonoBehaviour
{
    string path;
    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    static extern long WritePrivateProfileString(
    string Section, string Key, string Value, string FilePath);
    //[DllImport("kernel32", CharSet = CharSet.Unicode)]
    // extern int GetPrivateProfileString(
    //    string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);
    FirebaseFirestore db;
    internal GameData gameData;
    public static DataManager dataManager;
    private void Awake()
    {
        gameData = new GameData();
        path = Path.Combine(Application.dataPath.Replace("/", "\\"), "config.ini");
        db = FirebaseFirestore.DefaultInstance;
        if (!dataManager)
        {
            dataManager = this;
        }
    }

    public string GetConfigData(DataSection _section, string _key)
    {
        switch (_section)
        {
            case DataSection.SoundSetting:
                return (string)gameData.soundSetting[_key];
            case DataSection.Language:
                return (string)gameData.language[_key];
            default:
                return null;
        }
    }

    public async Task<List<DocumentSnapshot>> GetDocumentSnapshots(string _collectionRef, Func<Query, Query> _filter = null)
    {
        Query query = db.Collection(_collectionRef);

        if (_filter != null)
        {
            query = _filter(query);
        }

        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        return snapshot.Documents.ToList();
    }
    public DocumentReference GetDocumentReference(string _documentRef) => db.Document(_documentRef);

    public async Task<Dictionary<string, object>> GetField(string _collectionRef, string _documentId)
    {
        DocumentReference documentRef = db.Collection(_collectionRef).Document(_documentId);
        DocumentSnapshot documentSnapshot = await documentRef.GetSnapshotAsync();
        if (documentSnapshot != null)
            return documentSnapshot.ToDictionary();
        else
        {
            Debug.Log("DocumentSnapshot Null");
            return null;
        }
    }

    public async Task<object> GetFieldData(string _field, string _collectionRef, string _documentId)
    {
        Dictionary<string, object> documentData = await GetField(_collectionRef, _documentId);
        if (documentData.TryGetValue(_field, out object value))
        {
            return value;
        }
        else
        {
            return null;
        }
    }

    public async void SetDocumentData(string _field, object _value, string _collectionRef, string _documentId)
    {
        //기존 도큐먼트에 Set
        CollectionReference collectionRef = db.Collection(_collectionRef);
        DocumentReference documentRef = collectionRef.Document(_documentId);
        Dictionary<string, object> dict = new Dictionary<string, object> { { _field, _value } };
        await documentRef.SetAsync(dict, SetOptions.MergeAll);
    }
    public async Task<string> SetDocumentData(string _field, object _value, string _collectionRef)
    {
        //도큐먼트 만들면서 Set
        CollectionReference collectionRef = db.Collection(_collectionRef);
        DocumentReference documentRef = collectionRef.Document();
        Dictionary<string, object> dict = new Dictionary<string, object> { { _field, _value } };
        await documentRef.SetAsync(dict, SetOptions.MergeAll);
        return documentRef.Id;
    }
    public async void SetDocumentData(Dictionary<string, object> _dict, string _collectionRef, string _documentId)
    {
        CollectionReference collectionRef = db.Collection(_collectionRef);

        // _documentId가 null이면 Firestore에서 자동으로 문서 ID를 생성
        DocumentReference documentRef = collectionRef.Document(_documentId);

        await documentRef.SetAsync(_dict, SetOptions.MergeAll);
    }
    public async Task<string> SetDocumentData(Dictionary<string, object> _dict, string _collectionRef)
    {
        CollectionReference collectionRef = db.Collection(_collectionRef);
        DocumentReference documentRef = collectionRef.Document();
        await documentRef.SetAsync(_dict, SetOptions.MergeAll);
        return documentRef.Id;
    }

    public void SetConfigData(DataSection _section, string _key, object _value)
    {
        if (!string.IsNullOrEmpty(_key) && _value != null)
            switch (_section)
            {
                case DataSection.SoundSetting:
                    UpdateDict(gameData.soundSetting, "SoundSetting");
                    break;
                case DataSection.Language:
                    UpdateDict(gameData.language, "Language");
                    break;
            }

        void UpdateDict(Dictionary<string, object> _dict, string _sectionName)
        {
            if (_dict.ContainsKey(_key))
                _dict[_key] = _value;
            else
                _dict.Add(_key, _value);
            WritePrivateProfileString(_sectionName, _key, _value.ToString(), path);
        }
    }
    public Dictionary<string, object> ConvertToObjDictionary<T>(Dictionary<string, T> sourceDictionary)
    {
        Dictionary<string, object> targetDictionary = new Dictionary<string, object>();

        foreach (var kvp in sourceDictionary)
        {
            targetDictionary.Add(kvp.Key, kvp.Value);
        }

        return targetDictionary;
    }
}
internal class GameData
{
    public Dictionary<string, object> soundSetting;
    public Dictionary<string, object> language;
    Dictionary<string, object> curDict;
    internal GameData()
    {
        soundSetting = new();
        language = new();
        string path = Path.Combine(Application.dataPath.Replace("/", "\\"), "config.ini");
        string[] lines;
        try
        {
            lines = File.ReadAllLines(path);
        }
        catch
        {
            Debug.Log("No Config.ini");
            return;
        }
        foreach (string line in lines)
        {
            if (line.StartsWith("[") && line.EndsWith("]"))//섹션
            {
                switch (line.Substring(1, line.Length - 2))
                {
                    case "SoundSetting":
                        curDict = soundSetting;
                        break;
                    case "Language":
                        curDict = language;
                        break;

                }
            }
            else if (!string.IsNullOrEmpty(line))
            {
                string[] parts = line.Split('=');
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();
                    curDict.Add(key, value);
                }
            }
        }
    }
}