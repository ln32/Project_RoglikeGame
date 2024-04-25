using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class _InvenDataEncoder
{
    internal static List<ItemUnit> GetData_toItemList()
    {
        if (CJH_GameManager._instance == null)
        {
            Debug.Log("???");
            return null;
        }

        return StringData_ToItemList(CJH_GameManager._instance.invenData);
    }

    internal static int GetData_toGoldValue()
    {
        if (CJH_GameManager._instance == null)
        {
            Debug.Log("???");
            return -1;
        }

        return StringData_ToGoldValue(CJH_GameManager._instance.invenData);
    }

    static string ItemListData_ToString(List<ItemUnit> _list)
    {
        string _str = "";
        for (int i = 0; i < _list.Count; i++)
        {
            _str += _list[i].index + "/";
            _str += parseList_toIntStr(_list[i].itemData) + "/"; ;
            _str += parseList_toIntStr(_list[i].invenAddr) + "/"; ;
            _str += _list[i].GoldValue + "/";
        }

        return _str;



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


    static List<ItemUnit> StringData_ToItemList(string _str)
    {
        List<ItemUnit> rtnList = new();
        string[] sliced = _str.Split('/');
        if (sliced.Length % 4 != 2)
        {
            Debug.Log("? " + sliced.Length % 4 + " ?"); return null;
        }

        for (int i = 1; i < sliced.Length-1; i+=4)
        {
            ItemUnit tempItem = new ItemUnit();
            tempItem.itemName = "sad";
            tempItem.index = int.Parse(sliced[i]);
            tempItem.itemData = parseIntStr_toList(sliced[i+1]);
            tempItem.invenAddr = parseIntStr_toList(sliced[i+2]);
            tempItem.GoldValue = int.Parse(sliced[i+3]);
            rtnList.Add(tempItem);
        }

        return rtnList;

        List<int> parseIntStr_toList(string _input)
        {
            List<int> rtn = new List<int>();
            for (int i = 0; i < _input.Length; i++)
            {
                rtn.Add(_input[i] - 48);
            }
            return rtn;
        }
    }

    static int StringData_ToGoldValue(string _str)
    {
        string[] sliced = _str.Split('/');
        if (sliced.Length % 4 != 2)
        {
            Debug.Log("? " + sliced.Length % 4 + " ?"); return -1;
        }

        return int.Parse(sliced[0]);
    }


    internal static void SetData_byItemList()
    {
        var temp = SGT_GUI_ItemData.GetSGT();
        if (temp == null)
        {
            Debug.Log("SGT_GUI_ItemData.GetSGT() == null");
            return;
        }
        else
        {
            string cash = ItemListData_ToString(temp.itemUnits);
            GameManager.gameManager.SetMapData_History(cash);
            return;
        } 
    }
}