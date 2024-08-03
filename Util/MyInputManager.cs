using System;
using System.Collections.Generic;
using UnityEngine;

public class MyInputManager : MonoBehaviour
{
    List<InputUnit> myInputList= new List<InputUnit>();
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

    public void AddInputEventList(Action _func, KeyCode _inputType)
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
class InputUnit
{
    Action func;
    KeyCode inputType;

    public InputUnit(Action _func, KeyCode _inputType)
    {
        func = _func;
        inputType = _inputType;
    }

    public bool CheckToSearch(Action _func, KeyCode _inputType)
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