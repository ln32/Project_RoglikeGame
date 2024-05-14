using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

public class Playlist : MonoBehaviour
{
    async void Start()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference playlistref = db.Collection("playlist");
        QuerySnapshot snapshot = await playlistref.GetSnapshotAsync();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            Dictionary<string, object> documentDictionary = document.ToDictionary();
            Debug.Log("title:  " + documentDictionary["title"] as string);
            if (documentDictionary.ContainsKey("description"))
            {
                Debug.Log("link: " + documentDictionary["link"] as string);
            }
            Debug.Log("available: " + documentDictionary["available"] as string);
            Debug.Log("time: " + documentDictionary["time"] as string);
        }
    }
}