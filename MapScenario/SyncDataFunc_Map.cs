using System.Collections.Generic;
using UnityEngine;

static public class SyncDataFunc_Map
{
    static public void SetMapData_History_Default(this GameManager gameManager, int _stage)
    {
        GameManager.gameManager.stageIndex = _stage;
        GameManager.gameManager.nodeLevel = 0;
        GameManager.gameManager.history = "";
    }
    static public void SetMapData_History(this GameManager gameManager, string _history, int _stageIndex, int _nodeLevel)
    {
        GameManager.gameManager.history = _history;
        GameManager.gameManager.stageIndex = _stageIndex;
        GameManager.gameManager.nodeLevel = _nodeLevel;
    }
    static public void SetMapData_History(this GameManager gameManager, string _data)
    {
        GameManager.gameManager.invenData = _data;
    }
    static public void SetMapData_CharEquips(this MapScenario _MapScenario, int Char_Index, string[] Skill_NameSet)
    {
        // TODO : func space when need to additional Skill
    }

    static public string GetSceneName_byEventIndex(this GameManager gameManager, int _NodeEvent)
    {

# if UNITY_EDITOR
        Debug.Log("Selecting Next Scene Target  Func");
#endif

        int index = (_NodeEvent / 100);

        switch (index)
        {
            case 0: return "Shop";
            case 1: return gameManager.GetCurrStageName();
            case 2: return "Shop"; //TODO: Add scene name to transition to the corresponding battle scene
            case 3: return "Shop"; //TODO: Add scene name to transition to the corresponding event scene
            default:
                break;
        }

        return "Error Index  - " + _NodeEvent;
    }
}


public static class GetSceneName_currStage
{
    public static string GetCurrStageName(this GameManager gameManager)
    {
        int stage = GameManager.gameManager.stageIndex;
        return "Stage " + stage;
    }
}