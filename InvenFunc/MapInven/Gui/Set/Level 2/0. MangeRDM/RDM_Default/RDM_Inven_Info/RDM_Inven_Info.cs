using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class RDM_Inven_Info : MonoBehaviour, iRoot_DDO_Manager
{
    [SerializeField] internal RDM_ValueSet _REF;
    [SerializeField] internal MapRDM_ItemInfoCtrl mapRDM_ItemInfoCtrl;
    public GUI_InvenSetManager m_inven;
    public InputCtrl_withRDM ctrl;

    public void SetClickEvent_RDM(SlotGUI_InvenSlot _gui)
    {
        ctrl.SetClickEvent(_gui);
    }

    public void SetSlotTransform_OnDrag(IDragDropObj _targetDDO)
    {
        if (_targetDDO.GetTransform_ItemGUI() == null)
            return;

        _REF._inputM.setDuringState(false);
        ctrl.SetSlotTransform_OnDrag(_targetDDO);
        ctrl.SetParent_ItemTrans(_REF._AboveOfAll.transform);
        mapRDM_ItemInfoCtrl.SetItemInfo_byItemDataUnit(ctrl.GetMyFocusingItem_toShowInfo());
    }


    public void ReturnToInit_EndDrag()
    {
        Debug.Log("ReturnToInit_EndDrag");
        if (ctrl.IsDragObjExist() == false)
            return;

        _REF._inputM.setDuringState(false);

        ctrl.ReturnToInit_EndDrag();

        mapRDM_ItemInfoCtrl.SetItemInfo_byItemDataUnit(ctrl.GetMyFocusingItem_toShowInfo());
    }


    public void SetEvent_OnEnter(IResponedByDrop _currRBD)
    {
        Debug.Log("SetEvent_OnEnter");
        if (_currRBD as iInvenSlot != null)
        {
            if ((_currRBD as iInvenSlot).GetTransform_ItemGUI() != null)
            {
                ctrl.SetEvent_OnEnter(_currRBD);
            }
        }


        if (!ctrl.IsDragObjExist())
        {
            // 드래그 하지도 않음
            if (ctrl.IsSameDrag(_currRBD))
            {
                _currRBD.GetTargetSlotGUI().SetColor_ONFOCUS();
                mapRDM_ItemInfoCtrl.SetItemInfo_byItemDataUnit(ctrl.GetMyFocusingItem_toShowInfo());

                return;
            }
            else
            {
                _currRBD.GetTargetSlotGUI().SetColor_ONFOCUS();
                mapRDM_ItemInfoCtrl.SetItemInfo_byItemDataUnit(ctrl.GetMyFocusingItem_toShowInfo());

                return;
            }
        }
        else
        {
            // 드래그 대상 존재
            if (!ctrl.IsSameDrag(_currRBD))
            {
                // 대상이 인벤 슬롯인가
                if (ctrl.targetDDO.IsInteractable_byGetRBD(this, _currRBD))
                {
                    ctrl.setCurrRBD(_currRBD);
                    _currRBD.GetTargetSlotGUI().SetColor_ABLE();
                }
                else
                {
                    ctrl.setCurrRBD(null);
                    _currRBD.GetTargetSlotGUI().SetColor_DISABLE();
                }
                mapRDM_ItemInfoCtrl.SetItemInfo_byItemDataUnit(ctrl.GetMyFocusingItem_toShowInfo());

                return;
            }
            else if (_currRBD as RBD_CasherZone)
            {
                // 대상이 인벤 슬롯인가
                if (ctrl.targetDDO.IsInteractable_byGetRBD(this, _currRBD))
                {
                    ctrl.setCurrRBD(_currRBD);
                    _currRBD.GetTargetSlotGUI().SetColor_ABLE();
                }
                else
                    _currRBD.GetTargetSlotGUI().SetColor_DISABLE();

                mapRDM_ItemInfoCtrl.SetItemInfo_byItemDataUnit(ctrl.GetMyFocusingItem_toShowInfo());

                return;
            }
        }
    }

    public void SetEvent_OnExit(IResponedByDrop _currRBD = null)
    {
        _currRBD.GetTargetSlotGUI().SetColor_DEFAULT();
        ctrl.SetEvent_OnExit(_currRBD);
        mapRDM_ItemInfoCtrl.SetItemInfo_byItemDataUnit(ctrl.GetMyFocusingItem_toShowInfo());
    }

    public SGT_GUI_ItemData GetInvenSGT()
    {
        return _REF._SGT_GUI_ItemData;
    }

    public bool IsDragObjExist()
    {
        return ctrl.IsDragObjExist();
    }
}

[Serializable]
internal class RDM_ValueSet
{
    [SerializeField] internal MyInputManager _inputM;
    [SerializeField] internal Canvas _AboveOfAll;
    [SerializeField] internal SGT_GUI_ItemData _SGT_GUI_ItemData;
}