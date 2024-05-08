using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class Tools_InvenSetManager
{
    static internal GUI_ItemUnit GetGUI_byItemData(this MyInvenSpriteDB _dataSet, List<int> _itemData,Transform _trans)
    {
        GUI_ItemUnit obj = Object.Instantiate(_dataSet.invenPrefab, _trans);

        obj.SetImageGUI_Sprite(_dataSet.GetSprite_byItemData(_itemData));

        if (true)
        {
            if (_itemData[0] == 0)
            {
                Sprite target = _dataSet._Value_Dice.DB_DiceSpriteRoot.myData[_itemData[_itemData.Count - 1]];
                Color target2 = _dataSet._Value_Dice.color_Case[_itemData[_itemData.Count - 2]];
                if (_itemData[1] == 0)
                    obj.SetGui_ToIngredient(target, target2);
                else
                    obj.SetGui_ToIngredient(target, target2);
            }
            else
            {
                string temp = "Lv ";
                temp += _itemData[_itemData.Count - 1];
                obj.SetNameText(temp);
            }
        }

        obj.SetSizeAuto(_trans);
        return obj;
    }

    static internal SlotGUI_InvenSlot GetSlotGUI_byAddr(this GUI_InvenSetManager _inven, List<int> _addrData)
    {
        int _invenIndex = _addrData[0];
        int _slotIndex = _addrData[1]; 
        if (_inven.myInvenSet.Count <= _invenIndex)
            return null;

        if(_inven.myInvenSet[_invenIndex].MySlotList.Count <= _slotIndex)
            return null;

        return _inven.myInvenSet[_invenIndex].MySlotList[_slotIndex];
    }

    static internal SlotGUI_InvenSlot GetSlotGUI_byMin(this GUI_InvenSetManager _inven)
    {
        for (int i = 0; i < _inven.myInvenSet[0].MySlotList.Count; i++)
        {
            if (_inven.myInvenSet[0].MySlotList[i]._itemGUI == null)
                return _inven.myInvenSet[0].MySlotList[i];
        }

        return null;
    }

    static internal void SetItemData_byData(this SlotGUI_InvenSlot _inven, ItemUnit _item)
    {
        _inven._itemGUI._myData = _item;
        if (_item == null)
        {
            Object.Destroy(_inven._itemGUI.gameObject);
            _inven.SetGUI_byItemGUI(null);
        }    

        return;
    }

    static internal void SetItemData_byData(this SlotGUI_ShopGoods _inven, ItemUnit _item)
    {
        _inven._itemGUI._myData = _item;

        return;
    }

    static internal void SetItemData_byData(this SlotGUI_CookResult _inven, ItemUnit _item)
    {
        _inven._itemGUI._myData = _item;

        return;
    }
}