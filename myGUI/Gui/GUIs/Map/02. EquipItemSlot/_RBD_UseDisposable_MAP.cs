using UnityEngine;

public static class _RBD_UseDisposable_MAP
{
    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, RBD_UseDisposable _dst)
    {
        if (!(_inven is RDM_MapSC)) return;

        var itemData = _src._itemGUI._myData.itemData;
        if (itemData[0] != 0) return;

        var characterData = CharacterData.GetSGT();
        var tempChar = characterData.GetCharData(_dst.myIndex);

        // interacting Item
        if (true)
        {
            tempChar.resist += (itemData[1] * 2 + 1);
            int targetHp = (int)tempChar.hp + (itemData[1] * 2 + 1);
            tempChar.hp = Mathf.Min(tempChar.maxHp, targetHp);
        }
   

        characterData.Event_SpendItem(_dst.myIndex);
        _src.SetItemData_byData(null);
    }

}
