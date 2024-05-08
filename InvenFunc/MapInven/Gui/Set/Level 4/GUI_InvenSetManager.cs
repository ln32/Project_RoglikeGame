using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GUI_InvenSetManager : MonoBehaviour
{
    [SerializeField] public List<GUI_InvenSpaceSlotSet> myInvenSet = new();
    public GoldEffectFunc GoldEffect;
    public Transform _InsTrans;
    public bool _isDebug = false;

    [ContextMenu("SetState_onInit")]
    public void SetState_onInit()
    {
        if (!_isDebug)
            return;

        if(myInvenSet == null)
            myInvenSet = GetComponentsInChildren<GUI_InvenSpaceSlotSet>().ToList();

        for (int i = 0; i < myInvenSet.Count; i++)
        {
            List<int> data = new();
            myInvenSet[i].index = i;
            myInvenSet[i].SetState_onInit(data);
        }
    }

    [ContextMenu("_SetMyGrid")]
    public void _SetMyGrid()
    {
        if (!_isDebug)
            return;

        if (myInvenSet == null)
            myInvenSet = GetComponentsInChildren<GUI_InvenSpaceSlotSet>().ToList();

        for (int i = 0; i < myInvenSet.Count; i++)
        {
            List<int> data = new();
            myInvenSet[i].index = i;
            myInvenSet[i].SetState_onInit(data);
        }
    }
}