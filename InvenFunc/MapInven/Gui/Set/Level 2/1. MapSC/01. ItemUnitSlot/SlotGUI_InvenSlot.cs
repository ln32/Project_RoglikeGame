using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SlotGUI_InvenSlot : MonoBehaviour, iInvenSlot, IPointerDownHandler
{
    [SerializeField] private GUI_Ctrl myGUI_CTRL;
    [SerializeField] private _SlotItemGuiEvent _ItemEvent;

    public List<int> myAddr = new();
    public GUI_ItemUnit _itemGUI; 
    public Image _ExistEffect;
    public int _index;


    public void SetState_onInit(int index, List<int> _data)
    {
        _index = index;
        myAddr = new();
        for (int i = 0; i < _data.Count; i++)
        {
            myAddr.Add(_data[i]);
        }
        myAddr.Add(index);

        _itemGUI = GetComponentInChildren<GUI_ItemUnit>();

        if (_itemGUI)
            _itemGUI.SetAddressData(myAddr);
    }

    public SlotGUI_InvenSlot GetMyItemGUI() { return this; }



    private iRoot_DDO_Manager cash = null;
    public iRoot_DDO_Manager GetDDO_Manager()
    {
        if(cash != null)
            return cash;

        return transform.root.GetComponent<iRoot_DDO_Manager>();
    }


    public Transform GetTransform_ItemGUI()
    {
        if (_itemGUI == null)
            return null;

        return _itemGUI.transform;
    }

    public iSlotGUI GetTargetSlotGUI()
    {
        return myGUI_CTRL;
    }

    public bool IsInteractable_byGetRBD(iRoot_DDO_Manager _inven, IResponedByDrop target)
    {
        return _inven.CheckUpAvailable(this,target);
    }

    public void InteractDDO_byGetRBD(IResponedByDrop _src)
    {
        if (true)
        {
            if ((_src as SlotGUI_EquipSlot == true))
            {
                GetDDO_Manager().InteractFuncByRBD(this, _src as SlotGUI_EquipSlot);
                return;
            }

            else if ((_src as SlotGUI_InvenSlot == true))
            {
                GetDDO_Manager().InteractFuncByRBD(this, _src as SlotGUI_InvenSlot);
                return;
            }

            else if ((_src as RBD_CasherZone == true))
            {
                GetDDO_Manager().InteractFuncByRBD(this, _src as RBD_CasherZone);
                return;
            }

            else if ((_src as RBD_SellZone == true))
            {
                GetDDO_Manager().InteractFuncByRBD(this, _src as RBD_SellZone);
                return;
            }

            else if ((_src as RBD_IngridimentSlot == true))
            {
                GetDDO_Manager().InteractFuncByRBD(this, _src as RBD_IngridimentSlot);
                return;
            }

            else if ((_src as RBD_UseDisposable == true))
            {
                GetDDO_Manager().InteractFuncByRBD(this, _src as RBD_UseDisposable);
                return;
            }
        }

        //  Not Allocated Case
        GetDDO_Manager().InteractFuncByRBD(this);
        return;
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

        RectTransform _RectTransform = target.transform.GetComponent<RectTransform>();
        _RectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
    }

    // on click
    public void OnPointerDown(PointerEventData eventData)
    {
        RDM_MapSC _MapSC = GetDDO_Manager() as RDM_MapSC;
        if (_MapSC != null)
        {
            _MapSC.SetClickEvent_RDM(this);
        }
    }

    public void MSG_u_r_Infocusing(bool _input)
    {
        if(_input)
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
        if (slotEff != null)
            slotEff.event_DEFAULT.Invoke();
    }
    public void SetColor_ONFOCUS()
    {
        if (slotEff != null)
            slotEff.event_ONFOCUS.Invoke();
    }
    public void SetColor_ABLE()
    {
        if (slotEff != null)
            slotEff.event_ABLE.Invoke();
    }
    public void SetColor_DISABLE()
    {
        if (slotEff != null)    
            slotEff.event_DISABLE.Invoke();
    }
    public void SetColor_INFO()
    {
        if (slotEff != null)
            slotEff.event_INFO.Invoke();
    }
}