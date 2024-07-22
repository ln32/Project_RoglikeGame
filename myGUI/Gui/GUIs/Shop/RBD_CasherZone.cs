using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static _GUI_ItemUnitFunc;

public class RBD_CasherZone : MonoBehaviour, IResponedByDrop
{
    public TextMeshProUGUI text;
    public ItemUnit currItem;
    [SerializeField] internal GUI_Ctrl myGUI_CTRL;

    private iRoot_DDO_Manager cash;

    public iRoot_DDO_Manager GetDDO_Manager()
    {
        return cash ??= transform.root.GetComponent<iRoot_DDO_Manager>();
    }

    public iSlotGUI GetTargetSlotGUI()
    {
        return myGUI_CTRL;
    }

    public void Sell_Item()
    {
        if (currItemGUI == null || isInven == null || isInven._itemGUI == null || currItem == null)
        {
            return;
        }

        if (GetDDO_Manager() is RDM_ShopSC shopSC)
        {
            shopSC.Sell_Item_byBtnClick(isInven);
        }
        ClearCurrentState();
    }

    private GUI_ItemUnit currItemGUI;
    private SlotGUI_InvenSlot isInven;

    public void DDO_Event_byInvenSlot(SlotGUI_InvenSlot inputSlot)
    {
        ClearCurrentState();
        currItemGUI = inputSlot._itemGUI;
        isInven = inputSlot;

        this.SetGUI_OnFocus(currItemGUI);
        SetItemInfo_byGUI(currItemGUI);
    }

    public void SetItemInfo_ShopGoods(SlotGUI_ShopGoods slot)
    {
        ClearCurrentState();
        currItemGUI = slot._itemGUI;
        isInven = null;

        this.SetGUI_OnFocus(currItemGUI);
        SetItemInfo_byGUI(currItemGUI);
    }

    private void SetItemInfo_byGUI(GUI_ItemUnit currItemGUI)
    {
        var inputData = currItemGUI._myData;
        text.text =
            $" {inputData.itemName}\n" +
            $"Type = {inputData.itemData[0]}\n" +
            $"Color = {inputData.itemData[1]}\n" +
            $"Level = {inputData.itemData[2]}\n" +
            $"Price = {inputData.GoldValue}\n";
    }

    private void ClearCurrentState()
    {
        if (currItemGUI == null) return;

        this.SetGUI_Default(currItemGUI);
        text.text = " .";
        currItemGUI.SetImageGUI_toMaterial(null);
        currItemGUI = null;
        isInven = null;
    }
}
