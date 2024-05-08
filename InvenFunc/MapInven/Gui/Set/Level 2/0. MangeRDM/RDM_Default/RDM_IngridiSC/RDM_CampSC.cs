using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class RDM_CampSC : RDM_Default
{
    public GUI_IngridiSlotManager _GUI_IngridiSlotManager;
    public GUI_InvenSetManager _GUI_InvenSetManager;
    public GUI_ResultCookSet _GUI_ResultCookSet;
    public InvenSC_Default invenSC;

    private void Start()
    {
        _REF._SGT_GUI_ItemData = invenSC.invenData_SGT;
    }

    public void BtnEvent_Cook()
    {
        this.GetInvenItem_byItemIndex();
        _GUI_IngridiSlotManager.RefreshMyGUI();
    }
}