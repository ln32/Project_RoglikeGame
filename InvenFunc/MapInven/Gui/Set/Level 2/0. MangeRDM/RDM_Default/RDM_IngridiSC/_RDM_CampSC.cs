using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public static class _RDM_CampSC
{
    internal static void SetIngridiment_byInvenSlot(this RDM_CampSC _CampSC, SlotGUI_InvenSlot _src, RBD_IngridimentSlot _dst)
    {
        var _SGT_GUI_ItemData = SGT_GUI_ItemData.GetSGT();
        GUI_ItemUnit _itemGUI_Src = _src._itemGUI;
        int isCrash = _CampSC._GUI_IngridiSlotManager._values.IsDisAvabibleValue(_itemGUI_Src._myData.index);
        
        if(isCrash > -1)
        {
            _CampSC._GUI_IngridiSlotManager._values.RBD_Slots[isCrash].SetDefault();
        }

        _dst.SetDefault();
        _dst.myGUI_Slot.value = _itemGUI_Src._myData.index;

        if (true)
        {
            GUI_ItemUnit _itemGUI_Dst = _SGT_GUI_ItemData.spriteDataSet.GetGUI_byItemData(_src._itemGUI._myData.itemData, _dst.transform);
            _itemGUI_Dst._myData = _src._itemGUI._myData;
            _dst._GUI_ItemUnit = _itemGUI_Dst;

            _dst.myGUI_Slot.GUI_EffectImg.color = _dst.myGUI_Slot.GUI_EffectColor;
            _dst.myGUI_Slot.defaultImg = _dst.myGUI_Slot.GUI_myImg.sprite;
        }

        _src._itemGUI.SetSizeAuto();
        _CampSC._GUI_IngridiSlotManager.RefreshMyGUI();
        return;
    }

    internal static void GetInvenItem_byItemIndex(this RDM_CampSC _CampSC)
    {
        GUI_IngridiSlotManager _m = _CampSC._GUI_IngridiSlotManager;
        if (true)
        {
            // Check ingridiment is Ready
            if (_m._values.checkCurr() == false)
                return;
        }

        // Data Set (inven Data)
        if (true)
        {
            int[] temp = _m._values.GetCookSlotsItemIndex();

            List<ItemUnit> invenGUI = new();
            List<SlotGUI_InvenSlot> _ItemList_Data = _CampSC.invenSC.invenGUI_Manager.myInvenSet[0].MySlotList;
            
            // filtering 2
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == -1)
                    continue;
                invenGUI.Add(_ItemList_Data[temp[i]].GetMyItemGUI()._itemGUI._myData);
            }

            // Delete Filtered
            for (int i = 0; i < invenGUI.Count; i++)
            {
                List<int> addr = invenGUI[i].invenAddr;

                SlotGUI_InvenSlot targetInvenSlot = _CampSC._GUI_InvenSetManager.GetSlotGUI_byAddr(addr);

                targetInvenSlot.SetItemData_byData(null);
                _CampSC.invenSC.invenData_SGT.itemUnits.Remove(invenGUI[i]);
            }
        }

        // Slot Set
        if (true)
        {
            _m.CookFunc_onIngridiment();
        }

        //_CampSC._GUI_ResultCookSet.FillUpResultCook();
        _CampSC._GUI_IngridiSlotManager.Event_Reset();
        return;
    }
}