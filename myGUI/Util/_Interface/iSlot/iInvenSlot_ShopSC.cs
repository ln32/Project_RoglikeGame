using System.Collections.Generic;
using UnityEngine;

public interface iInvenSlot_ShopSC : IDragDropObj, IResponedByDrop
{
    public void SetState(int index,List<int> _data);

    new Transform GetTransform_ItemGUI();

    new iRoot_DDO_Manager GetDDO_Manager();
}