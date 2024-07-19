using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_ResultCookSet : MonoBehaviour
{
    public SlotGUI_CookResult MySlotList;
    public void FillUpResultCook(int _input)
    {
        SGT_GUI_ItemData invenData_SGT = SGT_GUI_ItemData.GetInstance();

        ItemUnit itemData = new ItemUnit();
        itemData.InitData_Random_Cooks(_input);

        GUI_ItemUnit ins_ItemGUI = invenData_SGT.spriteDataSet.GetGUI_byItemData(itemData.itemData, MySlotList.transform);
        MySlotList.SetGUI_byItemGUI(ins_ItemGUI);
        MySlotList.SetItemData_byData(itemData);
    }

    public void FillUpResultCook()
    {
        SGT_GUI_ItemData invenData_SGT = SGT_GUI_ItemData.GetInstance();

        ItemUnit itemData = new ItemUnit();
        itemData.InitData_Random_Cooks();

        GUI_ItemUnit ins_ItemGUI = invenData_SGT.spriteDataSet.GetGUI_byItemData(itemData.itemData, MySlotList.transform);
        MySlotList.SetGUI_byItemGUI(ins_ItemGUI);
        MySlotList.SetItemData_byData(itemData);
    }
}
