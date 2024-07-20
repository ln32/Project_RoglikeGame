using System;
public class RDM_CampSC : RDM_Default
{
    internal InvenSC_Default invenSC;
    internal GUI_IngridiSlotManager _GUI_IngridiSlotManager;
    internal GUI_InvenSetManager _GUI_InvenSetManager;
    protected GUI_ResultCookSet _GUI_ResultCookSet;


    private void Start()
    {
        _REF._SGT_GUI_ItemData = invenSC.invenData_SGT;
    }

    public void BtnEvent_Cook()
    {
        this.GetInventoryItems_byItemIndex();
        _GUI_IngridiSlotManager.RefreshMyGUI();
    }
}