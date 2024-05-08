using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class RDM_MapSC : MonoBehaviour, iRoot_DDO_Manager
{
    public MyInputManager _inputM;
    public GUI_InvenSetManager m_inven;
    public Transform _AboveOfAll;
    [SerializeField] internal MapRDM_ItemInfoCtrl mapRDM_ItemInfoCtrl;

    public InputCtrl_withRDM ctrl;

    //private Transform _itemTrans,_defaultParent; 
    //private IDragDropObj targetDDO;
    //private IResponedByDrop currRBD;

    public void SetClickEvent_RDM(SlotGUI_InvenSlot _gui)
    {   
        ctrl.SetClickEvent(_gui);
    }

    public void SetSlotTransform_OnDrag(IDragDropObj _targetDDO)
    {
        if (_targetDDO.GetTransform_ItemGUI() == null)
            return;

        _inputM.setDuringState(false);
        ctrl.SetSlotTransform_OnDrag(_targetDDO);
        ctrl.SetParent_ItemTrans(_AboveOfAll);
        mapRDM_ItemInfoCtrl.SetItemInfo_byItemDataUnit(ctrl.GetMyFocusingItem_toShowInfo());
    }


    public void ReturnToInit_EndDrag()
    {
        if (ctrl.IsDragObjExist() == false)
            return;

        _inputM.setDuringState(false);

        ctrl.ReturnToInit_EndDrag();

        mapRDM_ItemInfoCtrl.SetItemInfo_byItemDataUnit(ctrl.GetMyFocusingItem_toShowInfo());
    }


    public void SetEvent_OnEnter(IResponedByDrop _currRBD)
    {
        if(_currRBD as iInvenSlot != null)
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

    public InvenSC_Map _invenSC;
    public SGT_GUI_ItemData GetInvenSGT()
    {
        return _invenSC.invenData_SGT;
    }

    public bool IsDragObjExist()
    {
        return ctrl.IsDragObjExist();
    }
}