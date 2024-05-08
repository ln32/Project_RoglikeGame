using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SGT_GUI_ItemData : MonoBehaviour
{
    private static SGT_GUI_ItemData dataSGT;
    public int currGold;
    public List<ItemUnit> itemUnits = new();
    [SerializeField] internal MyInvenSpriteDB spriteDataSet;

    public void InitSGT(ref SGT_GUI_ItemData _localData)
    {
        if (dataSGT == null)
        {
            DontDestroyOnLoad(_localData);
            dataSGT = _localData;
            itemUnits = _InvenDataEncoder.GetData_toItemList();
            currGold = _InvenDataEncoder.GetData_toGoldValue();
        }
        else
        {
            if (dataSGT == _localData)
            {
                return;
            }

            GameObject temp = _localData.gameObject;
            _localData = dataSGT;
            Destroy(temp);
        }
    }

    public void AddItemUnit_byPurchase(ItemUnit data)
    {
        itemUnits.Add(data);
    }


    static public SGT_GUI_ItemData GetSGT()
    {
        return dataSGT;
    }

    static internal void GetCharInvenSGT(int _CharIndex, ref string[] skillEquipped, ref string[] weaponEquipped)
    {
        List<ItemUnit> _itemUnits = dataSGT.itemUnits;

        if (weaponEquipped == null)
            weaponEquipped = new string[1] { "Null" };

        for (int i = 0; i < _itemUnits.Count; i++)
        {
            if (_itemUnits[i].invenAddr[0] == _CharIndex)
            {
                int skill_Index = _itemUnits[i].invenAddr[1];

                if (skill_Index < 2)
                {
                    string ItemCode = ConvSkillData_toString(_itemUnits[i].itemData);
                    skillEquipped[skill_Index] = (ItemCode);
                }
                else if (skill_Index < 4)
                {
                    string ItemCode = ConvEquipData_toString(_itemUnits[i].itemData);
                    weaponEquipped[skill_Index - 2] = (ItemCode);
                }
            }
        }

        return;
        

        string ConvSkillData_toString(List<int> target)
        {
            string rtnStr = "";
            if (target[0] != 2)
                return "null";

            switch (target[1])
            {
                case 0:
                    rtnStr += "Power_";
                    break;
                case 1:
                    rtnStr += "Sustain_";
                    break;
                case 2:
                    rtnStr += "Util_";
                    break;
                default:
                    break;
            }
            rtnStr += target[2];
            rtnStr += ":::"+target[3];
            return rtnStr;
        }

        string ConvEquipData_toString(List<int> target)
        {
            string rtnStr = "";

            switch (target[0])
            {
                case 0:
                    rtnStr += "Equuip_0_";
                    break;
                case 1:
                    rtnStr += "Equuip_1_";
                    break;
                case 2:
                    rtnStr += "Equip_2_";
                    break;
                default:
                    break;
            }

            rtnStr += target[1] + " / "; 
            rtnStr += target[2];
            return rtnStr;
        }
    }
}

[Serializable]
public class ItemUnit
{
    static int _index = 0;
    static public void SetIndex_byItemList(int target)
    {
        _index = target;
    }

    public int index = -1;
    public string itemName;

    // Item Type 1  : weapon skill food
    // Item Type 2  : R G B
    // Level        : 1 2 3
    public List<int> itemData = new();
    public List<int> invenAddr = new();
    public int GoldValue;

    internal void InitData_Random_Slot(SlotGUI_InvenSlot _slot)
    {
        initData_Index();
        initData_ItemData();
        GoldValue = UnityEngine.Random.Range(50, 150);
        invenAddr = _slot.myAddr;

        void initData_Index()
        {
            index = _index++;
            itemName = "Rand_" + index;
        }
    }

    internal void InitData_Random_Goods()
    {
        initData_Index();
        initData_ItemData();
        GoldValue = UnityEngine.Random.Range(50, 150);

        void initData_Index()
        {
            index = _index++;
            itemName = "RandGoods_" + index;
        }
    }

    internal void InitData_Random_Cooks(int _input = -1)
    {
        initData_Index();
        initData_ItemData_Cook();
        GoldValue = UnityEngine.Random.Range(50, 150);

        void initData_Index()
        {
            index = _index++;
            itemName = "RandGoods_" + index;
        }

        void initData_ItemData_Cook()
        {
            if (SGT_GUI_ItemData.GetSGT() == null)
                return;

            MyInvenSpriteDB _ItemDataTable = SGT_GUI_ItemData.GetSGT().spriteDataSet;

            if (true)
            {
                itemData.Add(0);
                itemData.Add(1);
            }


            while (true)
            {
                int temp = 0;

                temp = _ItemDataTable.GetCount_byItemData(itemData);
                if (temp < 0)
                    break;

                itemData.Add(UnityEngine.Random.Range(0, temp));
            }

            if (true)
            {
                if (_input != -1)
                {
                    itemData.RemoveAt(itemData.Count - 1);
                    itemData.Add(_input);

                }
            }
        }
    }

    void initData_ItemData()
    {
        MyInvenSpriteDB _ItemDataTable = SGT_GUI_ItemData.GetSGT().spriteDataSet;

        int index_0 = _ItemDataTable.GetCount_byItemData(itemData);
        if (true)
        {
            index_0 = UnityEngine.Random.Range(0, index_0);
            itemData.Add(index_0);

            if (index_0 == 0)
            {
                itemData.Add(0);
            }
        }


        while (true)
        {
            int temp = 0;

            temp = _ItemDataTable.GetCount_byItemData(itemData);
            if (temp < 0)
                break;

            itemData.Add(UnityEngine.Random.Range(0, temp));
        }

        if (true)
        {
            if (index_0 != 0)
            {
                itemData.RemoveAt(itemData.Count - 1);
                itemData.Add(0);
            }
        }
    }

    public void SetIndex_byPurchase()
    {
        initData_Index();
        itemName = "Purchase - " + index;

        void initData_Index()
        {
            index = _index++;
            itemName = "Rand_" + index;
        }
    }
}