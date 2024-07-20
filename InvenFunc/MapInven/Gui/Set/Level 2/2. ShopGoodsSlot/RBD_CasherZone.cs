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

    private iRoot_DDO_Manager cash = null;

    public iRoot_DDO_Manager GetDDO_Manager()
    {
        if (cash != null)
            return cash;

        return transform.root.GetComponent<iRoot_DDO_Manager>();
    }

    public iSlotGUI GetTargetSlotGUI()
    {
        return myGUI_CTRL;
    }

    public void Sell_Item()
    {
        if(currItemGUI == null) return;
        if (isInven == null) return;
        if (isInven._itemGUI == null) return;
        if (currItem == null) return;


        RDM_ShopSC _RDM_ShopSC = GetDDO_Manager() as RDM_ShopSC;
        if (_RDM_ShopSC)
        {
            _RDM_ShopSC.Sell_Item_byBtnClick(isInven);
        }
        clearCurrState();
        return;
    }

    GUI_ItemUnit currItemGUI = null;
    SlotGUI_InvenSlot isInven = null;

    public void DDO_Event_byInvenSlot(SlotGUI_InvenSlot inputSlot)
    {
        // Info Event - SetItemInfo_InvenSlot
        clearCurrState();
        currItemGUI = inputSlot._itemGUI; isInven = inputSlot;

        this.SetGUI_OnFocus(currItemGUI);

        SetItemInfo_byGUI(inputSlot._itemGUI);
    }

    public void SetItemInfo_ShopGoods(SlotGUI_ShopGoods _slot)
    {
        clearCurrState();
        // Info Event - SetItemInfo_ShopGoods 

        currItemGUI = _slot._itemGUI; isInven = null;
        this.SetGUI_OnFocus(currItemGUI);

        SetItemInfo_byGUI(_slot._itemGUI);
    }

    void SetItemInfo_byGUI(GUI_ItemUnit _currItemGUI)
    {
        Sprite img = currItemGUI.GetImageGUI_Sprite();

        ItemUnit inputData = _currItemGUI._myData;
        text.text =
            " " + inputData.itemName + "\n" +
            "Type = " + inputData.itemData[0] + "\n" +
            "Color = " + inputData.itemData[1] + "\n" +
            "Level = " + inputData.itemData[2] + "\n" +
            "Price = " + inputData.GoldValue + "\n";
    }

    void clearCurrState()
    {
        if (currItemGUI == null)
            return;

        this.SetGUI_Default(currItemGUI);
        text.text = " .";
        currItemGUI.SetImageGUI_toMaterial(null);
        currItemGUI = null; 
        isInven = null;
    }
}