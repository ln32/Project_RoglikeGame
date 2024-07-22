using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapRDM_ItemInfoDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject myVisualObj;
    [SerializeField] private TextMeshProUGUI value_ItemName;
    [SerializeField] private TextMeshProUGUI value_ItemType;

    public void SetItemInfo_byItemDataUnit(iInvenSlot targetSlot)
    {
        if (targetSlot == null)
        {
            SetDefault();
            return;
        }

        myVisualObj.SetActive(true);

        if (targetSlot is SlotGUI_InvenSlot parsedSlot && parsedSlot._itemGUI != null)
        {
            GUI_ItemUnit gui = parsedSlot._itemGUI;
            ItemUnit data = gui._myData;
            value_ItemName.text = data.itemName;
            value_ItemType.text = string.Join(" / ", data.itemData);
        }
        else
        {
            SetDefault();
        }
    }

    public void SetDefault()
    {
        myVisualObj.SetActive(false);
        value_ItemName.text = string.Empty;
        value_ItemType.text = string.Empty;
    }

    private void OnEnable()
    {
        SetDefault();
    }
}
