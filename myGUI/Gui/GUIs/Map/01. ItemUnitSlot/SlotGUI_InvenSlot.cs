using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotGUI_InvenSlot : MonoBehaviour, iInvenSlot, IPointerDownHandler
{
    internal GUI_ItemUnit _itemGUI;

    [SerializeField] private GUI_Ctrl myGUI_CTRL;
    [SerializeField] private SlotItemGuiEvent _ItemEvent;

    public List<int> myAddr { get; internal set; }
    protected Image _ExistEffect;
    protected int _index;

    private iRoot_DDO_Manager cash = null;

    public void SetState_onInit(int index, List<int> _data)
    {
        _index = index;
        myAddr = new List<int>(_data) { index };

        _itemGUI = GetComponentInChildren<GUI_ItemUnit>();
        _itemGUI?.SetAddressData(myAddr);
    }

    public SlotGUI_InvenSlot GetMyItemGUI() => this;

    public iRoot_DDO_Manager GetDDO_Manager()
    {
        return cash ??= transform.root.GetComponent<iRoot_DDO_Manager>();
    }

    public Transform GetTransform_ItemGUI() => _itemGUI?.transform;

    public iSlotGUI GetTargetSlotGUI() => myGUI_CTRL;

    public bool IsInteractable_byGetRBD(iRoot_DDO_Manager _inven, IResponedByDrop target)
    {
        return _inven.CheckUpAvailable(this, target);
    }

    public void InteractDDO_byGetRBD(IResponedByDrop _src)
    {
        var manager = GetDDO_Manager();
        switch (_src)
        {
            case SlotGUI_EquipSlot equipSlot:
                manager.InteractFuncByRBD(this, equipSlot);
                break;
            case SlotGUI_InvenSlot invenSlot:
                manager.InteractFuncByRBD(this, invenSlot);
                break;
            case RBD_CasherZone casherZone:
                manager.InteractFuncByRBD(this, casherZone);
                break;
            case RBD_SellZone sellZone:
                manager.InteractFuncByRBD(this, sellZone);
                break;
            case RBD_IngridimentSlot ingridimentSlot:
                manager.InteractFuncByRBD(this, ingridimentSlot);
                break;
            case RBD_UseDisposable useDisposable:
                manager.InteractFuncByRBD(this, useDisposable);
                break;
            default:
                manager.InteractFuncByRBD(this);
                break;
        }
    }

    public void SetGUI_byItemGUI(GUI_ItemUnit target)
    {
        _ItemEvent.SetGui_byIsNull(target == null);

        if (target == null)
        {
            _ExistEffect.enabled = false;
            _itemGUI = null;
            return;
        }

        _ExistEffect.enabled = true;
        _itemGUI = target;

        target.transform.SetParent(transform);
        target._myData.invenAddr = myAddr;
        target.SetSizeAuto();

        var rectTransform = target.transform.GetComponent<RectTransform>();
        rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetDDO_Manager() is RDM_MapSC mapSC)
        {
            mapSC.SetClickEvent_RDM(this);
        }
    }

    public void MSG_u_r_Infocusing(bool _input)
    {
        if (_input)
            myGUI_CTRL.SetColor_INFO();
        else
            myGUI_CTRL.SetColor_DEFAULT();
    }
}

[Serializable]
internal class GUI_Ctrl : iSlotGUI
{
    [SerializeField] internal SlotEffect_Default slotEff;

    public void SetColor_DEFAULT()
    {
        slotEff?.event_DEFAULT.Invoke();
    }

    public void SetColor_ONFOCUS()
    {
        slotEff?.event_ONFOCUS.Invoke();
    }

    public void SetColor_ABLE()
    {
        slotEff?.event_ABLE.Invoke();
    }

    public void SetColor_DISABLE()
    {
        slotEff?.event_DISABLE.Invoke();
    }

    public void SetColor_INFO()
    {
        slotEff?.event_INFO.Invoke();
    }
}
