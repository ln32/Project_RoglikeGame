using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_IngridiSlot : MonoBehaviour
{
    public int value;

    public Image GUI_myImg;
    public Color GUI_DisableColor;
    public Sprite defaultImg;

    public Image GUI_EffectImg;
    public Color GUI_EffectColor;


    internal void SetDefault()
    {
        GUI_myImg.color = GUI_DisableColor;
        GUI_EffectImg.color = GUI_DisableColor;
        GUI_myImg.sprite = defaultImg;
        value = -1;
    }
}
