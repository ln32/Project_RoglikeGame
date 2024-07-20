using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class _RDM_CampCook
{
    internal static void SetIngredient_byInvenSlot(this RDM_CampCook cookCamp, SlotGUI_InvenSlot sourceSlot, RBD_IngridimentSlot destinationSlot)
    {
        var itemData = SGT_GUI_ItemData.GetInstance();
        GUI_ItemUnit sourceItemGUI = sourceSlot._itemGUI;
        int crashIndex = cookCamp.GUI_IngridiSlotManager._values.IsDisAvabibleValue(sourceItemGUI._myData.index);

        // Handle item crash
        if (crashIndex > -1)
        {
            cookCamp.GUI_IngridiSlotManager._values.RBD_Slots[crashIndex].SetDefault();
            cookCamp.Event_SlotDropDown_toNull(crashIndex);
        }

        // Set destination slot with source item
        destinationSlot.SetDefault();
        destinationSlot.myGUI_Slot.value = sourceItemGUI._myData.index;

        GUI_ItemUnit destinationItemGUI = itemData.spriteDataSet.GetGUI_byItemData(sourceSlot._itemGUI._myData.itemData, destinationSlot.transform);
        destinationItemGUI._myData = sourceSlot._itemGUI._myData;
        destinationSlot._GUI_ItemUnit = destinationItemGUI;

        // Adjust item size
        AdjustItemSize(destinationSlot._GUI_ItemUnit.transform, destinationSlot.transform);

        // Update GUI effects
        destinationSlot.myGUI_Slot.GUI_EffectImg.color = destinationSlot.myGUI_Slot.GUI_EffectColor;
        destinationSlot.myGUI_Slot.defaultImg = destinationSlot.myGUI_Slot.GUI_myImg.sprite;

        sourceSlot._itemGUI.SetSizeAuto();

        // Refresh the ingredient slot manager GUI
        cookCamp.GUI_IngridiSlotManager.RefreshMyGUI();

        // Set inventory GUI focus
        cookCamp.Event_SlotDropDown(sourceSlot, destinationSlot);
    }

    internal static void GetInvenItem_byItemIndex(this RDM_CampCook cookCamp)
    {
        var ingredientSlotManager = cookCamp.GUI_IngridiSlotManager;

        // Check if ingredients are ready
        if (!ingredientSlotManager._values.checkCurr())
        {
            Debug.Log("Ingredients are not ready.");
            return;
        }

        // Determine target level based on ingredient pattern
        int targetLevel = DetermineTargetLevel(ingredientSlotManager);

        // Update inventory data
        UpdateInventoryData(cookCamp, ingredientSlotManager);

        // Perform cooking function on ingredients
        ingredientSlotManager.CookFunc_onIngridiment();

        // Update result cook set
        cookCamp.GUI_ResultCookSet.FillUpResultCook(targetLevel);

        // Reset ingredient slot manager
        cookCamp.GUI_IngridiSlotManager.Event_Reset();
    }

    private static void AdjustItemSize(Transform sourceTransform, Transform destinationTransform)
    {
        RectTransform sourceRectTransform = sourceTransform.GetComponent<RectTransform>();
        RectTransform destinationRectTransform = destinationTransform.GetComponent<RectTransform>();
        sourceRectTransform.sizeDelta = destinationRectTransform.sizeDelta;
    }

    private static int DetermineTargetLevel(GUI_IngridiSlotManager ingredientSlotManager)
    {
        Vector3Int stdV2_X, stdV2_Y;
        List<Vector2Int> patternData = ingredientSlotManager._IngredientPoker.Check_PatternList();
        ingredientSlotManager._IngredientPoker.GetV3_byData(patternData, out stdV2_X, out stdV2_Y);

        int minLevel = Mathf.Min(stdV2_X.z, stdV2_Y.z);
        int maxLevel = stdV2_X.z + stdV2_Y.z;
        int targetLevel = Mathf.Min(Random.Range(minLevel, maxLevel), 6);

        Debug.Log($"{targetLevel} / {stdV2_X} / {stdV2_Y}");

        return targetLevel;
    }

    private static void UpdateInventoryData(RDM_CampCook cookCamp, GUI_IngridiSlotManager ingredientSlotManager)
    {
        int[] ingredientIndices = ingredientSlotManager._values.GetCookSlotsItemIndex();

        List<ItemUnit> inventoryItems = new();
        List<SlotGUI_InvenSlot> inventorySlotList = cookCamp.invenSC.invenGUI_Manager.myInvenSet[0].MySlotList;

        // Collect items from inventory slots
        foreach (int index in ingredientIndices)
        {
            if (index == -1) continue;
            inventoryItems.Add(inventorySlotList[index].GetMyItemGUI()._itemGUI._myData);
        }

        // Remove collected items from inventory
        foreach (var item in inventoryItems)
        {
            List<int> address = item.invenAddr;
            SlotGUI_InvenSlot targetSlot = cookCamp.GUI_InvenSetManager.GetSlotGUI_byAddr(address);
            targetSlot.SetItemData_byData(null);
            cookCamp.invenSC.invenData_SGT.RemoveItem_byItem(item);
        }
    }
}
