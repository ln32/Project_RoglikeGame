using System;
using UnityEngine;

public class RDM_ShopSC : MonoBehaviour, iRoot_DDO_Manager
{
    [SerializeField]  internal ShopSC_REF _REF;
    public Transform _AboveOfAll;
    private Transform _itemTrans,_defaultParent; 
    private IDragDropObj targetDDO;
    private IResponedByDrop currRBD;

    public void SetSlotTransform_OnDrag(IDragDropObj _targetDDO)
    {
        if (_targetDDO.GetTransform_ItemGUI() == null)
            return;

        this.Check_DragStart(_targetDDO);

        _REF.Input_M.setDuringState(true);
        setDefaultState();
        targetDDO = _targetDDO;
        _itemTrans = _targetDDO.GetTransform_ItemGUI();
        _defaultParent = _itemTrans.parent;

        this.SetGUI_OnDrag(targetDDO);
        _itemTrans.SetParent(_AboveOfAll, false);
    }

    public void ReturnToInit_EndDrag()
    {
        if (IsDragObjExist() == false)
            return;

        this.Check_DragEnd(targetDDO);

        _REF.Input_M.setDuringState(false);

        this.SetGUI_EndDrag(targetDDO);
        _itemTrans.SetParent(_defaultParent,false);

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
                
                if (targetDDO.IsInteractable_byGetRBD(this,_currRBD))
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
        this.SetEffect_TradeDefault();
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
        _REF.Inven_M.GoldEffect.PurchaseGoodsFunc(target);

        int currGold = _REF.InvenSC.invenData_SGT.GetGold();
        _REF.InvenSC.invenData_SGT.SetGold(currGold - target);
    }

    public void GainGoldEffect(int target)
    {
        _REF.Inven_M.GoldEffect.GainGold(target);

        int currGold = _REF.InvenSC.invenData_SGT.GetGold();
        _REF.InvenSC.invenData_SGT.SetGold(currGold + target);
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
}

[Serializable]
public class ShopSC_REF{
    public MyInputManager Input_M;
    public GUI_InvenSetManager Inven_M;
    public RBD_CasherZone RBD_Info;
    public InvenSC_Shop InvenSC;
    public Effect_PopGather GoldEFF;
}