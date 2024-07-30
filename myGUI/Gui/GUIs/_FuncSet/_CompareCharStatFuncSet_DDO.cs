public static class _CompareCharStatFuncSet_DDO
{
    static public bool CompareItem_withStat(this SlotGUI_EquipSlot targetEquip, ItemUnit target)
    {
        int charComp_0 = (int)targetEquip.itemType;
        int itemComp_0 = target.itemData[0];

        if (charComp_0 != itemComp_0)
        {
            return false;
        }

        if (charComp_0 == 0)
        {
            int charType = (int)targetEquip.detailType;
            int itemType = target.itemData[1];

            if (charType != itemType)
            {
                return false;
            }
        }

        int itemStat = target.itemData[2];
        int charStat = target.itemData[3];
        bool isAvabile = (charStat >= itemStat);

        return isAvabile;
    }

}
