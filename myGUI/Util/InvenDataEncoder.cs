using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InvenDataEncoder
{
    internal static List<ItemUnit> GetData_toItemList()
    {
        return StringDataToItemList(GameManager.gameManager.invenData);
    }

    internal static int GetData_toGoldValue()
    {
        if (GameManager.gameManager == null)
        {
            return -1;
        }

        return StringDataToGoldValue(GameManager.gameManager.invenData);
    }

    private static string ItemListToString(List<ItemUnit> _list)
    {
        string result = "";

        for (int i = 0; i < _list.Count; i++)
        {
            result += _list[i].index + "/";
            result += parseList_toIntStr(_list[i].itemData) + "/"; ;
            result += parseList_toIntStr(_list[i].invenAddr) + "/"; ;
            result += _list[i].GoldValue + "/";
        }
        return result;

        string parseList_toIntStr(List<int> _input)
        {
            string rtn = "";
            for (int i = 0; i < _input.Count; i++)
            {
                rtn += _input[i];
            }
            return rtn;
        }
    }

    private static string ParseListToIntStr(List<int> list)
    {
        return string.Join("", list);
    }

    private static List<ItemUnit> StringDataToItemList(string data)
    {
        var itemList = new List<ItemUnit>();
        var slicedData = data.Split('/');

        if (slicedData.Length % 4 != 2)
        {
            return null;
        }

        for (int i = 1; i < slicedData.Length - 1; i += 4)
        {
            var item = new ItemUnit
            {
                itemName = "Inited Item",
                index = int.Parse(slicedData[i]),
                itemData = ParseIntStrToList(slicedData[i + 1]),
                invenAddr = ParseIntStrToList(slicedData[i + 2]),
                GoldValue = int.Parse(slicedData[i + 3])
            };
            itemList.Add(item);
        }

        return itemList;
    }

    private static List<int> ParseIntStrToList(string data)
    {
        var list = new List<int>();
        foreach (var ch in data)
        {
            list.Add(ch - '0'); // '0'을 빼서 숫자로 변환
        }
        return list;
    }

    private static int StringDataToGoldValue(string data)
    {
        var slicedData = data.Split('/');
        return slicedData.Length % 4 == 2 ? int.Parse(slicedData[0]) : -1;
    }

    internal static void SetDataByItemList()
    {
        var temp = SGT_GUI_ItemData.GetInstance();
        if (temp != null)
        {
            string data = ItemListToString(temp.GetItemUnitList());
            GameManager.gameManager.SetMapData_History(data);
        }
    }
}
