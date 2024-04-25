using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapRDM_ItemInfoCtrl : MonoBehaviour
{
    [SerializeField] GameObject myVisualObj;
    [SerializeField] TextMeshProUGUI value_ItemName;
    [SerializeField] TextMeshProUGUI value_ItemType;
    [SerializeField] List<string> first;

    // Update is called once per frame
    public void SetItemInfo_byItemDataUnit(iInvenSlot targetSlot)
    {
        if (targetSlot == null)
            SetDefault();

        myVisualObj.SetActive(true);

        SlotGUI_InvenSlot parsedSlot = targetSlot as SlotGUI_InvenSlot; 
        if (parsedSlot == null) { SetDefault(); return; }

        GUI_ItemUnit _GUI = parsedSlot._itemGUI; 
        if (_GUI == null) { SetDefault(); return; }

        ItemUnit _data = parsedSlot._itemGUI._myData;
        value_ItemName.text = _data.itemName;

        string temp = "";
        for (int i = 0; i < _data.itemData.Count ; i++)
        {
            temp += _data.itemData[i] + " / ";
        }
        value_ItemType.text = temp;
    }    
    
    // Update is called once per frame
    public void SetDefault()
    {
        myVisualObj.SetActive(false);
    }

    void OnEnable()
    {
        SetDefault();
    }
}
