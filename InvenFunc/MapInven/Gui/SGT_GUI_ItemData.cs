using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGT_GUI_ItemData : MonoBehaviour
{
    private static SGT_GUI_ItemData instance;

    [SerializeField] private List<ItemUnit> itemUnits = new List<ItemUnit>();
    [SerializeField] internal MyInvenSpriteDB spriteDataSet;
    private int currGold = 0;

    public void InitSGT(ref SGT_GUI_ItemData localData)
    {
        if (instance == null)
        {
            DontDestroyOnLoad(localData);
            instance = localData;
            itemUnits = _InvenDataEncoder.GetData_toItemList();
            currGold = _InvenDataEncoder.GetData_toGoldValue();
        }
        else if (instance != localData)
        {
            Destroy(localData.gameObject);
            localData = instance;
        }
    }

    public void AddItemUnit_byPurchase(ItemUnit item)
    {
        itemUnits.Add(item);
    }

    public static SGT_GUI_ItemData GetInstance()
    {
        return instance;
    }

    public List<ItemUnit> GetItemUnitList()
    {
        return itemUnits;
    }

    public int GetGold()
    {
        return currGold;
    }

    public void SetGold(int goldAmount)
    {
        currGold = goldAmount;
    }

    public void RemoveItem_byItem(ItemUnit item)
    {
        itemUnits.Remove(item);
    }

    public static void GetCharInvenSGT(int charIndex, ref string[] skillEquipped)
    {
        // Func to adapt between each work
    }
}

[Serializable]
public class ItemUnit
{
    private static int _index = 0;

    public int index = -1;
    public string itemName;
    public List<int> itemData = new();
    public List<int> invenAddr = new();
    public int GoldValue;

    public static void SetIndex_byItemList(int target)
    {
        _index = target;
    }

    internal void InitData_Random_Slot(SlotGUI_InvenSlot slot)
    {
        InitializeItemUnit(slot.myAddr);
        itemName = $"Rand_{index}";
    }

    internal void InitData_Random_Goods()
    {
        InitializeItemUnit();
        itemName = $"RandGoods_{index}";
    }

    internal void InitData_Random_Cooks(int input = -1)
    {
        InitializeItemUnit();
        itemName = $"RandGoods_{index}";
        InitializeCookData(input);
    }

    public void SetIndex_byPurchase()
    {
        InitializeIndex();
        itemName = $"Purchase - {index}";
    }

    private void InitializeItemUnit(List<int> address = null)
    {
        InitializeIndex();
        InitializeItemData();
        GoldValue = UnityEngine.Random.Range(50, 150);
        invenAddr = address;
    }

    private void InitializeIndex()
    {
        index = _index++;
    }

    private void InitializeItemData()
    {
        MyInvenSpriteDB itemDataTable = SGT_GUI_ItemData.GetInstance().spriteDataSet;

        int index_0 = itemDataTable.GetCount_byItemData(itemData);
        index_0 = UnityEngine.Random.Range(0, index_0);
        itemData.Add(index_0);

        if (index_0 == 0)
        {
            itemData.Add(0);
        }

        while (true)
        {
            int temp = itemDataTable.GetCount_byItemData(itemData);
            if (temp < 0) break;
            itemData.Add(UnityEngine.Random.Range(0, temp));
        }

        if (index_0 != 0)
        {
            itemData.RemoveAt(itemData.Count - 1);
            itemData.Add(0);
        }
    }

    private void InitializeCookData(int input)
    {
        if (SGT_GUI_ItemData.GetInstance() == null) return;

        MyInvenSpriteDB itemDataTable = SGT_GUI_ItemData.GetInstance().spriteDataSet;

        itemData.Add(0);
        itemData.Add(1);

        while (true)
        {
            int temp = itemDataTable.GetCount_byItemData(itemData);
            if (temp < 0) break;
            itemData.Add(UnityEngine.Random.Range(0, temp));
        }

        if (input != -1)
        {
            itemData.RemoveAt(itemData.Count - 1);
            itemData.Add(input);
        }
    }
}
