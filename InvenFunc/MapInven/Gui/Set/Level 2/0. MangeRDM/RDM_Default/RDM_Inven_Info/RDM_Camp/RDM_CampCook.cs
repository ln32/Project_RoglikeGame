using System;
using System.Collections.Generic;
using UnityEngine;

public class RDM_CampCook : RDM_Inven_Info
{
    internal GUI_IngridiSlotManager GUI_IngridiSlotManager;
    internal GUI_InvenSetManager GUI_InvenSetManager;
    internal GUI_ResultCookSet GUI_ResultCookSet;
    internal InvenSC_Default invenSC;

    [SerializeField] internal List<MyCookSlotIndicator> myCookSlotIndicatorList;

    private void Start()
    {
        _REF._SGT_GUI_ItemData = invenSC.invenData_SGT;
    }

    public void Event_SlotDropDown(SlotGUI_InvenSlot _src, RBD_IngridimentSlot _dst)
    {
        myCookSlotIndicatorList[_dst._index].SetEvent_SlotDropDown(_src, _dst);
    }

    public void Event_SlotDropDown_toNull(int _index)
    {
        myCookSlotIndicatorList[_index].SetEvent_SlotDropDown_toNull();
    }

    public void Event_ResetSlotEffect()
    {
        for (int i = 0; i < myCookSlotIndicatorList.Count; i++)
        {
            myCookSlotIndicatorList[i].SetEvent_SlotDropDown_toNull();
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
