using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDM_Default : MonoBehaviour, iRoot_DDO_Manager
{
    [SerializeField] internal RDM_DefaultValue _REF;

    public SGT_GUI_ItemData GetInvenSGT()
    {
        return _REF._SGT_GUI_ItemData;
    }

    public bool IsDragObjExist()
    {
        return _REF.targetDDO != null;
    }

    public void ReturnToInit_EndDrag()
    {
        if (IsDragObjExist() == false)
            return;

        _REF._inputM.setDuringState(false);
        _REF._itemTrans.SetParent(_REF._defaultParent, false);
        _REF.targetDDO.InteractDDO_byGetRBD(_REF.currRBD);
        setDefaultState();
        _REF._itemTrans = null; _REF.currRBD = null;
    }

    public void SetEvent_OnEnter(IResponedByDrop _currRBD)
    {
        if (_REF._itemTrans == null)
        {
            if (_REF.targetDDO as iInvenSlot == _currRBD as iInvenSlot)
            {
                _currRBD.GetTargetSlotGUI().SetColor_ONFOCUS();
                return;
            }
            else
            {
                _currRBD.GetTargetSlotGUI().SetColor_ONFOCUS();
                return;
            }
        }
        else
        {
            if (_REF.targetDDO as iInvenSlot != _currRBD as iInvenSlot)
            {

                if (_REF.targetDDO.IsInteractable_byGetRBD(this, _currRBD))
                {
                    _REF.currRBD = _currRBD;
                    _currRBD.GetTargetSlotGUI().SetColor_ABLE();
                }
                else
                    _currRBD.GetTargetSlotGUI().SetColor_DISABLE();

                return;
            }
            else if (_currRBD as RBD_CasherZone)
            {
                // ¾Æ¸¶ cash°¡ null
                if (_REF.targetDDO.IsInteractable_byGetRBD(this, _currRBD))
                {
                    _REF.currRBD = _currRBD;
                    _currRBD.GetTargetSlotGUI().SetColor_ABLE();
                }
                else
                    _currRBD.GetTargetSlotGUI().SetColor_DISABLE();

                return;
            }
        }
    }

    public void SetEvent_OnExit(IResponedByDrop _currRBD = null)
    {
        _currRBD.GetTargetSlotGUI().SetColor_DEFAULT();
        setDefaultState();
    }

    public void SetSlotTransform_OnDrag(IDragDropObj _targetDDO)
    {
        if (_targetDDO.GetTransform_ItemGUI() == null)
            return;

        _REF._inputM.setDuringState(true);
        setDefaultState();
        _REF.targetDDO = _targetDDO;
        _REF._itemTrans = _targetDDO.GetTransform_ItemGUI();
        _REF._defaultParent = _REF._itemTrans.parent;
        _REF._itemTrans.SetParent(_REF._AboveOfAll, false);
    }

    void setDefaultState()
    {
        if (_REF.currRBD != null)
        {
            _REF.currRBD.GetTargetSlotGUI().SetColor_DEFAULT();
        }
        _REF.currRBD = null;
    }
}

[Serializable]
internal class RDM_DefaultValue
{
    [SerializeField] internal MyInputManager _inputM;
    [SerializeField] internal Transform _AboveOfAll;
    [SerializeField] internal SGT_GUI_ItemData _SGT_GUI_ItemData;
    internal Transform _itemTrans, _defaultParent;
    internal IDragDropObj targetDDO;
    internal IResponedByDrop currRBD;

    internal bool isDragingEvent()
    {
        return _itemTrans == null;
    }
}