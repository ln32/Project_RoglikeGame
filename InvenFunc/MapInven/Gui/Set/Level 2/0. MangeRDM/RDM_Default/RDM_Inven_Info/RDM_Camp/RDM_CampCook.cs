using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RDM_CampCook : RDM_Inven_Info
{
    public GUI_IngridiSlotManager _GUI_IngridiSlotManager;
    public GUI_InvenSetManager _GUI_InvenSetManager;
    public GUI_ResultCookSet _GUI_ResultCookSet;
    public InvenSC_Default invenSC;

    private void Start()
    {
        _REF._SGT_GUI_ItemData = invenSC.invenData_SGT;
    }

    [SerializeField] internal List<MyCookSlotIndicator> _MyCookSlotIndicatorList;
    public void Event_SlotDropDown(SlotGUI_InvenSlot _src, RBD_IngridimentSlot _dst)
    {
        _MyCookSlotIndicatorList[_dst._index].SetEvent_SlotDropDown(_src, _dst);
    }

    public void Event_SlotDropDown_toNull(int _index)
    {
        _MyCookSlotIndicatorList[_index].SetEvent_SlotDropDown_toNull();

    }
    public void Event_ResetSlotEffect()
    {
        for (int i = 0; i < _MyCookSlotIndicatorList.Count; i++)
        {
            _MyCookSlotIndicatorList[i].SetEvent_SlotDropDown_toNull();
        }
    }
}

[Serializable]
internal class MyCookSlotIndicator
{
    [SerializeField] internal GUI_ItemUnit myHighlight_ItemUnit;
    [SerializeField] internal GameObject myInitedObj;
    [SerializeField] internal GameObject _MyEffectPrefab;

    internal void SetEvent_SlotDropDown(SlotGUI_InvenSlot _src, RBD_IngridimentSlot _dst)
    {
        if (myInitedObj != null)
        {
            myHighlight_ItemUnit = null; 
            UnityEngine.Object.Destroy(myInitedObj);
        }

        if (_src != null)
        {
            myHighlight_ItemUnit = _src._itemGUI;
            myInitedObj = UnityEngine.Object.Instantiate(_MyEffectPrefab, myHighlight_ItemUnit.transform);
        }
    }
    internal void SetEvent_SlotDropDown_toNull()
    {
        if (myInitedObj != null)
        {
            myHighlight_ItemUnit = null;
            UnityEngine.Object.Destroy(myInitedObj);
        }
    }
}
