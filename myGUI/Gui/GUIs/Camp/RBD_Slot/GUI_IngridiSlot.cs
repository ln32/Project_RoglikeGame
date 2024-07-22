using UnityEngine;
using UnityEngine.UI;

public class GUI_IngridiSlot : MonoBehaviour
{
    [SerializeField] internal int value;
    [SerializeField] internal Sprite defaultImg;

    [SerializeField] internal Image GUI_myImg;
    [SerializeField] internal Color GUI_DisableColor;


    [SerializeField] internal Image GUI_EffectImg;
    [SerializeField] internal Color GUI_EffectColor;


    internal void SetDefault()
    {
        GUI_myImg.color = GUI_DisableColor;
        GUI_EffectImg.color = GUI_DisableColor;
        GUI_myImg.sprite = defaultImg;
        value = -1;
    }
}
