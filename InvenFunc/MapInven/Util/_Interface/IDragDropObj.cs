using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IDragDropObj : IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Transform GetTransform_ItemGUI();
    iRoot_DDO_Manager GetDDO_Manager();

    bool IsInteractable_byGetRBD(iRoot_DDO_Manager _inven, IResponedByDrop target);
    
    void InteractDDO_byGetRBD(IResponedByDrop target);

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        GetDDO_Manager().SetSlotTransform_OnDrag(this);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (GetTransform_ItemGUI() == false)
        {
            return;
        }
        else
        {
            Transform target = GetTransform_ItemGUI();
            target.position = eventData.position;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (GetTransform_ItemGUI() == null)
            return;

        GetDDO_Manager().ReturnToInit_EndDrag();
    }
}