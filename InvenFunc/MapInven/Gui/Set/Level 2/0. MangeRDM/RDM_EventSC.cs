using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDM_EventSC : MonoBehaviour, iRoot_DDO_Manager
{
    public MyInputManager _inputM;
    public GUI_InvenSetManager m_inven;
    public RBD_CasherZone InfoBox;
    public InvenSC_Shop _invenSC;
    public Transform _AboveOfAll;

    private Transform _itemTrans, _defaultParent;
    private IDragDropObj targetDDO;
    private IResponedByDrop currRBD;

    public void SetSlotTransform_OnDrag(IDragDropObj _targetDDO)
    {
        if (_targetDDO.GetTransform_ItemGUI() == null)
            return;

        _inputM.setDuringState(true);
        setDefaultState();
        targetDDO = _targetDDO;
        _itemTrans = _targetDDO.GetTransform_ItemGUI();
        _defaultParent = _itemTrans.parent;
        _itemTrans.SetParent(_AboveOfAll, false);
    }

    public void ReturnToInit_EndDrag()
    {
        if (IsDragObjExist() == false)
            return;

        _inputM.setDuringState(false);

        _itemTrans.SetParent(_defaultParent, false);
        targetDDO.InteractDDO_byGetRBD(currRBD);
        setDefaultState();
        _itemTrans = null; currRBD = null;
    }

    public bool IsDragObjExist()
    {
        return targetDDO != null;
    }

    public void SetEvent_OnEnter(IResponedByDrop _currRBD)
    {
        if (_itemTrans == null)
        {
            if (targetDDO as iInvenSlot == _currRBD as iInvenSlot)
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
            if (targetDDO as iInvenSlot != _currRBD as iInvenSlot)
            {

                if (targetDDO.IsInteractable_byGetRBD(this, _currRBD))
                {
                    currRBD = _currRBD;
                    _currRBD.GetTargetSlotGUI().SetColor_ABLE();
                }
                else
                    _currRBD.GetTargetSlotGUI().SetColor_DISABLE();

                return;
            }
            else if (_currRBD as RBD_CasherZone)
            {
                // ¾Æ¸¶ cash°¡ null
                if (targetDDO.IsInteractable_byGetRBD(this, _currRBD))
                {
                    currRBD = _currRBD;
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

    void setDefaultState()
    {
        if (currRBD != null)
        {
            currRBD.GetTargetSlotGUI().SetColor_DEFAULT();
        }
        currRBD = null;
    }

    public void PurchaseItemEffect(int target)
    {
        m_inven.GoldEffect.PurchaseGoodsFunc(target);
        _invenSC.invenData_SGT.currGold -= target;
    }

    public void GainGoldEffect(int target)
    {
        m_inven.GoldEffect.GainGold(target);
        _invenSC.invenData_SGT.currGold += target;
    }

    public SGT_GUI_ItemData GetInvenSGT()
    {
        return _invenSC.invenData_SGT;
    }
}