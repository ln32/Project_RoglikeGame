using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJH_GameManager : MonoBehaviour
{
    public static CJH_GameManager _instance;
    public int nodeLevel = 0;
    public int stageIndex = 0;
    public string history;
    public string invenData;

    internal void setData()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(this);
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(this);
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}