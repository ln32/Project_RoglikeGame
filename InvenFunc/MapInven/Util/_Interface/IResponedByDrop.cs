using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IResponedByDrop : IPointerEnterHandler, IPointerExitHandler
{
    iRoot_DDO_Manager GetDDO_Manager();

    iSlotGUI GetTargetSlotGUI();

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        GetDDO_Manager().SetEvent_OnEnter(this);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        GetDDO_Manager().SetEvent_OnExit(this);
    }
}

public interface iSlotGUI
{
    public void SetColor_DEFAULT();
    public void SetColor_ONFOCUS();
    public void SetColor_ABLE();
    public void SetColor_DISABLE();
}