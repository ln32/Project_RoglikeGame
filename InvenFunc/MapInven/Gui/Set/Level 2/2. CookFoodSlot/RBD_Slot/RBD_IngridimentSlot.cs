using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBD_IngridimentSlot : MonoBehaviour, IResponedByDrop
{
    [SerializeField] internal GUI_Ctrl myGUI_CTRL;
    [SerializeField] internal GUI_ItemUnit _GUI_ItemUnit;
    public GUI_IngridiSlot myGUI_Slot;
    [SerializeField] internal int _index;

    private iRoot_DDO_Manager cash = null;

    public iRoot_DDO_Manager GetDDO_Manager()
    {
        if (cash != null)
            return cash;
        cash = transform.root.GetComponent<iRoot_DDO_Manager>();

        return transform.root.GetComponent<iRoot_DDO_Manager>();
    }

    public iSlotGUI GetTargetSlotGUI()
    {
        return myGUI_CTRL;
    }

    public void DDO_Event_byInvenSlot(SlotGUI_InvenSlot _src)
    {
        if (false)
        {
            RDM_CampSC _RDM_CookSC = GetDDO_Manager() as RDM_CampSC;
            if (_RDM_CookSC)
            {
                _RDM_CookSC.SetIngridiment_byInvenSlot(_src, this);
                return;
            }
        }

        if (true)
        {
            RDM_CampCook _RDM_CampCook = GetDDO_Manager() as RDM_CampCook;
            if (_RDM_CampCook)
            {
                Debug.Log("TODO : CheckLevel");
                _RDM_CampCook.SetIngridiment_byInvenSlot(_src, this);
                return;
            }
        }
    }

    public void SetDefault()
    {
        myGUI_Slot.SetDefault();
        if (_GUI_ItemUnit != null)
        {
            Destroy(_GUI_ItemUnit.gameObject);
            _GUI_ItemUnit = null;
        }
    }
}
