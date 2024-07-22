using UnityEngine;

internal static class _MoveItemFunc
{
    internal static Color ItemToItem_CheckColor(this iRoot_DDO_Manager inven, SlotGUI_InvenSlot src, SlotGUI_InvenSlot dst)
    {
        if (inven.Comp_ItemToItem(src, dst) && CheckItem_isFusionAble(src._itemGUI._myData))
        {
            return Color.blue;
        }

        return Color.white;
    }

    internal static void ItemToItem_EventDDO(this iRoot_DDO_Manager inven, SlotGUI_InvenSlot src, SlotGUI_InvenSlot dst)
    {
        if (inven.Comp_ItemToItem(src, dst) && CheckItem_isFusionAble(src._itemGUI._myData))
        {
            inven.FusionFunc(src, dst);
        }
        else
        {
            inven.MoveFunc(src, dst);
        }
    }

    internal static bool Comp_ItemToItem(this iRoot_DDO_Manager inven, SlotGUI_InvenSlot src, SlotGUI_InvenSlot dst)
    {
        if (dst._itemGUI == null || src._itemGUI == dst._itemGUI)
        {
            return false;
        }

        ItemUnit srcData = src._itemGUI._myData;
        ItemUnit dstData = dst._itemGUI._myData;

        for (int i = 0; i < srcData.itemData.Count; i++)
        {
            if (srcData.itemData[i] != dstData.itemData[i])
            {
                return false;
            }
        }

        return true;
    }

    private static void MoveFunc(this iRoot_DDO_Manager inven, SlotGUI_InvenSlot src, SlotGUI_InvenSlot dst)
    {
        GUI_ItemUnit srcGUI = src._itemGUI;
        GUI_ItemUnit dstGUI = dst._itemGUI;

        dst.SetGUI_byItemGUI(srcGUI);
        src.SetGUI_byItemGUI(dstGUI);
    }

    private static void FusionFunc(this iRoot_DDO_Manager inven, SlotGUI_InvenSlot src, SlotGUI_InvenSlot dst)
    {
        inven.GetInvenSGT().RemoveItem_byItem(src._itemGUI._myData);
        src.SetItemData_byData(null);

        ItemUnit targetData = dst._itemGUI._myData;
        IncreaseItemLevelIfApplicable(targetData);

        GUI_ItemUnit newGUI = inven.GetInvenSGT().spriteDataSet.GetGUI_byItemData(targetData.itemData, src.GetTransform_ItemGUI());
        dst.SetItemData_byData(null);
        dst.SetGUI_byItemGUI(newGUI);
        dst.SetItemData_byData(targetData);
    }

    private static void IncreaseItemLevelIfApplicable(ItemUnit targetData)
    {
        if (targetData.itemData[0] != 0)
        {
            targetData.itemData[targetData.itemData.Count - 1]++;
        }
    }

    private static bool CheckItem_isFusionAble(ItemUnit target)
    {
        return target.itemData[0] != 0 && target.itemData[target.itemData.Count - 1] < 2;
    }
}
