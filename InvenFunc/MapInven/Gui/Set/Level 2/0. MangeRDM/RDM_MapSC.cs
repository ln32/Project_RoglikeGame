using UnityEngine;

public class RDM_MapSC : MonoBehaviour, iRoot_DDO_Manager
{
    [SerializeField] protected MyInputManager inputManager;
    [SerializeField] protected GUI_InvenSetManager inventoryManager;
    [SerializeField] protected Transform aboveOfAll;
    [SerializeField] internal MapRDM_ItemInfoCtrl mapItemInfoCtrl;
    [SerializeField] internal InvenSC_Map inventorySC;

    protected InputCtrl_withRDM control;

    public void SetClickEvent_RDM(SlotGUI_InvenSlot gui)
    {
        control.SetClickEvent(gui);
    }

    public void SetSlotTransform_OnDrag(IDragDropObj targetDDO)
    {
        if (targetDDO.GetTransform_ItemGUI() == null)
            return;

        inputManager.setDuringState(false);
        control.SetSlotTransform_OnDrag(targetDDO);
        control.SetParent_ItemTrans(aboveOfAll);
        UpdateItemInfo();
    }

    public void ReturnToInit_EndDrag()
    {
        if (!control.IsDragObjExist())
            return;

        inputManager.setDuringState(false);
        control.ReturnToInit_EndDrag();
        UpdateItemInfo();
    }

    public void SetEvent_OnEnter(IResponedByDrop currentRBD)
    {
        if (currentRBD as iInvenSlot != null && (currentRBD as iInvenSlot).GetTransform_ItemGUI() != null)
        {
            control.SetEvent_OnEnter(currentRBD);
        }

        if (!control.IsDragObjExist())
        {
            HandleNonDragEvent(currentRBD);
        }
        else
        {
            HandleDragEvent(currentRBD);
        }
    }

    public void SetEvent_OnExit(IResponedByDrop currentRBD = null)
    {
        currentRBD?.GetTargetSlotGUI().SetColor_DEFAULT();
        control.SetEvent_OnExit(currentRBD);
        UpdateItemInfo();
    }

    public SGT_GUI_ItemData GetInvenSGT()
    {
        return inventorySC.invenData_SGT;
    }

    public bool IsDragObjExist()
    {
        return control.IsDragObjExist();
    }

    private void HandleNonDragEvent(IResponedByDrop currentRBD)
    {
        currentRBD.GetTargetSlotGUI().SetColor_ONFOCUS();
        UpdateItemInfo();
    }

    private void HandleDragEvent(IResponedByDrop currentRBD)
    {
        if (!control.IsSameDrag(currentRBD))
        {
            if (control.targetDDO.IsInteractable_byGetRBD(this, currentRBD))
            {
                control.SetCurrRBD(currentRBD);
                currentRBD.GetTargetSlotGUI().SetColor_ABLE();
            }
            else
            {
                control.SetCurrRBD(null);
                currentRBD.GetTargetSlotGUI().SetColor_DISABLE();
            }
            UpdateItemInfo();
        }
        else if (currentRBD as RBD_CasherZone)
        {
            if (control.targetDDO.IsInteractable_byGetRBD(this, currentRBD))
            {
                control.SetCurrRBD(currentRBD);
                currentRBD.GetTargetSlotGUI().SetColor_ABLE();
            }
            else
            {
                currentRBD.GetTargetSlotGUI().SetColor_DISABLE();
            }
            UpdateItemInfo();
        }
    }

    private void UpdateItemInfo()
    {
        mapItemInfoCtrl.SetItemInfo_byItemDataUnit(control.GetMyFocusingItem_toShowInfo());
    }
}
