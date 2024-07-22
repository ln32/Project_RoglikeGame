using System;
using System.Collections.Generic;
using UnityEngine;

public class MyInputManager : MonoBehaviour
{
    [SerializeField] internal List<InputUnit> myInputList= new List<InputUnit>();
    [SerializeField] protected bool isDuringEvent = false;

    private void Update()
    {
        if (isDuringEvent == false)
        {
            for (int i = 0; i < myInputList.Count; i++)
            {
                myInputList[i].GetInputCheck();
            }
        }  
    }

    public void AddInputEventList(GUI_MapScenario.ProgressMap_preInput _func, KeyCode _inputType)
    {
        for (int i = 0; i < myInputList.Count; i++)
        {
            if (myInputList[i].CheckToSearch(_func, _inputType))
            {
                return;
            }
        }

        myInputList.Add(new InputUnit(_func, _inputType));
    }

    public void SetInputObjActivate(bool input)
    {
        SceneToSceneFuncSGT.GetInstance().EventSysyemObj.SetActive(input);
    }

    public void setDuringState(bool input)
    {
        isDuringEvent = input;
    }
}

[Serializable]
internal class InputUnit
{
    GUI_MapScenario.ProgressMap_preInput func;
    KeyCode inputType;

    public InputUnit(GUI_MapScenario.ProgressMap_preInput _func, KeyCode _inputType)
    {
        func = _func;
        inputType = _inputType;
    }

    public bool CheckToSearch(GUI_MapScenario.ProgressMap_preInput _func, KeyCode _inputType)
    {
        if (inputType == _inputType)
        {
            func = _func;
            return true;
        }
        else
            return false;
    }

    public void GetInputCheck()
    {
        if(Input.GetKeyDown(inputType))
        {
            func();
        }
    }
}