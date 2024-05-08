using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static GUI_MapScenario;

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
            ProgressMap_preInput task = new(() => {
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

        ProgressMap_preInput task = new(() => {; });
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
    public SceneToSceneFuncSGT STS;
}

[Serializable]
internal struct EventSC_CombValues_UTIL
{
    public MyInputManager InputM;
    public _AsycLoadScene ALS;
    public EventSC_Char_UTIL myCharGUI;
}

[Serializable]
internal class EventSC_Char_UTIL
{
    public bool isCharGUI = false;
    public myGUIAnimator CharGUI_Intro;
    public myGUIAnimator CharGUI_Outtro;
}