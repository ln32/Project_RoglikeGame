using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GUI_InvenSpaceSlotSet : MonoBehaviour
{
    public List<int> myAddr = new();
    public List<SlotGUI_InvenSlot> MySlotList = new List<SlotGUI_InvenSlot>();
    public int index = -1;

    public void SetState_onInit(List<int> _data)
    {
        myAddr = new();
        for (int i = 0; i < _data.Count; i++)
        {
            myAddr.Add(_data[i]);
        }
        myAddr.Add(index);

        MySlotList = GetComponentsInChildren<SlotGUI_InvenSlot>().ToList();
        for (int i = 0; i < MySlotList.Count; i++)
        {
            MySlotList[i].SetState_onInit(i,myAddr);
        }
    }

    public void FixGridSize()
    {
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        RectTransform rect = GetComponent<RectTransform>();

        float totalWidth = (grid.cellSize.x * 5 + grid.spacing.x * 4);
        float ratio_Width = rect.rect.width / totalWidth;


        float totalHeight = (grid.cellSize.y * 3 + grid.spacing.y * 2);
        float ratio_Height= rect.rect.height / totalHeight;
        
        float minRatio = Mathf.Min(ratio_Width, ratio_Height);

        grid.cellSize *= minRatio;
        grid.spacing *= minRatio;
    }
}
