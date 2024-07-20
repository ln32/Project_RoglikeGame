using System.Collections.Generic;
using static Tools_InvenSetManager;

public static class _RDM_CampSC
{
    internal static void SetIngredientByInventorySlot(this RDM_CampSC campSC, SlotGUI_InvenSlot src_Slot, RBD_IngridimentSlot dst_Slot)
    {
        var guiItemData = SGT_GUI_ItemData.GetInstance();
        GUI_ItemUnit sourceItemGUI = src_Slot._itemGUI;
        int conflictIndex = campSC._GUI_IngridiSlotManager._values.IsDisAvabibleValue(sourceItemGUI._myData.index);

        if (conflictIndex > -1)
        {
            campSC._GUI_IngridiSlotManager._values.RBD_Slots[conflictIndex].SetDefault();
        }

        dst_Slot.SetDefault();
        dst_Slot.myGUI_Slot.value = sourceItemGUI._myData.index;

        GUI_ItemUnit destinationItemGUI = guiItemData.spriteDataSet.GetGUI_byItemData(sourceItemGUI._myData.itemData, dst_Slot.transform);
        destinationItemGUI._myData = sourceItemGUI._myData;
        dst_Slot._GUI_ItemUnit = destinationItemGUI;

        dst_Slot.myGUI_Slot.GUI_EffectImg.color = dst_Slot.myGUI_Slot.GUI_EffectColor;
        dst_Slot.myGUI_Slot.defaultImg = dst_Slot.myGUI_Slot.GUI_myImg.sprite;

        src_Slot._itemGUI.SetSizeAuto();
        campSC._GUI_IngridiSlotManager.RefreshMyGUI();
    }

    internal static void GetInventoryItems_byItemIndex(this RDM_CampSC campSC)
    {
        GUI_IngridiSlotManager ingredientSlotManager = campSC._GUI_IngridiSlotManager;
        if (!ingredientSlotManager._values.checkCurr())
            return;

        int[] ingredientIndexes = ingredientSlotManager._values.GetCookSlotsItemIndex();
        List<ItemUnit> inventoryItems = GetInventoryItems(campSC, ingredientIndexes);

        RemoveFilteredItems(campSC, inventoryItems);

        ingredientSlotManager.CookFunc_onIngridiment();
        ingredientSlotManager.Event_Reset();
    }

    internal static List<ItemUnit> GetInventoryItems(RDM_CampSC campSC, int[] ingredientAry)
    {
        List<ItemUnit> inventoryItems = new();
        List<SlotGUI_InvenSlot> itemListData = campSC.invenSC.invenGUI_Manager.myInvenSet[0].MySlotList;

        for (int i = 0; i < ingredientAry.Length; i++)
        {
            if (ingredientAry[i] == -1)
                continue;
            inventoryItems.Add(itemListData[ingredientAry[i]].GetMyItemGUI()._itemGUI._myData);
        }

        return inventoryItems;
    }

    private static void RemoveFilteredItems(RDM_CampSC campSC, List<ItemUnit> inventoryItems)
    {
        foreach (var item in inventoryItems)
        {
            List<int> address = item.invenAddr;
            SlotGUI_InvenSlot targetSlot = campSC._GUI_InvenSetManager.GetSlotGUI_byAddr(address);

            targetSlot.SetItemData_byData(null);
            campSC.invenSC.invenData_SGT.RemoveItem_byItem(item);
        }
    }
}
