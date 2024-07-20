using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCtrl_withRDM : MonoBehaviour
{
    [SerializeField] private Transform itemTrans, defaultParent;
    [SerializeField] internal SlotGUI_InvenSlot clickGui;

    private IResponedByDrop currentRBD;
    internal IDragDropObj targetDDO;

    public void SetClickEvent(SlotGUI_InvenSlot gui)
    {
        clickGui?.MSG_u_r_Infocusing(false);

        if (gui == null || gui.GetMyItemGUI()._itemGUI == null)
        {
            clickGui = null;
        }
        else
        {
            clickGui = gui;
            clickGui.MSG_u_r_Infocusing(true);
        }
    }

    public void SetSlotTransform_OnDrag(IDragDropObj targetDDO)
    {
        SetDefaultState();
        SetTargetDDO(targetDDO);
        itemTrans = targetDDO.GetTransform_ItemGUI();
        defaultParent = itemTrans.parent;
    }

    public void ReturnToInit_EndDrag()
    {
        SetClickEvent(null);
        ResetItemTransform();
        targetDDO.InteractDDO_byGetRBD(currentRBD);
        ClearTargetDDO();
        ClearCurrentRBD();
    }

    public void SetEvent_OnEnter(IResponedByDrop currRBD)
    {
        SetCurrRBD(currRBD);
        clickGui?.MSG_u_r_Infocusing(true);
    }

    public void SetEvent_OnExit(IResponedByDrop currRBD = null)
    {
        ClearCurrentRBD();
        clickGui?.MSG_u_r_Infocusing(true);
    }

    public void SetParent_ItemTrans(Transform trans)
    {
        itemTrans.SetParent(trans, false);
    }

    public bool IsDragObjExist()
    {
        return targetDDO != null;
    }

    public bool IsSameDrag(IResponedByDrop currRBD)
    {
        return targetDDO as iInvenSlot == currRBD as iInvenSlot;
    }

    public void SetCurrRBD(IResponedByDrop currRBD)
    {
        currentRBD = currRBD;
    }

    public iInvenSlot GetMyFocusingItem_toShowInfo()
    {
        var currRBDSlot = currentRBD as iInvenSlot;
        if (currRBDSlot != null)
        {
            if (currRBDSlot.GetTransform_ItemGUI() == null && clickGui != null)
                return clickGui;

            return currRBDSlot;
        }

        return clickGui;
    }

    private void SetDefaultState()
    {
        currentRBD?.GetTargetSlotGUI().SetColor_DEFAULT();
        ClearCurrentRBD();
        clickGui?.MSG_u_r_Infocusing(true);
    }
    private void SetTargetDDO(IDragDropObj targetDDO)
    {
        this.targetDDO = targetDDO;
    }

    private void ClearTargetDDO()
    {
        targetDDO = null;
    }

    private void ClearCurrentRBD()
    {
        currentRBD = null;
    }

    private void ResetItemTransform()
    {
        itemTrans.SetParent(defaultParent, false);
        itemTrans = null;
        defaultParent = null;
    }

    private void OnEnable()
    {
        SetClickEvent(null);
    }
}
