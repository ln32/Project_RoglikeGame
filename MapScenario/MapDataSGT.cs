using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataSGT : MonoBehaviour
{
    private static MapDataSGT mapSingleton;
 
    public MapHistoryData myHistory;
    public CreateStageScenario _cs;
    public GUI_MapScenario _mapGUI;
    public MapScenario CurrMS;

    public Camera visualObj;
    public bool isHaveToCreate = false; 

    public static MapDataSGT GlobalInit()
    {
        return mapSingleton;
    }

    public bool CheckInitOn(MapScenario _scenario)
    {
        if (mapSingleton == null)
        {
            Application.targetFrameRate = 60;
            mapSingleton = this;
            DontDestroyOnLoad(this);

            CurrMS = _scenario;
            //_UpdateData(_scenario);
            return true;
        }
        else if (mapSingleton == this)
        {
            Debug.Log("it is strange");
            _UpdateData(_scenario);
            CurrMS = _scenario;
            return false;
        }
        else
        {
            if (mapSingleton.isHaveToCreate)
            {
                myHistory = mapSingleton.myHistory;

                Destroy(mapSingleton.gameObject);
                {
                    mapSingleton = this;
                    DontDestroyOnLoad(this);

                    _UpdateData(_scenario);

                    CurrMS = _scenario;
                    return true;
                }
            }

            mapSingleton.gameObject.SetActive(true);
            mapSingleton.CurrMS = _scenario;
            _scenario._SGT.mapDATA = mapSingleton;
            mapSingleton._UpdateData(_scenario);
            Destroy(gameObject);

            return false;
        }
    }

    public void ApplyStageClearToHistory()
    {
        myHistory.ClearLevel();
    }

    public void _UpdateData(MapScenario _scenario)
    {
        _scenario._SGT.mapDATA = mapSingleton;
        _scenario.History = myHistory;
        _scenario._SC.cs = mapSingleton._cs;
        _scenario._SC.mapGUI = mapSingleton._mapGUI;
    }
}
