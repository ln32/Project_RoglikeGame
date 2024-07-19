using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSC_Shop : MonoBehaviour
{
    public SGT_GUI_ItemData invenData_SGT;
    public GUI_InvenSetManager invenGUI_Manager;
    public List<ItemUnit> ItemList_Data;
    
    void Awake()
    {
        invenData_SGT.InitSGT(ref invenData_SGT);
        ItemList_Data = invenData_SGT.GetItemUnitList();
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
            for (int i = 0; i < ItemList_Data.Count; i++)
            {
                SlotGUI_InvenSlot temp = invenGUI_Manager.GetSlotGUI_byAddr(ItemList_Data[i].invenAddr);

                if (temp)
                {
                    GUI_ItemUnit insObj = invenData_SGT.spriteDataSet.GetGUI_byItemData(ItemList_Data[i].itemData, invenGUI_Manager._InsTrans);
                    temp.SetGUI_byItemGUI(insObj);
                    temp.SetItemData_byData(ItemList_Data[i]);
                }
            }
        }
    }

    void SetNewGUI_byData(ItemUnit _data)
    {
        GUI_ItemUnit sad = invenData_SGT.spriteDataSet.GetGUI_byItemData(_data.itemData, invenGUI_Manager._InsTrans);
        SlotGUI_InvenSlot temp = invenGUI_Manager.GetSlotGUI_byAddr(_data.invenAddr);
        temp.SetItemData_byData(null);
        temp.SetGUI_byItemGUI(sad);
        temp.SetItemData_byData(_data);
    }
}