using System;
using System.Collections.Generic;
using UnityEngine;

public class GUI_IngridiSlotManager : MonoBehaviour
{
    [SerializeField] internal GUI_IngridiSlotValues Values;
    [SerializeField] internal IngredientPoker IngredientPoker;

    public void CookFunc_onIngridiment()
    {
        foreach (var slot in Values.RBD_Slots)
        {
            if (slot._GUI_ItemUnit != null)
            {
                Destroy(slot._GUI_ItemUnit.gameObject);
            }
        }
        IngredientPoker.SetDefault();
    }

    public void Event_Reset()
    {
        foreach (var slot in Values.RBD_Slots)
        {
            slot.SetDefault();
        }
        IngredientPoker.SetDefault();
        IngredientPoker.SetColor_toDefault();
    }

    public void RefreshMyGUI()
    {
        List<Vector2Int> invenGUI = new List<Vector2Int>(Values.RBD_Slots.Count);

        for (int i = 0; i < Values.RBD_Slots.Count; i++)
        {
            var itemUnit = Values.RBD_Slots[i]._GUI_ItemUnit;
            invenGUI.Add(itemUnit == null
                ? Vector2Int.one * -1
                : new Vector2Int(itemUnit._myData.itemData[^2], itemUnit._myData.itemData[^1]));
        }

        IngredientPoker.InitValues(invenGUI);
        List<Vector2Int> _rtnData = IngredientPoker.Check_PatternList();
        IngredientPoker.SetColor_byReturnData(_rtnData);
    }
}

[Serializable]
internal struct GUI_IngridiSlotValues
{
    [SerializeField] internal List<RBD_IngridimentSlot> RBD_Slots;

    internal bool checkCurr()
    {
        return RBD_Slots.TrueForAll(slot => slot.myGUI_Slot.value >= 0);
    }

    internal int IsDisAvabibleValue(int data)
    {
        return RBD_Slots.FindIndex(slot => slot.myGUI_Slot.value == data);
    }

    internal int[] GetCookSlotsItemIndex()
    {
        return RBD_Slots.ConvertAll(slot =>
            slot._GUI_ItemUnit == null
                ? -1
                : slot._GUI_ItemUnit._myData.invenAddr[^1]
        ).ToArray();
    }
}
