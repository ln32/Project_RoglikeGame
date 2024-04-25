using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class _MoveItemFunc
{
    // above func
    internal static Color ItemToItem_CheckColor(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, SlotGUI_InvenSlot _dst)
    {
        Debug.Log(" <<");
        if (_inven.Comp_ItemToItem(_src, _dst))
        {
            if (CheckItem_isFusionAble(_src._itemGUI._myData))
            {
                return Color.blue;
            }
            else
                Debug.Log("??");
        }

        return Color.white;
    }
    // drop down func   
    internal static void ItemToItem_EventDDO(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, SlotGUI_InvenSlot _dst)
    {
        if (_inven.Comp_ItemToItem(_src, _dst))
        {
            if (CheckItem_isFusionAble(_src._itemGUI._myData))
            {
                _inven.FusionFunc(_src, _dst);
                return;
            }
        }

        _inven.MoveFunc(_src, _dst);
        return;
    }

    // IS SAME ITEM
    internal static bool Comp_ItemToItem(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, SlotGUI_InvenSlot _dst)
    {
        if(_dst._itemGUI == null) return false;
        if (_src._itemGUI == _dst._itemGUI) return false;

        ItemUnit _srcData = _src._itemGUI._myData;
        ItemUnit _dstData = _dst._itemGUI._myData;

        for (int i = 0; i < _srcData.itemData.Count; i++)
        {
            if(_srcData.itemData[i] != _dstData.itemData[i]) return false; 
        }

        return true;
    }

    static void MoveFunc(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, SlotGUI_InvenSlot _dst)
    {
        GUI_ItemUnit _srcGUI = _src._itemGUI;
        GUI_ItemUnit _dstGUI = _dst._itemGUI;
        _dst.SetGUI_byItemGUI(_srcGUI);
        _src.SetGUI_byItemGUI(_dstGUI);
        return;
    }

    static void FusionFunc(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, SlotGUI_InvenSlot _dst)
    {
        _inven.GetInvenSGT().itemUnits.Remove(_src._itemGUI._myData);
        _src.SetItemData_byData(null);

        ItemUnit targetData = _dst._itemGUI._myData; 
        func();
        GUI_ItemUnit newGUI = _inven.GetInvenSGT().spriteDataSet.GetGUI_byItemData(targetData.itemData, _src.GetTransform_ItemGUI());
        _dst.SetItemData_byData(null);
        _dst.SetGUI_byItemGUI(newGUI);
        _dst.SetItemData_byData(targetData);
        
        return;


        void func()
        {
            if (targetData.itemData[0] != 0)
                targetData.itemData[targetData.itemData.Count - 1]++;
        }
    }

    static bool CheckItem_isFusionAble(ItemUnit target) 
    {
        if (target.itemData[0] == 0)
        {
            return false;
        }

        return target.itemData[target.itemData.Count-1] < 2;
    }
}
