using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class _RDM_CampCook
{
    internal static void SetIngridiment_byInvenSlot(this RDM_CampCook _CookSC, SlotGUI_InvenSlot _src, RBD_IngridimentSlot _dst)
    {
        var _SGT_GUI_ItemData = SGT_GUI_ItemData.GetInstance();
        GUI_ItemUnit _itemGUI_Src = _src._itemGUI;
        int isCrash = _CookSC._GUI_IngridiSlotManager._values.IsDisAvabibleValue(_itemGUI_Src._myData.index);

        if (isCrash > -1)
        {
            _CookSC._GUI_IngridiSlotManager._values.RBD_Slots[isCrash].SetDefault();
            _CookSC.Event_SlotDropDown_toNull(isCrash);
        }

        _dst.SetDefault();
        _dst.myGUI_Slot.value = _itemGUI_Src._myData.index;

        if (true)
        {
            GUI_ItemUnit _itemGUI_Dst = _SGT_GUI_ItemData.spriteDataSet.GetGUI_byItemData(_src._itemGUI._myData.itemData, _dst.transform);
            _itemGUI_Dst._myData = _src._itemGUI._myData;
            _dst._GUI_ItemUnit = _itemGUI_Dst;

            if (true)
            {
                RectTransform trans_src = _dst._GUI_ItemUnit.transform.GetComponent<RectTransform>();
                RectTransform trans_dst = _dst.transform.GetComponent<RectTransform>();

                RectTransform _RectTransform = trans_src.transform.GetComponent<RectTransform>();
                _RectTransform.sizeDelta = trans_dst.GetComponent<RectTransform>().sizeDelta;
            }

            _dst.myGUI_Slot.GUI_EffectImg.color = _dst.myGUI_Slot.GUI_EffectColor;
            _dst.myGUI_Slot.defaultImg = _dst.myGUI_Slot.GUI_myImg.sprite;
        }


        _src._itemGUI.SetSizeAuto();

        // Refresh result board 
        _CookSC._GUI_IngridiSlotManager.RefreshMyGUI();

        // Set Inven GUI _im focused
        _CookSC.Event_SlotDropDown(_src, _dst);
        return;
    }

    internal static void GetInvenItem_byItemIndex(this RDM_CampCook _CookSC)
    {
        GUI_IngridiSlotManager _m = _CookSC._GUI_IngridiSlotManager;
        if (true)
        {
            // Check ingridiment is Ready
            if (_m._values.checkCurr() == false)
            {
                Debug.Log("need indegri");
                return;
            }
        }

        int targetLevel = 0;
        if (true)
        {
            Vector3Int stdV2_X = new Vector3Int(0, 0, 0);
            Vector3Int stdV2_Y = new Vector3Int(0, 0, 0);
            List<Vector2Int> _rtnData = _CookSC._GUI_IngridiSlotManager._IngredientPoker.Check_PatternList();
            _CookSC._GUI_IngridiSlotManager._IngredientPoker.GetV3_byData(_rtnData,out stdV2_X, out stdV2_Y);
            int temp_Min = Mathf.Min(stdV2_X.z, stdV2_Y.z);
            int temp_Max = (stdV2_X.z + stdV2_Y.z);
            targetLevel = Mathf.Min(Random.Range(temp_Min, temp_Max),6);
            Debug.Log(targetLevel + " / "+stdV2_X + " / " + stdV2_Y);
        }

        // Data Set (inven Data)
        if (true)
        {
            int[] temp = _m._values.GetCookSlotsItemIndex();

            List<ItemUnit> invenGUI = new();
            List<SlotGUI_InvenSlot> _ItemList_Data = _CookSC.invenSC.invenGUI_Manager.myInvenSet[0].MySlotList;

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
                SlotGUI_InvenSlot targetInvenSlot = _CookSC._GUI_InvenSetManager.GetSlotGUI_byAddr(addr);

                targetInvenSlot.SetItemData_byData(null);
                _CookSC.invenSC.invenData_SGT.RemoveItem_byItem(invenGUI[i]);
            }
        }

        // Slot Set
        if (true)
        {
            _m.CookFunc_onIngridiment();
        }

        _CookSC._GUI_ResultCookSet.FillUpResultCook(targetLevel);
        _CookSC._GUI_IngridiSlotManager.Event_Reset();
        return;
    }
}