using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyInvenSpriteDB : MonoBehaviour
{
    public InvenSpriteDB_Node DB_ItemSpriteRoot;
    [SerializeField] internal Values_DiceColor _Value_Dice;
    public GUI_ItemUnit invenPrefab;


    public int GetCount_byItemData(List<int> itemData)
    {
        InvenSpriteDB_Node temp = getBranch_byItemData(itemData);

        if (temp == null)
            return -1;

        if(temp.isLeaf())
            return temp.myData.Count;
        else
            return temp.myChild.Count;

        InvenSpriteDB_Node getBranch_byItemData(List<int> itemData)
        {
            InvenSpriteDB_Node temp = DB_ItemSpriteRoot;

            for (int i = 0; i < itemData.Count; i++)
            {
                if (temp == null)
                    return null;

                int targetIndex = itemData[i];
                if (temp.myChild.Count > targetIndex)
                {
                    temp = temp.myChild[targetIndex];
                }
                else
                    temp = null;
            }
            return temp;
        }

    }

    public Sprite GetSprite_byItemData(List<int> itemData)
    {
        int lastIndex = itemData[itemData.Count - 1];
        InvenSpriteDB_Node temp = getBranch_byItemData(itemData);

        if (temp != null && temp.isLeaf())
        {
            return temp.myData[lastIndex];
        }else
            return null;

        InvenSpriteDB_Node getBranch_byItemData(List<int> itemData)
        {
            InvenSpriteDB_Node temp = DB_ItemSpriteRoot;

            for (int i = 0; i < itemData.Count-1; i++)
            {
                if (temp == null)
                    return null;

                int targetIndex = itemData[i];
                if (temp.myChild.Count > targetIndex)
                {
                    temp = temp.myChild[targetIndex];
                }
                else
                    temp = null;
            }

            return temp;
        }

    }
}
[Serializable]
internal class Values_DiceColor
{
    public InvenSpriteDB_Node DB_DiceSpriteRoot;
    public Color[] color_Case;
}
