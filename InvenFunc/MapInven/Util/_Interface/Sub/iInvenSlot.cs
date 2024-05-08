using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iInvenSlot : IDragDropObj, IResponedByDrop
{
    public void SetState_onInit(int index,List<int> _data);

    new Transform GetTransform_ItemGUI();

    new iRoot_DDO_Manager GetDDO_Manager();
}