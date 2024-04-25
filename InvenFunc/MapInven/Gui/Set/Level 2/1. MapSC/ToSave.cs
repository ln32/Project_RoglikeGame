using UnityEngine;

public class ToSave : MonoBehaviour
{
    [SerializeField] internal GUI_Ctrl myGUI_CTRL;

    private iRoot_DDO_Manager cash = null;
    public iRoot_DDO_Manager GetDDO_Manager()
    {
        if (cash != null)
            return cash;

        return transform.root.GetComponent<iRoot_DDO_Manager>();
    }

    public iSlotGUI GetTargetSlotGUI()
    {
        return myGUI_CTRL;
    }

    public void DDO_Event_byInvenSlot(SlotGUI_InvenSlot _src)
    {
        /*
        RDM_CookSC _RDM_CookSC = GetDDO_Manager() as RDM_CookSC;
        if (_RDM_CookSC)
        {
            _RDM_CookSC.SetIngridiment_byInvenSlot(_src, this);
        }
        */
    }
}
