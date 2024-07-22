using UnityEngine;

public class RBD_SellZone : MonoBehaviour, IResponedByDrop
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
        RDM_ShopSC _RDM_ShopSC = GetDDO_Manager() as RDM_ShopSC;
        if (_RDM_ShopSC)
        {
            _RDM_ShopSC.Sell_Item_byBtnClick(_src);
        }
    }
}