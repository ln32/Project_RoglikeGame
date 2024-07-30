using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_ItemUnit : MonoBehaviour
{
    [SerializeField] internal ItemUnit _myData;
    [SerializeField] internal Values_GUI _GUI;

    public void SetSizeAuto(Transform target = null)
    {
        transform.SetParent(target ?? transform.parent);

        Transform myTrans = transform;

        if (target != null)
            myTrans.SetParent(target);

        RectTransform _rect = GetComponent<RectTransform>();

        myTrans.localPosition = Vector3.zero;
        myTrans.localScale = Vector3.one;
    }

    public void SetNameText(string name)
    {
        _GUI.nameText.text = name;
    }

    public void SetGui_toIngredient(Sprite diceSprite, Color diceColor)
    {
        _GUI.img_Equip.gameObject.SetActive(false);
        _GUI.img_DiceSub.gameObject.SetActive(true);
        _GUI.img_DiceMain.gameObject.SetActive(true);
        _GUI.img_DiceMain.sprite = diceSprite;
        _GUI.img_DiceSub.color = diceColor;
    }

    public string GetNameText()
    {
        return _GUI.nameText.text;
    }

    public void SetAddressData(List<int> address)
    {
        _myData.invenAddr = address;
    }

    public Sprite GetImageGUI_Sprite()
    {
        return _GUI.img_Main.sprite;
    }

    public Material GetImageGUI_Material()
    {
        return _GUI.img_Main.material;
    }

    public void SetImageGUI_toSprite(Sprite target)
    {
        _GUI.img_Main.sprite = target;
        return;
    }
    public void SetImageGUI_toMaterial(Material target)
    {
        _GUI.img_Main.material = target;
        return;
    }
}

[Serializable]
internal class Values_GUI
{
    internal TextMeshProUGUI nameText;
    internal Image img_Main;
    internal Material color_Focused, color_Default,color_Cash;
    internal Image img_Equip, img_DiceMain, img_DiceSub;
}