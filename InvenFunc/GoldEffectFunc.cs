using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldEffectFunc : MonoBehaviour
{
    [SerializeField] GoldValues_GUI _GUI;
    public int currGold;
    public int targetGold;
    public float currTimeLength, timer_ShowPrice;
    public float currCoef, priceCoef;

    public float delayTime;
    delegate void ClearFunc(); ClearFunc clearFunc;
    IEnumerator currEffect = null;

    private void Start()
    {
        targetGold = currGold = SGT_GUI_ItemData.GetSGT().currGold;
        _GUI.myText.text = "" + targetGold;
    }

    public void ShowPriceFunc(int itemPrice)
    {
        currShowedPrice = itemPrice;
        checkCurrEffect(ShowPriceEffect(itemPrice));
    }

    IEnumerator ShowPriceEffect(int itemPrice)
    {
        clearFunc = new(() => { setPriceAlpha(255); });
        float time = 0f;
        _GUI.semiGoldText_1.text = "- "+itemPrice;

        while (true)
        {
            time += Time.deltaTime;

            if (time > timer_ShowPrice)
                break;

            int temp = (int)Mathf.Lerp(0, 255, Mathf.Pow(time / timer_ShowPrice, 0.4f));
            setPriceAlpha(temp);

            yield return new WaitForSeconds(0.05f);
        }

        if (clearFunc != null)
        {
            clearFunc();
            clearFunc = null;
        }
    }

    public void ClosePriceFunc(int itemPrice)
    {
        checkCurrEffect(ClosePriceEffect(itemPrice));
    }

    IEnumerator ClosePriceEffect(int itemPrice)
    {
        float time = 0f;
        clearFunc = new(() => { setPriceAlpha(0); });

        while (true)
        {
            time += Time.deltaTime;

            if (time > timer_ShowPrice)
                break;

            int temp = (int)Mathf.Lerp(0, 200, 1 - Mathf.Pow(time / timer_ShowPrice, priceCoef));
            setPriceAlpha(temp);

            yield return new WaitForSeconds(0.05f);
        }

        if (clearFunc != null)
        {
            clearFunc();
            clearFunc = null;
        }
    }

    public void PurchaseGoodsFunc(int itemPrice)
    {
        checkCurrEffect(PurchaseGoodsEffect());
    }

    IEnumerator PurchaseGoodsEffect()
    {
        clearFunc = new(() => {
            currGold = targetGold;
            _GUI.myText.text = "" + currGold;
        });

        int _currGold = currGold;
        targetGold = _currGold - currShowedPrice;
        float time = 0f;

        while (true)
        {
            time += Time.deltaTime;
            currGold = (int)Mathf.Lerp(_currGold, targetGold, Mathf.Pow(time / timer_ShowPrice, 0.5f));
            _GUI.myText.text = "" + (currGold);

            if (time > priceCoef)
                break;

            yield return new WaitForSeconds(0.05f);
        }

        if (clearFunc != null)
        {
            clearFunc();
            clearFunc = null;
        }
    }

    public void GainGold(int _targetGold)
    {
        checkCurrEffect(GainGoldEffect(_targetGold));
    }

    IEnumerator GainGoldEffect(int _targetGold)
    {
        clearFunc = new(() => {
            currGold = targetGold;
            _GUI.myText.text = "" + currGold;
        });

        int _currGold = currGold;
        targetGold = _currGold + _targetGold;
        float time = 0f;

        yield return new WaitForSeconds(delayTime);

        while (true)
        {
            time += Time.deltaTime;
            currGold = (int)Mathf.Lerp(_currGold, targetGold, Mathf.Pow(time / timer_ShowPrice, 0.5f));
            _GUI.myText.text = "" + (currGold);

            if (time > priceCoef)
                break;

            yield return new WaitForSeconds(0.05f);
        }

        if (clearFunc != null)
        {
            clearFunc();
            clearFunc = null;
        }
    }

    void checkCurrEffect(IEnumerator target)
    {
        if (clearFunc != null)
        {
            clearFunc();
            clearFunc = null;
        }

        if (currEffect != null)
        {
            StopCoroutine(currEffect);
        }

        currEffect = target;
        StartCoroutine(target);
    }

    int currShowedPrice = 0;

    void setPriceAlpha(int ratio)
    {
        Color32 temp = new Color32(0, 0, 0, 0);
      
        temp.a = (byte)ratio;

        _GUI.semiGoldText_1.color = temp;
        _GUI.semiGoldText_2.color = temp;
    }

    public Transform getGoldTextTransform()
    {
        return _GUI.myText.transform;
    }

    public void SetImg_TradeAble()
    {
        _GUI.goldPanal.color = _GUI.ableColor;
    }
    public void SetImg_TradeDisable()
    {
        _GUI.goldPanal.color = _GUI.disableColor;
    }
    public void SetImg_TradeDefault()
    {
        _GUI.goldPanal.color = _GUI.defaultColor;
    }
}

[Serializable]
internal class GoldValues_GUI{
    public TextMeshProUGUI myText;
    public TextMeshProUGUI semiGoldText_1, semiGoldText_2;
    public Image goldPanal;
    public Color defaultColor, ableColor, disableColor; 
}
