using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class InputCtrl_withRDM : MonoBehaviour
{
    [SerializeField] private Transform _itemTrans, _defaultParent;
    [SerializeField] private bool b_targetDDO, b_currRBD;
    internal IDragDropObj targetDDO;
    private IResponedByDrop currRBD;

    [SerializeField] internal SlotGUI_InvenSlot _ClickGui;
    public void SetClickEvent(SlotGUI_InvenSlot _gui)
    {
        if(_ClickGui != null)
            _ClickGui.MSG_u_r_Infocusing(false);

        if (_gui == null) { _ClickGui = null; return; }


        if (_gui.GetMyItemGUI()._itemGUI == null)
        {
            _ClickGui = null;
        }
        else
        {
            _ClickGui = _gui;
            _ClickGui.MSG_u_r_Infocusing(true);
        }
    }

    public void SetSlotTransform_OnDrag(IDragDropObj _targetDDO)
    {
        setDefaultState();
        set_targetDDO(_targetDDO);
        _itemTrans = _targetDDO.GetTransform_ItemGUI();
        _defaultParent = _itemTrans.parent;

        Debug.Log("SetSlotTransform_OnDrag");
    }

    public void ReturnToInit_EndDrag()
    {
        SetClickEvent(null);

        _itemTrans.SetParent(_defaultParent, false);
        targetDDO.InteractDDO_byGetRBD(currRBD);
        set_targetDDO(null);
        set_currRBD(null);

        _itemTrans = null; _defaultParent = null;

        Debug.Log("ReturnToInit_EndDrag");
    }

    public void SetEvent_OnEnter(IResponedByDrop _currRBD)
    {
        setCurrRBD(_currRBD);

        if (_ClickGui != null)
            _ClickGui.MSG_u_r_Infocusing(true);
    }

    public void SetEvent_OnExit(IResponedByDrop _currRBD = null)
    {
        Debug.Log("SetEvent_OnExit");
        set_currRBD(null);

        if (_ClickGui != null)
            _ClickGui.MSG_u_r_Infocusing(true);
    }

    public void SetParent_ItemTrans(Transform _trans)
    {
        _itemTrans.SetParent(_trans, false);
    }

    public bool IsDragObjExist()
    {
        return targetDDO != null;
    }

    public bool IsSameDrag(IResponedByDrop _currRBD)
    {
        return targetDDO as iInvenSlot == _currRBD as iInvenSlot;
    }

    public void setDefaultState()
    {
        if (currRBD != null)
            currRBD.GetTargetSlotGUI().SetColor_DEFAULT();

        set_currRBD(null);
        if (_ClickGui != null)
            _ClickGui.MSG_u_r_Infocusing(true);
    }

    public void setCurrRBD(IResponedByDrop _currRBD)
    {
        set_currRBD(_currRBD);
    }

    void set_targetDDO(IDragDropObj _targetDDO)
    {
        b_targetDDO = (_targetDDO != null);
        targetDDO = _targetDDO;
    }

    void set_currRBD(IResponedByDrop _currRBD)
    {
        b_currRBD = (_currRBD != null);
        currRBD = _currRBD;
    }

    public iInvenSlot GetMyFocusingItem_toShowInfo()
    {
        iInvenSlot _currRBD = currRBD as iInvenSlot;
        if (_currRBD != null)
        {
            if(_currRBD.GetTransform_ItemGUI() == null &&_ClickGui != null)
                return _ClickGui;

            return _currRBD;
        }

        if (_ClickGui != null)
        {
            return _ClickGui;
        }

        return null;
    }

    void OnEnable()
    {
        SetClickEvent(null);
    }
}