internal static class _TradeFuncSet_DDO
{
    internal static void SetEffect_TradeAble(this RDM_ShopSC inven, SlotGUI_ShopGoods src)
    {
        inven._REF.Inven_M.GoldEffect.SetImg_TradeAble();
    }

    internal static void SetEffect_TradeDisable(this RDM_ShopSC inven, SlotGUI_ShopGoods src)
    {
        inven._REF.Inven_M.GoldEffect.SetImg_TradeDisable();
    }

    internal static void SetEffect_TradeDefault(this RDM_ShopSC inven)
    {
        inven._REF.Inven_M.GoldEffect.SetImg_TradeDefault();
    }

    internal static void Check_DragStart(this RDM_ShopSC inven, IDragDropObj src)
    {
        if (src is SlotGUI_ShopGoods shopGoods)
        {
            inven.ShowGoldPrice(shopGoods._itemGUI._myData.GoldValue);
        }
    }

    internal static void Check_DragEnd(this RDM_ShopSC inven, IDragDropObj src)
    {
        if (src is SlotGUI_ShopGoods shopGoods)
        {
            inven.CloseGoldPrice(shopGoods._itemGUI._myData.GoldValue);
        }
    }

    internal static bool BuyItem_byDragDrop(this RDM_ShopSC inven, SlotGUI_ShopGoods src, SlotGUI_InvenSlot dst)
    {
        if (!inven.IsCanBuy_compGold(src))
        {
            return false;
        }

        inven.PurchaseItemEffect(src._itemGUI._myData.GoldValue);
        inven.GetInvenSGT().AddItemUnit_byPurchase(src._itemGUI._myData);

        GUI_ItemUnit srcGUI = src._itemGUI;
        dst.SetGUI_byItemGUI(srcGUI);
        src.SetGUI_byItemGUI(null);

        return true;
    }

    internal static void Sell_Item_byBtnClick(this RDM_ShopSC inven, SlotGUI_InvenSlot src)
    {
        ItemUnit tempItem = src._itemGUI._myData;

        inven._REF.GoldEFF.GoldEffect_byTrans(src.transform, inven._REF.Inven_M.GoldEffect.GetGoldTextTransform());
        inven.GainGoldEffect(tempItem.GoldValue);
        inven.GetInvenSGT().RemoveItem_byItem(tempItem);
        src.SetItemData_byData(null);
    }

    internal static bool IsCanBuy_compGold(this RDM_ShopSC inven, SlotGUI_ShopGoods item)
    {
        return inven._REF.InvenSC.invenData_SGT.GetGold() > item._itemGUI._myData.GoldValue;
    }
}
