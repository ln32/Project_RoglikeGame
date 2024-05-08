using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

static public class SyncDataFunc_Map
{
    static public void SetMapData_History_Default(this GameManager gameManager, int _stage)
    {
        if (gameManager != null)
        {
            gameManager.history = _stage + "_";
            gameManager.nodeLevel = 0;
        }
        else
        {
            Debug.Log("gameManager is null");
        }
    }
    static public void SetMapData_History(this GameManager gameManager, string _history, int _stageIndex, int _nodeLevel)
    {
        if (gameManager != null)
        {
            gameManager.history = _history; 
            gameManager.nodeLevel = _nodeLevel;
            DataManager.dataManager.SetDocumentData("NodeLevel", null, "Progress", GameManager.gameManager.Uid);
        }
        else
        {
            Debug.Log("gameManager is null - " + new Vector2(_stageIndex, _nodeLevel));
        }
    }
    static public void SetMapData_History(this GameManager gameManager, string _data)
    {

        if (gameManager != null)
        {
            GameManager.gameManager.invenData = _data;
            DataManager.dataManager.SetDocumentData("NodeLevel", null, "Progress", GameManager.gameManager.Uid);
        }
        else
        {
            Debug.Log("gameManager is null");
        }
    }

    static public void SetMapData_CharSkills(this MapScenario _MapScenario, int Char_Index, string[] Skill_NameSet)
    {
        CharacterManager _CharacterManager = CharacterManager.characterManager;
        for (int i = 0; i < Skill_NameSet.Length; i++)
        {
            if (Skill_NameSet[i] != "Null")
            {
                if (_CharacterManager != null)
                {
                    CharacterData getChar = _CharacterManager.GetCharacter(Char_Index);
                    getChar.ChangeSkill(i, Skill_NameSet[i]);
                }
            }
        }
    }
    static public void SetMapData_CharEquips(this MapScenario _MapScenario, int Char_Index, string[] Skill_NameSet)
    {
        CharacterManager _CharacterManager = CharacterManager.characterManager;
        for (int i = 0; i < Skill_NameSet.Length; i++)
        {
            if (Skill_NameSet[i] != "Null")
            {
                if (_CharacterManager != null)
                {
                    CharacterData getChar = _CharacterManager.GetCharacter(Char_Index);
                    getChar.ChangeSkill(i, Skill_NameSet[i]);
                }
            }
        }
    }

    static public string GetSceneName_byEventIndex(this GameManager gameManager, int _NodeEvent)
    {
        int index = (_NodeEvent / 100);
        switch (index)
        {
            case 0: return "Camp_Event";
            case 1: return gameManager.GetCurrStageName(); ;
            case 2: return "Battle"; 
            default:
                break;
        }
        return "???";
    }
}


public static class GetSceneName_currStage
{
    public static string GetCurrStageName(this GameManager gameManager)
    {
        if (gameManager != null)
        {
            //int stage = CJH_GameManager._instance.stageIndex;
            //CJH_GameManager._instance.history = "";

            return "Stage 0";// + stage;
        }
        else
        {
            Debug.Log("gm is null");
            int stage = CJH_GameManager._instance.stageIndex;
            // CJH_GameManager._instance.history = "";

            return "Stage " + stage;
        }
    }
}


public static class GetSceneName_EventNodes
{
    public static string sad(this GameManager gameManager)
    {
        if (gameManager != null)
        {
            //int stage = CJH_GameManager._instance.stageIndex;
            //CJH_GameManager._instance.history = "";

            return "Stage 0";// + stage;
        }
        else
        {
            Debug.Log("gm is null");
            int stage = CJH_GameManager._instance.stageIndex;
            // CJH_GameManager._instance.history = "";

            return "Stage " + stage;
        }
    }
}
