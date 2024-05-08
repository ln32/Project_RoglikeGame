using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class LocalTest : MonoBehaviour
{
    
    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    static extern long WritePrivateProfileString(
        string Section, string Key, string Value, string FilePath);


    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    static extern int GetPrivateProfileString(
        string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);
    static string path;
    public string ReadIni()
    {
        string filePath = Path.Combine(path, "config.ini");
        Debug.Log(filePath);
        var value = new StringBuilder(255);
        GetPrivateProfileString("base", "password", "Error", value, 255, filePath);

        Debug.Log(value.ToString());

        return value.ToString();
    }

    public static void WriteIni(string Section, string Key, string Value)
    {
        WritePrivateProfileString(Section, Key, Value, path);
    }
    void Start()
    {
        path = Application.dataPath.Replace("/", "\\");
        ReadIni();
        WriteIni("Test", "A", "a");
    }
}
