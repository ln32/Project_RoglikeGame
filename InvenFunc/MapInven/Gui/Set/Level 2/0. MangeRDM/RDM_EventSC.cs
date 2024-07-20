using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDM_EventSC : MonoBehaviour, iRoot_DDO_Manager
{
    protected MyInputManager inputManager;
    protected GUI_InvenSetManager invenManager;
    protected RBD_CasherZone infoBox;
    protected InvenSC_Shop invenShop;
    protected Transform aboveOfAll;

    private Transform itemTransform, defaultParent;
    private IDragDropObj targetDDO;
    private IResponedByDrop currentRBD;

    public void SetSlotTransform_OnDrag(IDragDropObj targetDDO)
    {
        if (targetDDO.GetTransform_ItemGUI() == null)
            return;

        inputManager.setDuringState(true);
        ResetDefaultState();
        this.targetDDO = targetDDO;
        itemTransform = targetDDO.GetTransform_ItemGUI();
        defaultParent = itemTransform.parent;
        itemTransform.SetParent(aboveOfAll, false);
    }

    public void ReturnToInit_EndDrag()
    {
        if (!IsDragObjExist())
            return;

        inputManager.setDuringState(false);
        ResetItemTransform();
        targetDDO.InteractDDO_byGetRBD(currentRBD);
        ResetDefaultState();
        itemTransform = null;
        currentRBD = null;
    }

    public bool IsDragObjExist()
    {
        return targetDDO != null;
    }

    public void SetEvent_OnEnter(IResponedByDrop currentRBD)
    {
        if (itemTransform == null)
        {
            HandleEventForSameSlot(currentRBD);
        }
        else
        {
            HandleEventForDifferentSlot(currentRBD);
        }
    }

    public void SetEvent_OnExit(IResponedByDrop currentRBD = null)
    {
        currentRBD?.GetTargetSlotGUI().SetColor_DEFAULT();
        ResetDefaultState();
    }

    private void HandleEventForSameSlot(IResponedByDrop currentRBD)
    {
        if (targetDDO as iInvenSlot == currentRBD as iInvenSlot)
        {
            currentRBD.GetTargetSlotGUI().SetColor_ONFOCUS();
        }
        else
        {
            currentRBD.GetTargetSlotGUI().SetColor_ONFOCUS();
        }
    }

    private void HandleEventForDifferentSlot(IResponedByDrop currentRBD)
    {
        if (targetDDO as iInvenSlot != currentRBD as iInvenSlot)
        {
            if (targetDDO.IsInteractable_byGetRBD(this, currentRBD))
            {
                this.currentRBD = currentRBD;
                currentRBD.GetTargetSlotGUI().SetColor_ABLE();
            }
            else
            {
                currentRBD.GetTargetSlotGUI().SetColor_DISABLE();
            }
        }
        else if (currentRBD is RBD_CasherZone)
        {
            if (targetDDO.IsInteractable_byGetRBD(this, currentRBD))
            {
                this.currentRBD = currentRBD;
                currentRBD.GetTargetSlotGUI().SetColor_ABLE();
            }
            else
            {
                currentRBD.GetTargetSlotGUI().SetColor_DISABLE();
            }
        }
    }

    private void ResetDefaultState()
    {
        currentRBD?.GetTargetSlotGUI().SetColor_DEFAULT();
        currentRBD = null;
    }

    private void ResetItemTransform()
    {
        itemTransform.SetParent(defaultParent, false);
    }

    public void PurchaseItemEffect(int target)
    {
        invenManager.GoldEffect.PurchaseGoodsFunc(target);
        invenShop.invenData_SGT.SetGold(invenShop.invenData_SGT.GetGold() - target);
    }

    public void GainGoldEffect(int target)
    {
        invenManager.GoldEffect.GainGold(target);
        invenShop.invenData_SGT.SetGold(invenShop.invenData_SGT.GetGold() + target);
    }

    public SGT_GUI_ItemData GetInvenSGT()
    {
        return invenShop.invenData_SGT;
    }
}
