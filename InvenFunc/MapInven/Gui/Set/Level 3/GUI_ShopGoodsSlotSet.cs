using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUI_ShopGoodsSlotSet : MonoBehaviour
{
    public List<SlotGUI_ShopGoods> MySlotList = new List<SlotGUI_ShopGoods>();
    void Start()
    {
        FillUpGoods();
    }

    public void FillUpGoods()
    {
        SGT_GUI_ItemData invenData_SGT = SGT_GUI_ItemData.GetInstance();

        for (int i = 0; i < MySlotList.Count; i++)
        {
            ItemUnit itemData = new ItemUnit();
            itemData.InitData_Random_Goods();

            GUI_ItemUnit ins_ItemGUI = invenData_SGT.spriteDataSet.GetGUI_byItemData(itemData.itemData, MySlotList[i].transform);
            MySlotList[i].SetGUI_byItemGUI(ins_ItemGUI);
            MySlotList[i].SetItemData_byData(itemData);
        }
    }
}