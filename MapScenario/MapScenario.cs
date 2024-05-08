using System;
using System.Collections.Generic;
using UnityEngine;
using static GUI_MapScenario;

public class MapScenario : MonoBehaviour
{
    [SerializeField] internal Values_SGT _SGT;
    [SerializeField] internal Values_UTIL _UTIL;
    [SerializeField] internal Values_SCENARIO _SC;

    public delegate void OnClickFunc(int index);
    public MapHistoryData History;
    public GameObject MapSC_GUI;

    public bool isAction;
    bool _checkCreated = false;

    private void Awake()
    {
        _checkCreated = _SGT.mapDATA.CheckInitOn(this);
        SceneToSceneFuncSGT.InitSingleton(ref _SGT.STS);
        _SGT.mapDATA.visualObj.enabled = (true);
        SceneToSceneFuncSGT.ArriveScene_Map();

        // inven input
        if (true)
        {
            ProgressMap_preInput task = new(() => {
                if (_SGT.mapDATA.gameObject.activeSelf == true)
                {
                    EnterInventory();
                }
                else if (true)
                {
                    _UTIL.CES.BlurStart_InvenToMap();
                }
            });

            _UTIL.InputM.AddInputEventList(task, KeyCode.I);
        }
    }

    private void Start()
    {
        GameManager.gameManager.GetCurrStageName();
        if (_checkCreated)
            StartMapCreating();

        _SGT.mapDATA.gameObject.SetActive(true);
        _SC.mapGUI.SetInitState();
    }

    public void StartMapCreating()
    {
        // 결정 전 단계를 GUI에게 맡김
        // 맵 진행 시 일어나야할 이벤트를 GUI에게 넣어줌
        _SC.mapGUI._selectGUI._InitSelectedFunc(new OnClickFunc(ProgressMap));

        //  GUI 이벤트를 버튼에게 저장하기 위해 create map 에 넣어둠
        _SC.cs.SetOnClickFunc(_SC.mapGUI._selectGUI._SelectEvent);

        if (History.GetHistory().Length == 0)//(values.history == "Null")
        {
            //debug
            History.InitHistoryData();
            _SC.cs.NewGame(History.getSeed());
        }
        else
            LoadGame();

        _SC.mapGUI._selectGUI.SaveHighlight_Root();

        void LoadGame()
        {
            int seed = this.History._seed;             //(int)(long)GameManager.gameManager.progressDoc["Seed"];
            string history = this.History.GetHistory();    //(string)GameManager.gameManager.progressDoc["History"];

            string[] temp = history.Split('/');
            int[] result = new int[temp.Length - 1];

            for (int i = 0; i < temp.Length - 1; i++)
            {
                result[i] = int.Parse(temp[i]);
            }

            _SC.cs.LoadGame(seed, result);
        }

    }

    // Event On ClickBtn
    public void ProgressMap(int index)
    {
        if (_SGT.mapDATA.CurrMS._UTIL.ALS.IsLoadedScene())
        {
            Debug.Log("cant");
            return;
        }
        _InvenDataEncoder.SetData_byItemList();
        isAction = false;

        GameManager.gameManager.SetMapData_History(
            History.GetHistory() + (index + "/") ,
            History.GetStageIndex(),
            (History.GetHistory().Split('/').Length)
        );

        ProgressMap_preInput task = new(() => {; });
        Vector3 desV3 = _SC.cs.ProgressMap(index, ref task);

        if (desV3 != Vector3.zero) {;}
        else
        {
            _SGT.mapDATA.ApplyStageClearToHistory();
            _SGT.mapDATA.isHaveToCreate = true;
        }

        SceneToSceneFuncSGT.ExitScene_Map(
            new(() => {
                _SGT.mapDATA.visualObj.enabled = (false);
                _SGT.mapDATA.CurrMS.MapSC_GUI.SetActive(false);
            }));


        //MoveScene by MapSC
        if (true)
        {
            task += SceneChange;
            

            for (int charIndex = 1; charIndex < 4; charIndex++)
            {
                string[] skillEquipped = new string[2] { "Null", "Null" };
                string[] weaponEquipped = null;

                SGT_GUI_ItemData.GetCharInvenSGT(charIndex,ref skillEquipped, ref weaponEquipped);

                this.SetMapData_CharSkills(charIndex - 1, skillEquipped);
                this.SetMapData_CharEquips(charIndex - 1, weaponEquipped);
            }

            if (true)
            {
                // <<< Scene Name
                string dstSceneName = GameManager.gameManager.GetSceneName_byEventIndex(_SC.cs.GetIndex_atCurrFocusing());
                _SGT.mapDATA.CurrMS._UTIL.ALS.LoadScene_Asyc(dstSceneName);
                _SC.mapGUI.moveCamFunc(desV3, task);
            }
        }
    }

    public void ProgressCamp()
    {
        if (_UTIL.ALS.IsLoadedScene())
        {
            Debug.Log("cant");
            return;
        }

        ProgressMap_preInput task = new(() => {; });
        task += () => _UTIL.ALS.TimeToSwitchScene();
        task += () => _SGT.mapDATA.gameObject.SetActive(false);
        SceneToSceneFuncSGT.ExitScene_Map(task);

        // MoveScene To Camp, Cam Move 
        if (true)
        {
            MapSC_GUI.gameObject.SetActive(false);
            Debug.Log("MoveScene To Camp");
            _UTIL.ALS.LoadScene_Asyc("Camp");
        }
    }

    public void SceneChange()
    {
        isAction = true;
        _SGT.mapDATA.CurrMS._UTIL.ALS.TimeToSwitchScene();
        _SGT.mapDATA.gameObject.SetActive(false);
    }

    public void EnterInventory()
    {
        if (_UTIL.ALS.IsLoadedScene())
        {
            Debug.Log("cant");
            return;
        }

        _UTIL.CaptureBG.CaptureByMainCam(_SGT.mapDATA._mapGUI.CameraComp);
        ProgressMap_preInput task;
        ProgressMap_preInput rtnTask = new(() => {; });

        if (true) // Blur 작업 전 작업, Inven쪽 작업 할당
        {
            _SC.invenSC.setGUI_bySGT();
        }

        if (true) // Blur 작업 후 작업 설정, 
        {
            task = new(() => {; });
            task += () => { _SGT.mapDATA.gameObject.SetActive(false); }; 
            rtnTask += () => { _SGT.mapDATA.gameObject.SetActive(true); };
        }      
  

        _UTIL.CES.BlurStart(task, rtnTask);
    }
}

[Serializable]
public class MapHistoryData
{
    [SerializeField] internal int _stage;
    [SerializeField] internal int _seed;


    public void InitHistoryData()
    {
        _seed = GameManager.seed;
        GetHistory();
        return;
    }

    public string GetHistory()
    {
        string _history;
        if (GameManager.gameManager != null)
        {
            _history = GameManager.gameManager.history;
            Debug.Log("sad");
        } else
        {
            _history = CJH_GameManager._instance.history;
            _stage = CJH_GameManager._instance.stageIndex;
        }
        return _history;
    }

    public int GetStageIndex()
    {
        return _stage;
    }

    public int getSeed() { return _seed * (_stage+1); }

    public void ClearLevel()
    {
        _stage++;
        GameManager.gameManager.SetMapData_History_Default(_stage);
    }

    public string GetNextScene()
    {
        return "Stage " + _stage;
    }
}

[Serializable]
internal struct Values_SGT
{
    public MapDataSGT mapDATA;
    public SceneToSceneFuncSGT STS;
}

[Serializable]
internal struct Values_UTIL
{
    public OverlayTrick CaptureBG;
    public MyInputManager InputM;
    public InvenCtrl_MapSC CES;
    public _AsycLoadScene ALS;
}

[Serializable]
internal struct Values_SCENARIO
{
    public CreateStageScenario cs;
    public GUI_MapScenario mapGUI;
    public InvenSC_Map invenSC;
    public GUI_MapNodeInfo mapNodeInfo;
}
