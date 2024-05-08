using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InvenSC_Map : MonoBehaviour
{
    public SGT_GUI_ItemData invenData_SGT;
    public CharacterManager _cm;
    public GUI_InvenSetManager invenGUI_Manager;
    public List<ItemUnit> ItemList_Data;


    void Awake()
    {
        invenData_SGT.InitSGT(ref invenData_SGT);
        ItemList_Data = invenData_SGT.itemUnits;
    }

    private void Start()
    {

        setCharData_DEBUG();
        setGUI_bySGT();

        void setCharData_DEBUG()
        {
            Debug.Log("getCharData_DEBUG");

            if (_cm == null)
                _cm = FindObjectOfType<CharacterManager>();

            if (_cm == null)
            {
                Debug.Log("wognsdk cm sjgdjfk");
                return;
            }
        }
    }

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


    internal void addGUI_byData(ItemUnit data)
    {
        SlotGUI_InvenSlot targetSlot = invenGUI_Manager.GetSlotGUI_byAddr(data.invenAddr);
        GUI_ItemUnit ins_ItemGUI = invenData_SGT.spriteDataSet.GetGUI_byItemData(data.itemData, invenGUI_Manager._InsTrans);

        targetSlot.SetGUI_byItemGUI(ins_ItemGUI);
        targetSlot.SetItemData_byData(data);
    }

    [SerializeField] internal SetOfGUI_Map setGUI_Map;
    public void setGUI_bySGT()
    {  
        for (int i = 0; i < ItemList_Data.Count; i++)
        {
            SlotGUI_InvenSlot _SetInvenSlot = invenGUI_Manager.GetSlotGUI_byAddr(ItemList_Data[i].invenAddr);

            if (_SetInvenSlot != null)
            {
                GUI_ItemUnit _GUI_ItemUnit = invenData_SGT.spriteDataSet.GetGUI_byItemData(ItemList_Data[i].itemData, invenGUI_Manager._InsTrans);

                _SetInvenSlot.SetGUI_byItemGUI(_GUI_ItemUnit);
                _SetInvenSlot.SetItemData_byData(ItemList_Data[i]);
            }
        }

        setGUI_Map.MapGUI_GoldTopBar.text = SGT_GUI_ItemData.GetSGT().currGold + "";
        setGUI_Map.MapGUI_GoldInven.text = SGT_GUI_ItemData.GetSGT().currGold + ""; 
    }
}

[Serializable]
internal class SetOfGUI_Map
{
    public TextMeshProUGUI MapGUI_UserName;
    public TextMeshProUGUI MapGUI_CurrNode;
    public TextMeshProUGUI MapGUI_GoldTopBar;
    public TextMeshProUGUI MapGUI_GoldInven;
}