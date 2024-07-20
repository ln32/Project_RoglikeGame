using System;
using TreeEditor;
using UnityEngine;

public class RDM_ShopSC : MonoBehaviour, iRoot_DDO_Manager
{
    [SerializeField] internal ShopSC_REF _REF;
    [SerializeField] Transform aboveOfAll;
    [SerializeField] Transform itemTrans,defaultParent;
    [SerializeField] IDragDropObj targetDDO;
    [SerializeField] IResponedByDrop currRBD;

    public void SetSlotTransform_OnDrag(IDragDropObj targetDDO)
    {
        if (targetDDO.GetTransform_ItemGUI() == null)
            return;

        this.Check_DragStart(targetDDO);

        _REF.Input_M.setDuringState(true);
        setDefaultState();

        this.targetDDO = targetDDO;
        itemTrans = targetDDO.GetTransform_ItemGUI();
        defaultParent = itemTrans.parent;

        this.SetGUI_OnDrag(targetDDO);
        itemTrans.SetParent(aboveOfAll, false);
    }
    public void ReturnToInit_EndDrag()
    {
        if (!IsDragObjExist())
            return;

        this.Check_DragEnd(targetDDO);

        _REF.Input_M.setDuringState(false);

        this.SetGUI_EndDrag(targetDDO);
        itemTrans.SetParent(defaultParent, false);

        targetDDO.InteractDDO_byGetRBD(currRBD);
        setDefaultState();
    }

    public bool IsDragObjExist()
    {
        return targetDDO != null;
    }

    public void SetEvent_OnEnter(IResponedByDrop _currRBD)
    {
        if (itemTrans == null)
        {
            HandleNonDragEvent(currRBD);
        }
        else
        {
            HandleDragEvent(currRBD);
        }

        void HandleNonDragEvent(IResponedByDrop currRBD)
        {
            if (targetDDO as iInvenSlot == currRBD as iInvenSlot)
            {
                currRBD.GetTargetSlotGUI().SetColor_ONFOCUS();
            }
        }

        void HandleDragEvent(IResponedByDrop currRBD)
        {
            if (targetDDO as iInvenSlot != currRBD as iInvenSlot)
            {
                UpdateSlotGUIState(currRBD);
            }
            else if (currRBD as RBD_CasherZone)
            {
                UpdateSlotGUIState(currRBD);
            }
        }

        void UpdateSlotGUIState(IResponedByDrop currRBD)
        {
            if (targetDDO.IsInteractable_byGetRBD(this, currRBD))
            {
                this.currRBD = currRBD;
                currRBD.GetTargetSlotGUI().SetColor_ABLE();
            }
            else
            {
                this.currRBD = null;
                currRBD.GetTargetSlotGUI().SetColor_DISABLE();
            }
        }
    }

    public void SetEvent_OnExit(IResponedByDrop _currRBD = null)
    {
        currRBD?.GetTargetSlotGUI().SetColor_DEFAULT();
        this.SetEffect_TradeDefault();
        setDefaultState();
    }

    public void ShowGoldPrice(int price)
    {
        _REF.Inven_M.GoldEffect.ShowPriceFunc(price);
    }

    public void CloseGoldPrice(int price)
    {
        _REF.Inven_M.GoldEffect.ClosePriceFunc(price);
    }

    public SGT_GUI_ItemData GetInvenSGT()
    {
        return _REF.InvenSC.invenData_SGT;
    }

    public void PurchaseItemEffect(int target)
    {
        _REF.Inven_M.GoldEffect.PurchaseGoodsFunc(target);
        UpdateGold(-target);
    }

    public void GainGoldEffect(int target)
    {
        _REF.Inven_M.GoldEffect.GainGold(target);
        UpdateGold(target);
    }



    private void UpdateGold(int amount)
    {
        int currentGold = _REF.InvenSC.invenData_SGT.GetGold();
        _REF.InvenSC.invenData_SGT.SetGold(currentGold + amount);
    }


    private void setDefaultState()
    {
        if (currRBD != null)
        {
            currRBD.GetTargetSlotGUI().SetColor_DEFAULT();
        }
        currRBD = null;
    }
}

[Serializable]
public class ShopSC_REF{
    public MyInputManager Input_M;
    public GUI_InvenSetManager Inven_M;
    public RBD_CasherZone RBD_Info;
    public InvenSC_Shop InvenSC;
    public Effect_PopGather GoldEFF;
}