using System;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class GUI_IngridiSlotManager : MonoBehaviour
{
    [SerializeField] internal GUI_IngridiSlotValues _values;
    public IngredientPoker _IngredientPoker;

    [ContextMenu("CookFunc_onIngridiment")]
    public void CookFunc_onIngridiment()
    {
        List<RBD_IngridimentSlot> _slots = _values.RBD_Slots;
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i]._GUI_ItemUnit == null)
                continue;
            GameObject target = _slots[i]._GUI_ItemUnit.gameObject;
            Destroy(target);
        }
        _IngredientPoker.SetDefault();
    }

    [ContextMenu("debug")]
    public void debugdebug()
    {
        List<RBD_IngridimentSlot> _slots = _values.RBD_Slots;
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i]._GUI_ItemUnit == null)
                continue;
            GameObject target = _slots[i]._GUI_ItemUnit.gameObject;
            target.transform.name = "dm";
        }
    }


    [ContextMenu("Event_Reset")]
    public void Event_Reset()
    {
        List<RBD_IngridimentSlot> _slots = _values.RBD_Slots;

        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].SetDefault();
        }
        _IngredientPoker.SetDefault();
        _IngredientPoker.SetColor_toDefault();
    }

    public void RefreshMyGUI()
    {
        List<Vector2Int> invenGUI = new List<Vector2Int>();
        for (int i = 0; i < _values.RBD_Slots.Count; i++)
        {
            invenGUI.Add( Vector2Int.one * -1);
        }

        for (int i = 0; i < _values.RBD_Slots.Count; i++)
        {
            if (_values.RBD_Slots[i]._GUI_ItemUnit == null)
                invenGUI[i] = Vector2Int.one * -1;
            else
            {
                List<int> asd = _values.RBD_Slots[i]._GUI_ItemUnit._myData.itemData;
               invenGUI[i] = new Vector2Int(asd[asd.Count-2], asd[asd.Count - 1]);
            }
        }

        _IngredientPoker.InitValues(invenGUI);
        List<Vector2Int> _rtnData = _IngredientPoker.Check_PatternList();
        _IngredientPoker.SetColor_byReturnData(_rtnData);
    }
}

[Serializable]
internal struct GUI_IngridiSlotValues
{
    [SerializeField] internal List<RBD_IngridimentSlot> RBD_Slots;

    internal bool checkCurr()
    {
        for (int i = 0; i < RBD_Slots.Count; i++)
        {
            if (RBD_Slots[i].myGUI_Slot.value < 0)
                return false;
        }

        return true;
    }

    internal int IsDisAvabibleValue(int data)
    {
        for (int i = 0; i < RBD_Slots.Count; i++)
        {
            if (RBD_Slots[i].myGUI_Slot.value == data)
                return i;
        }

        return -1;
    }


    internal int[] GetCookSlotsItemIndex()
    {
        int[] temp = new int[RBD_Slots.Count];

        for (int i = 0; i < temp.Length; i++)
        {
            if (RBD_Slots[i]._GUI_ItemUnit == null)
            {
                temp[i] = -1;
            }
            else
            {
                List<int> tempList  = RBD_Slots[i]._GUI_ItemUnit._myData.invenAddr;
                temp[i] = tempList[tempList.Count-1];
            }
        }

        return temp;
    }
}
