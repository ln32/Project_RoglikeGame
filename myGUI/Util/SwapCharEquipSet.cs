using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapCharEquipSet : MonoBehaviour
{
    [SerializeField] protected int _currIndex = 0;
    [SerializeField] public Color Interactive_True { get; protected set; }
    [SerializeField] public Color interactive_False { get; protected set; }
    [SerializeField] internal List<SwapBtnSet> swapPool;


    private void Start()
    {
        SwapActiveObj_byIndex(_currIndex);
    }

    protected void SwapActiveObj_byIndex(int _index)
    {
        _currIndex = _index;

        for (int i = 0; i < swapPool.Count; i++)
        {
            swapPool[i].SetBtnInterActivation((i == _index), this);
        }
    }
}

[Serializable]
internal class SwapBtnSet
{
    public Button btn;
    public GameObject obj;

    internal void SetBtnInterActivation(bool input, SwapCharEquipSet _targetColor)
    {
        if (input)
        {
            btn.interactable = !input;
            btn.image.color = _targetColor.Interactive_True;
            obj.SetActive(input);
        }
        else
        {
            btn.interactable = !input;
            btn.image.color = _targetColor.interactive_False;
            obj.SetActive(input);
        }
    }
}