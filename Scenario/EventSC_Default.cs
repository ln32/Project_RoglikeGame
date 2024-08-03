using System;
using UnityEngine;
using UnityEngine.Events;

public class EventSC_Default : MonoBehaviour
{
    [SerializeField] internal EventSC_CombValues_SGT _SGT;
    [SerializeField] internal EventSC_CombValues_UTIL _UTIL;
    [SerializeField] internal UnityEvent _ArriveEvent;
    [SerializeField] internal UnityEvent _ExitEvent;

    private void Awake()
    {
        SceneToSceneFuncSGT.InitSingleton(ref _SGT.STS);
        SceneToSceneFuncSGT.ArriveScene_Map(() => _ArriveEvent.Invoke());

        if (true)
        {
            Action task = new(() => {
                if (_UTIL.myCharGUI.isCharGUI == false)
                {
                    _UTIL.myCharGUI.isCharGUI = true;
                    _UTIL.myCharGUI.CharGUI_Intro.GoDst();
                }
                else if (true)
                {
                    _UTIL.myCharGUI.isCharGUI = false;
                    _UTIL.myCharGUI.CharGUI_Outtro.GoDst();
                }
            });

            _UTIL.InputM.AddInputEventList(task, KeyCode.I);
        }
    }

    public void SceneChange()
    {
        _UTIL.ALS.TimeToSwitchScene();
    }

    public void ProgressMap()
    {
        _ExitEvent.Invoke();
        if (_UTIL.ALS.IsLoadedScene())
        {
            Debug.Log("cant");
            return;
        }

        Action task = new(() => {; });
        task += SceneChange;

        SceneToSceneFuncSGT.ExitScene_Map(task);

        // Scene 이동 추가 및 카메라 이동 시작
        if (true)
        {
            _UTIL.ALS.LoadScene_Asyc("Stage 0");
        }

    }
}


[Serializable]
internal struct EventSC_CombValues_SGT
{
    internal SceneToSceneFuncSGT STS;
}

[Serializable]
internal struct EventSC_CombValues_UTIL
{
    internal MyInputManager InputM;
    internal Util_AsycLoadScene ALS;
    internal EventSC_Char_UTIL myCharGUI;
}

[Serializable]
internal class EventSC_Char_UTIL
{
    internal bool isCharGUI = false;
    internal GUI_Animator CharGUI_Intro;
    internal GUI_Animator CharGUI_Outtro;
}