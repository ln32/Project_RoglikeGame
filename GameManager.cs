using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public CharacterData charData;
    public int seed = 0;
    public int nodeLevel = 0;
    public int stageIndex = 0;
    public string history;
    public string invenData;

    internal void setData()
    {
        if (gameManager == null)
        {
            DontDestroyOnLoad(this);
            gameManager = this;
            charData.InitData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Awake()
    {
        if (gameManager == null)
        {
            DontDestroyOnLoad(this);
            gameManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}