using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class _InvenSetManagerPrivateFunc
{
    static internal GUI_ItemUnit GetGUI_byItemData(this MyInvenSpriteDB dataSet, List<int> itemData, Transform trans)
    {
        GUI_ItemUnit obj = Object.Instantiate(dataSet.invenPrefab, trans);
        obj.SetImageGUI_toSprite(dataSet.GetSprite_byItemData(itemData));

        if (itemData[0] == 0)
        {
            Sprite target = dataSet._Value_Dice.DB_DiceSpriteRoot.myData[itemData[^1]];
            Color targetColor = dataSet._Value_Dice.color_Case[itemData[^2]];
            obj.SetGui_toIngredient(target, targetColor);
        }
        else
        {
            string levelText = "Lv " + itemData[^1];
            obj.SetNameText(levelText);
        }

        obj.SetSizeAuto(trans);
        return obj;
    }

    static internal SlotGUI_InvenSlot GetSlotGUI_byAddr(this GUI_InvenSetManager inven, List<int> addrData)
    {
        int invenIndex = addrData[0];
        int slotIndex = addrData[1];

        if (inven.myInvenSet.Count > invenIndex && inven.myInvenSet[invenIndex].MySlotList.Count > slotIndex)
        {
            return inven.myInvenSet[invenIndex].MySlotList[slotIndex];
        }
        return null;
    }

    static internal SlotGUI_InvenSlot GetSlotGUI_byMin(this GUI_InvenSetManager inven)
    {
        foreach (var slot in inven.myInvenSet[0].MySlotList)
        {
            if (slot._itemGUI == null)
            {
                return slot;
            }
        }
        return null;
    }

    static internal void SetItemData_byData(this SlotGUI_InvenSlot inven, ItemUnit item)
    {
        if (item == null)
        {
            Object.Destroy(inven._itemGUI.gameObject);
            inven.SetGUI_byItemGUI(null);
        }
        else
        {
            inven._itemGUI._myData = item;
        }
    }

    static internal void SetItemData_byData(this SlotGUI_ShopGoods inven, ItemUnit item)
    {
        inven._itemGUI._myData = item;
    }

    static internal void SetItemData_byData(this SlotGUI_CookResult inven, ItemUnit item)
    {
        inven._itemGUI._myData = item;
    }
}