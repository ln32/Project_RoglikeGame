using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUI_ItemUnit : MonoBehaviour
{
    public ItemUnit _myData;
    [SerializeField] internal Values_GUI _GUI;

    public void SetSizeAuto(Transform target = null)
    {
        Transform myTrans = transform;

        if (target != null)
            myTrans.SetParent(target);

        RectTransform _rect = GetComponent<RectTransform>();

        myTrans.localPosition = Vector3.zero;
        myTrans.localScale = Vector3.one;
    }

    public void SetNameText(string input)
    {
        _GUI.nameText.text = input;
    }

    public void SetGui_ToIngredient(Sprite _TargetDiceSpr,Color _TargetColor)
    {
        _GUI.img_equip.gameObject.SetActive(false);
        _GUI.img_DiceSub.gameObject.SetActive(true);
        _GUI.img_DiceMain.gameObject.SetActive(true);
        _GUI.img_DiceMain.sprite = _TargetDiceSpr;
        _GUI.img_DiceSub.color = _TargetColor;
    }

    public string GetNameText()
    {
        return _GUI.nameText.text;
    }

    public void SetAddressData(List<int> _addr)
    {
        _myData.invenAddr = _addr;
    }

    public Sprite GetImageGUI_Sprite()
    {
        return _GUI.img_Main.sprite;
    }
    public Material GetImageGUI_Material()
    {
        return _GUI.img_Main.material;
    }
    public void SetImageGUI_Sprite(Sprite target)
    {
        _GUI.img_Main.sprite = target;
        return;
    }
    public void SetImageGUI_Material(Material target)
    {
        _GUI.img_Main.material = target;
        return;
    }
    public void SetImageGUI_Color(Color target)
    {
        _GUI.img_Main.color = target;
        return;
    }
    public void SetImageGUI_toNull()
    {
        return;
    }
}

[Serializable]
internal class Values_GUI
{
    public TextMeshProUGUI nameText;
    public Image img_Main;
    public Material color_Focused, color_Default,color_Cash;
    public Image img_equip, img_DiceMain, img_DiceSub;
}