using System;
using UnityEngine;
using static GUI_MapScenario;

public class MapScenario : MonoBehaviour
{
    [SerializeField] internal Values_SGT _SGT;
    [SerializeField] internal Values_UTIL _UTIL;
    [SerializeField] internal Values_SCENARIO _SC;

    [SerializeField] internal MapHistoryData History;
    [SerializeField] protected GameObject MapSC_GUI;

    internal bool isAction;
    private bool _checkCreated = false;

    private void Awake()
    {
        Initialize();
        SetupInventoryInput();
    }

    private void Initialize()
    {
        _checkCreated = _SGT.mapDATA.CheckInitOn(this);
        SceneToSceneFuncSGT.InitSingleton(ref _SGT.STS);
        _SGT.mapDATA.visualObj.enabled = true;
        SceneToSceneFuncSGT.ArriveScene_Map();
    }

    private void SetupInventoryInput()
    {
        ProgressMap_preInput task = new(() =>
        {
            if (_SGT.mapDATA.gameObject.activeSelf)
            {
                EnterInventory();
            }
            else
            {
                _UTIL.CES.BlurStart_InvenToMap();
            }
        });

        _UTIL.InputM.AddInputEventList(task, KeyCode.I);
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
        _SC.mapGUI._selectGUI._InitSelectedFunc(new Action<int>(ProgressMap));
        _SC.cs.SetOnClickFunc(_SC.mapGUI._selectGUI._SelectEvent);

        if (History.GetHistory().Length == 0)
        {
            History.InitHistoryData();
            _SC.cs.NewGame(History.getSeed());
        }
        else
        {
            LoadGame();
        }

        _SC.mapGUI._selectGUI.SaveHighlight_Root();
    }

    private void LoadGame()
    {
        int seed = History._seed;
        string history = History.GetHistory();

        string[] temp = history.Split('/');
        int[] result = new int[temp.Length - 1];

        for (int i = 0; i < temp.Length - 1; i++)
        {
            result[i] = int.Parse(temp[i]);
        }

        _SC.cs.LoadGame(seed, result);
    }

    public void ProgressMap(int index)
    {
        // can't progress, scene is loaded
        if (_SGT.mapDATA.CurrMS._UTIL.ALS.IsLoadedScene())
        {
            return;
        }

        InvenDataEncoder.GetData_toItemList();
        isAction = false;

        GameManager.gameManager.SetMapData_History(
            History.GetHistory() + (index + "/"),
            History.GetStageIndex(),
            (History.GetHistory().Split('/').Length)
        );

        ProgressMap_preInput task = new(() => { });
        Vector3 desV3 = _SC.cs.ProgressMap(index, ref task);

        if (desV3 == Vector3.zero)
        {
            _SGT.mapDATA.ApplyStageClearToHistory();
            _SGT.mapDATA.isHaveToCreate = true;
        }

        SceneToSceneFuncSGT.ExitScene_Map(() =>
        {
            _SGT.mapDATA.visualObj.enabled = false;
            _SGT.mapDATA.CurrMS.MapSC_GUI.SetActive(false);
        });

        task += SceneChange;

        for (int charIndex = 1; charIndex < 4; charIndex++)
        {
            string[] skillEquipped = new string[2] { "Null", "Null" };
            string[] weaponEquipped = null;

            SGT_GUI_ItemData.GetCharInvenSGT(charIndex, ref skillEquipped);

            this.SetMapData_CharEquips(charIndex - 1, weaponEquipped);
        }

        string dstSceneName = GameManager.gameManager.GetSceneName_byEventIndex(_SC.cs.GetIndex_CurrFocusing());
        _SGT.mapDATA.CurrMS._UTIL.ALS.LoadScene_Asyc(dstSceneName);
        _SC.mapGUI.MoveCamera(desV3, task);
    }

    public void ProgressCamp()
    {
        if (_UTIL.ALS.IsLoadedScene())
        {
            return;
        }

        ProgressMap_preInput task = new(() =>
        {
            _UTIL.ALS.TimeToSwitchScene();
            _SGT.mapDATA.gameObject.SetActive(false);
        });

        SceneToSceneFuncSGT.ExitScene_Map(task);

        MapSC_GUI.gameObject.SetActive(false);
        _UTIL.ALS.LoadScene_Asyc("Camp");
    }

    public void SceneChange()
    {
        isAction = true;
        _SGT.mapDATA.CurrMS._UTIL.ALS.TimeToSwitchScene();
        _SGT.mapDATA.gameObject.SetActive(false);
    }

    public void EnterInventory()
    {
        // can't enter inventory, scene is loaded
        if (_UTIL.ALS.IsLoadedScene())
        {
            return;
        }

        _UTIL.CaptureBG.CaptureByMainCam(_SGT.mapDATA._mapGUI.CameraComp);
        ProgressMap_preInput task = new(() => { });
        ProgressMap_preInput rtnTask = new(() => { });

        _SC.invenSC.setGUI_bySGT();

        task += () => { _SGT.mapDATA.gameObject.SetActive(false); };
        rtnTask += () => { _SGT.mapDATA.gameObject.SetActive(true); };

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
        _seed = GameManager.gameManager.seed;
        GetHistory();
    }

    public string GetHistory()
    {
        if (GameManager.gameManager != null)
        {
            return GameManager.gameManager.history;
        }
        else
        {
            _stage = GameManager.gameManager.stageIndex;
            return GameManager.gameManager.history;
        }
    }

    public int GetStageIndex()
    {
        return _stage;
    }

    public int getSeed() { return _seed; }

    public void ClearLevel()
    {
        _stage++;
        GameManager.gameManager.SetMapData_History_Default(_stage);
    }
}

[Serializable]
internal struct Values_SGT
{
    internal MapDataSGT mapDATA;
    internal SceneToSceneFuncSGT STS;
}

[Serializable]
internal struct Values_UTIL
{
    internal OverlayTrick CaptureBG;
    internal MyInputManager InputM;
    internal MapScenario_InvenCtrl CES;
    internal Util_AsycLoadScene ALS;
}

[Serializable]
internal struct Values_SCENARIO
{
    internal CreateStageScenario cs;
    internal GUI_MapScenario mapGUI;
    internal InvenSC_Map invenSC;
    internal GUI_MapNodeInfo mapNodeInfo;
}
