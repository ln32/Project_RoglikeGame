using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFunc_InvenCtrl : MonoBehaviour
{
    public InvenSC_Map _invenSC;

    public void AddFunc_Debug()
    {
        SlotGUI_InvenSlot targetSlot = _invenSC.invenGUI_Manager.GetSlotGUI_byMin();

        if (!targetSlot)
        {
            Debug.Log("Inven is Full"); return;
        }

        ItemUnit itemData = new ItemUnit();
        itemData.InitData_Random_Slot(targetSlot);
        _invenSC.ItemList_Data.Add(itemData);
        _invenSC.addGUI_byData(itemData);
    }

    public void AddFunc_Debug1()
    {
        if (true)
        {
            int tempValue = Random.Range(5, 10);

            CharacterData targetChar = null;
            if (true)
            {
                List<CharacterData> _AvailableList = new();

                for (int i = 0; i < 3; i++)
                {
                    CharacterData temp_CharacterData = CJH_CharacterData.getSGT().getCharData(i);
                    if (temp_CharacterData.hp > tempValue)
                        _AvailableList.Add(temp_CharacterData);
                }

                if (true)
                {
                    if (_AvailableList.Count == 0)
                    {
                        Debug.Log("HP Dead"); return;
                    }
                    int index = Random.Range(0, _AvailableList.Count);
                    targetChar = _AvailableList[index];
                }
            }
            targetChar.hp -= tempValue;



        }

        ItemUnit target_ItemUnit = null;
        if (true)
        {
            List<ItemUnit> _ItemUnit = _invenSC.invenData_SGT.itemUnits;
            List<ItemUnit> _TempNew = new List<ItemUnit>();
            for (int i = 0; i < _ItemUnit.Count; i++)
            {
                var targetItem = _ItemUnit[i];
                bool myCheck = true;
                if (true)
                {
                    bool isIt_onInven = targetItem.invenAddr[0] == 0;
                    bool isNotIngredient = targetItem.itemData[0] != 0;
                    bool isNotMaxLevel = targetItem.itemData[_ItemUnit[i].itemData.Count - 1] != 2;

                    myCheck = isIt_onInven && isNotIngredient && isNotMaxLevel;
                }

                if (myCheck)
                {
                    _TempNew.Add(_ItemUnit[i]);
                }
            }

            if (_TempNew.Count == 0)
            {
                Debug.Log("sad2"); return;
            }

            if (true)
            {
                int index = Random.Range(0, _TempNew.Count);
                target_ItemUnit = _TempNew[index];
            }
        }
        target_ItemUnit.itemData[target_ItemUnit.itemData.Count - 1]++;

        SlotGUI_InvenSlot sad1 = _invenSC.invenGUI_Manager.GetSlotGUI_byAddr(target_ItemUnit.invenAddr);
        GUI_ItemUnit sad2 = _invenSC.invenData_SGT.spriteDataSet.GetGUI_byItemData(target_ItemUnit.itemData, _invenSC.invenGUI_Manager._InsTrans);
        sad1.SetItemData_byData(null);
        sad1.SetGUI_byItemGUI(sad2);
    }

    public void AddFunc_Debug2()
    {
        if (true)
        {
            int tempValue = Random.Range(5, 10);

            CharacterData targetChar = null;
            if (true)
            {
                List<CharacterData> _AvailableList = new();

                for (int i = 0; i < 3; i++)
                {
                    CharacterData temp_CharacterData = CJH_CharacterData.getSGT().getCharData(i);
                    if (temp_CharacterData.hp > tempValue)
                        _AvailableList.Add(temp_CharacterData);
                }

                if (true)
                {
                    if (_AvailableList.Count == 0)
                    {
                        Debug.Log("sad"); return;
                    }
                    int index = Random.Range(0, _AvailableList.Count);
                    targetChar = _AvailableList[index];
                }
            }
            targetChar.hp -= tempValue;

        }

        AddFunc_Debug();
    }
}
