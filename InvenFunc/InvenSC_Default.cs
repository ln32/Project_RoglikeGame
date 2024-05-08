using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSC_Default : MonoBehaviour
{
    public SGT_GUI_ItemData invenData_SGT;
    public GUI_InvenSetManager invenGUI_Manager;
    public List<ItemUnit> ItemList_Data;
        
    void Start()
    {
        invenData_SGT.InitSGT(ref invenData_SGT);
        ItemList_Data = invenData_SGT.itemUnits;
        setGUI_bySGT();
    }

    [ContextMenu("AddItem_JustRandom")]
    public void AddItem_Debug()
    {
        SlotGUI_InvenSlot targetSlot = invenGUI_Manager.GetSlotGUI_byMin();

        if (!targetSlot)
        {
            Debug.Log("Inven is Full"); return;
        }

        ItemUnit itemData = new ItemUnit();
        itemData.InitData_Random_Slot(targetSlot);
        ItemList_Data.Add(itemData);
        addGUI_byData(itemData);
    }

    private void addGUI_byData(ItemUnit data)
    {
        SlotGUI_InvenSlot targetSlot = invenGUI_Manager.GetSlotGUI_byAddr(data.invenAddr);
        GUI_ItemUnit ins_ItemGUI = invenData_SGT.spriteDataSet.GetGUI_byItemData(data.itemData, invenGUI_Manager._InsTrans);

        targetSlot.SetGUI_byItemGUI(ins_ItemGUI);
        targetSlot.SetItemData_byData(data);
    }

    public void setGUI_bySGT()
    {
        if (ItemList_Data != null)
        {
            int maxIndex = 0;
            for (int i = 0; i < ItemList_Data.Count; i++)
            {
                SlotGUI_InvenSlot _SetInvenSlot = invenGUI_Manager.GetSlotGUI_byAddr(ItemList_Data[i].invenAddr);

                if (_SetInvenSlot != null)
                {
                    GUI_ItemUnit _GUI_ItemUnit = invenData_SGT.spriteDataSet.GetGUI_byItemData(ItemList_Data[i].itemData, invenGUI_Manager._InsTrans);
                    _SetInvenSlot.SetGUI_byItemGUI(_GUI_ItemUnit);
                    _SetInvenSlot.SetItemData_byData(ItemList_Data[i]);

                    if (maxIndex < _GUI_ItemUnit._myData.index)
                        maxIndex = _GUI_ItemUnit._myData.index;
                }
            }

            ItemUnit.SetIndex_byItemList(maxIndex + 1);
        }
    }
}
