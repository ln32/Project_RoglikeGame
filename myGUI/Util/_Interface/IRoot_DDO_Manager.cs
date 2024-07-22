using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iRoot_DDO_Manager
{
    public SGT_GUI_ItemData GetInvenSGT();

    public void SetSlotTransform_OnDrag(IDragDropObj _targetDDO);

    public void ReturnToInit_EndDrag();

    public bool IsDragObjExist();

    public void SetEvent_OnEnter(IResponedByDrop _currRBD);

    public void SetEvent_OnExit(IResponedByDrop _currRBD);
}

