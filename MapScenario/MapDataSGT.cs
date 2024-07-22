using UnityEngine;

public class MapDataSGT : MonoBehaviour
{
    private static MapDataSGT instance;

    [SerializeField] internal MapHistoryData myHistory;
    [SerializeField] internal CreateStageScenario _cs;
    [SerializeField] internal GUI_MapScenario _mapGUI;
    [SerializeField] internal MapScenario CurrMS;

    [SerializeField] internal Camera visualObj;
    [SerializeField] internal bool isHaveToCreate = false;

    public static MapDataSGT GlobalInit()
    {
        return instance;
    }

    public bool CheckInitOn(MapScenario _scenario)
    {
        if (instance == null)
        {
            // First initialization
            Application.targetFrameRate = 60;
            instance = this;
            DontDestroyOnLoad(this);

            CurrMS = _scenario;
            updateData(_scenario);
            return true;
        }
        else if (instance == this)
        {
            // Already initialized as this instance
            Debug.Log("it is strange");
            updateData(_scenario);
            CurrMS = _scenario;
            return false;
        }
        else
        {
            // Another instance exists
            if (instance.isHaveToCreate)
            {
                // Replace existing instance with this one
                myHistory = instance.myHistory;

                Destroy(instance.gameObject);
                instance = this;
                DontDestroyOnLoad(this);

                updateData(_scenario);
                CurrMS = _scenario;
                return true;
            }

            // Maintain existing instance
            instance.gameObject.SetActive(true);
            instance.CurrMS = _scenario;
            _scenario._SGT.mapDATA = instance;
            instance.updateData(_scenario);
            Destroy(gameObject);

            return false;
        }
    }

    public void ApplyStageClearToHistory()
    {
        myHistory.ClearLevel();
    }

    private void updateData(MapScenario _scenario)
    {
        _scenario._SGT.mapDATA = instance;
        _scenario.History = myHistory;
        _scenario._SC.cs = instance._cs;
        _scenario._SC.mapGUI = instance._mapGUI;
    }
}
