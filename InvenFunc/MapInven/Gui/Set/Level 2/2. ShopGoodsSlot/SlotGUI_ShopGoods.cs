using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotGUI_ShopGoods : MonoBehaviour, IDragDropObj
{
    public GUI_ItemUnit _itemGUI;

    private iRoot_DDO_Manager cash = null;
    public iRoot_DDO_Manager GetDDO_Manager()
    {
        if (cash != null)
            return cash;

        return transform.root.GetComponent<iRoot_DDO_Manager>();
    }

    public Transform GetTransform_ItemGUI()
    {
        if (_itemGUI == null)
            return null;

        return _itemGUI.transform;
    }

    public void InteractDDO_byGetRBD(IResponedByDrop target)
    {
        if (true)
        {
            SlotGUI_EquipSlot check = target as SlotGUI_EquipSlot;
            if (check)
            {
                GetDDO_Manager().InteractFuncByRBD(this, check);
                return;
            }
        }


        if (true)
        {
            SlotGUI_InvenSlot check = target as SlotGUI_InvenSlot;
            if (check)
            {
                GetDDO_Manager().InteractFuncByRBD(this, check);
                return;
            }
        }

        if (true)
        {
            RBD_CasherZone check = target as RBD_CasherZone;
            if (check)
            {
                GetDDO_Manager().InteractFuncByRBD(this, check);
                return;
            }
        }

        _itemGUI.SetSizeAuto();
        return;
    }

    public bool IsInteractable_byGetRBD(iRoot_DDO_Manager _inven, IResponedByDrop target)
    {
        return _inven.CheckUpAvailable(this,target);
    }
    public void SetGUI_byItemGUI(GUI_ItemUnit target)
    {
        _itemGUI = target;

        if (target)
        {
            target.SetSizeAuto(transform);
            _itemGUI.SetAddressData(null);
        }
    }
}