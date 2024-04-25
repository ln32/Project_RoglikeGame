using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotEffect_MapInven : MonoBehaviour
{
    [SerializeField] internal ColorSet _color;
    public Image img;

    public void event_DEFAULT()
    {
        img.color = _color.DEFAULT;
    }

    public void event_ONFOCUS()
    {
        img.color = _color.ONFOCUS;
    }

    public void event_ABLE()
    {
        img.color = _color.ABLE;
    }

    public void event_DISABLE()
    {
        img.color = _color.DISABLE;
    }
    public void event_INFO()
    {
        img.color = _color.INFO_FOCUS;
    }
}
