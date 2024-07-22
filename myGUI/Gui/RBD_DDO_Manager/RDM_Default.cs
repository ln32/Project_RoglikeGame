using System;
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
        if (!IsDragObjExist())
            return;

        _REF._inputM.setDuringState(false);
        _REF.itemTrans.SetParent(_REF.defaultParent, false);
        _REF.targetDDO.InteractDDO_byGetRBD(_REF.currRBD);
        SetDefaultState();
        _REF.itemTrans = null;
        _REF.currRBD = null;
    }

    public void SetEvent_OnEnter(IResponedByDrop _currRBD)
    {
        if (_REF.itemTrans == null)
        {
            HandleNonDragEnter(_currRBD);
        }
        else
        {
            HandleDragEnter(_currRBD);
        }
    }

    public void SetEvent_OnExit(IResponedByDrop _currRBD = null)
    {
        _currRBD?.GetTargetSlotGUI().SetColor_DEFAULT();
        SetDefaultState();
    }

    public void SetSlotTransform_OnDrag(IDragDropObj _targetDDO)
    {
        if (_targetDDO.GetTransform_ItemGUI() == null)
            return;

        _REF._inputM.setDuringState(true);
        SetDefaultState();
        _REF.targetDDO = _targetDDO;
        _REF.itemTrans = _targetDDO.GetTransform_ItemGUI();
        _REF.defaultParent = _REF.itemTrans.parent;
        _REF.itemTrans.SetParent(_REF._AboveOfAll, false);
    }

    private void HandleNonDragEnter(IResponedByDrop _currRBD)
    {
        if (_REF.targetDDO as iInvenSlot == _currRBD as iInvenSlot)
        {
            _currRBD.GetTargetSlotGUI().SetColor_ONFOCUS();
        }
        else
        {
            _currRBD.GetTargetSlotGUI().SetColor_ONFOCUS();
        }
    }

    private void HandleDragEnter(IResponedByDrop _currRBD)
    {
        if (_REF.targetDDO as iInvenSlot != _currRBD as iInvenSlot)
        {
            if (_REF.targetDDO.IsInteractable_byGetRBD(this, _currRBD))
            {
                _REF.currRBD = _currRBD;
                _currRBD.GetTargetSlotGUI().SetColor_ABLE();
            }
            else
            {
                _currRBD.GetTargetSlotGUI().SetColor_DISABLE();
            }
        }
        else if (_currRBD is RBD_CasherZone)
        {
            if (_REF.targetDDO.IsInteractable_byGetRBD(this, _currRBD))
            {
                _REF.currRBD = _currRBD;
                _currRBD.GetTargetSlotGUI().SetColor_ABLE();
            }
            else
            {
                _currRBD.GetTargetSlotGUI().SetColor_DISABLE();
            }
        }
    }

    private void SetDefaultState()
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
    internal Transform itemTrans, defaultParent;
    internal IDragDropObj targetDDO;
    internal IResponedByDrop currRBD;

    internal bool isDragingEvent()
    {
        return itemTrans == null;
    }
}
