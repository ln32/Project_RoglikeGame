using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldEffectFuncPrivate : MonoBehaviour
{
    [SerializeField] internal GoldValues_GUI _GUI;
    protected int currGold;
    protected int targetGold;
    protected float timer_ShowPrice, priceCoef, delayTime;
    protected int currShowedPrice = 0;

    delegate void ClearFunc();
    ClearFunc clearFunc;
    IEnumerator currEffect = null;



    protected IEnumerator ShowPriceEffect(int itemPrice)
    {
        InitializeEffect(() => SetPriceAlpha(255));
        _GUI.semiGoldText_1.text = "- " + itemPrice;
        yield return LerpAlphaEffect(0, 255, 0.4f);
    }

    
    protected IEnumerator ClosePriceEffect(int itemPrice)
    {
        InitializeEffect(() => SetPriceAlpha(0));
        yield return LerpAlphaEffect(0, 200, 1 - priceCoef);
    }


    protected IEnumerator PurchaseGoodsEffect()
    {
        InitializeEffect(() => UpdateGold(currGold, targetGold));
        targetGold = currGold - currShowedPrice;
        yield return LerpGoldEffect(currGold, targetGold, 0.5f);
    }


    protected void CheckCurrEffect(IEnumerator target)
    {
        clearFunc?.Invoke();
        clearFunc = null;

        if (currEffect != null)
        {
            StopCoroutine(currEffect);
        }

        currEffect = target;
        StartCoroutine(target);
    }

    protected void InitializeEffect(Action clearAction)
    {
        clearFunc = new ClearFunc(clearAction);
    }

    protected IEnumerator GainGoldEffect(int _targetGold)
    {
        InitializeEffect(() => UpdateGold(currGold, targetGold));
        targetGold = currGold + _targetGold;
        yield return new WaitForSeconds(delayTime);
        yield return LerpGoldEffect(currGold, targetGold, 0.5f);
    }

    protected IEnumerator LerpAlphaEffect(int startAlpha, int endAlpha, float power)
    {
        float time = 0f;

        while (time < timer_ShowPrice)
        {
            time += Time.deltaTime;
            int alpha = (int)Mathf.Lerp(startAlpha, endAlpha, Mathf.Pow(time / timer_ShowPrice, power));
            SetPriceAlpha(alpha);
            yield return new WaitForSeconds(0.05f);
        }

        clearFunc?.Invoke();
    }

    protected IEnumerator LerpGoldEffect(int startGold, int endGold, float power)
    {
        float time = 0f;

        while (time < timer_ShowPrice)
        {
            time += Time.deltaTime;
            currGold = (int)Mathf.Lerp(startGold, endGold, Mathf.Pow(time / timer_ShowPrice, power));
            UpdateGoldText(currGold);
            yield return new WaitForSeconds(0.05f);
        }

        clearFunc?.Invoke();
    }

    protected void SetPriceAlpha(int alpha)
    {
        _GUI.SetPriceAlpha(alpha);
    }



    protected void UpdateGold(int startGold, int endGold)
    {
        currGold = endGold;
        UpdateGoldText(currGold);
    }    

    protected void UpdateGoldText(int gold)
    {
        _GUI.UpdateGoldText(gold);
    }

    protected void SetPanelColor(Color color)
    {
        _GUI.SetPanelColor(color);
    }
}

[Serializable]
internal class GoldValues_GUI
{
    [SerializeField] public TextMeshProUGUI myText;
    [SerializeField] public TextMeshProUGUI semiGoldText_1, semiGoldText_2;
    [SerializeField] public Image goldPanal;
    [SerializeField] public Color defaultColor, ableColor, disableColor;

    internal void UpdateGoldText(int gold)
    {
        myText.text = gold.ToString();
    }

    internal void SetPanelColor(Color color)
    {
        goldPanal.color = color;
    }

    internal void SetPriceAlpha(int alpha)
    {
        Color32 color = new Color32(0, 0, 0, (byte)alpha);
        semiGoldText_1.color = color;
        semiGoldText_2.color = color;
    }

}
