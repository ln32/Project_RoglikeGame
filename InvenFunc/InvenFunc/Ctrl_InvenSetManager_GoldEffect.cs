using UnityEngine;

public class Ctrl_InvenSetManager_GoldEffect : GoldEffectFuncPrivate
{
    private void Start()
    {
        targetGold = SGT_GUI_ItemData.GetInstance().GetCurrGold();
        currGold = SGT_GUI_ItemData.GetInstance().GetCurrGold();
        _GUI.myText.text = "" + targetGold;
    }

    public void PurchaseGoodsFunc(int itemPrice)
    {
        CheckCurrEffect(PurchaseGoodsEffect());
    }

    public void ClosePriceFunc(int itemPrice)
    {
        CheckCurrEffect(ClosePriceEffect(itemPrice));
    }

    public void ShowPriceFunc(int itemPrice)
    {
        currShowedPrice = itemPrice;
        CheckCurrEffect(ShowPriceEffect(itemPrice));
    }

    public void GainGold(int _targetGold)
    {
        CheckCurrEffect(GainGoldEffect(_targetGold));
    }

    public void SetImg_TradeAble()
    {
        SetPanelColor(_GUI.ableColor);
    }

    public void SetImg_TradeDisable()
    {
        SetPanelColor(_GUI.disableColor);
    }

    public void SetImg_TradeDefault()
    { 
        SetPanelColor(_GUI.defaultColor);
    }

    public Transform GetGoldTextTransform()
    {
        return _GUI.myText.transform;
    }
}
