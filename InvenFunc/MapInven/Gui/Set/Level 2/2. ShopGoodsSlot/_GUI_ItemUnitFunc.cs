using UnityEngine;

internal static class _GUI_ItemUnitFunc
{
    internal static void SetGUI_OnFocus(this RBD_CasherZone _ShopSC, GUI_ItemUnit _GUI_ItemUnit)
    {
        _GUI_ItemUnit.SetImageGUI_toMaterial(_GUI_ItemUnit._GUI.color_Focused);
    }

    internal static void SetGUI_Default(this RBD_CasherZone _ShopSC, GUI_ItemUnit _GUI_ItemUnit)
    {
        _GUI_ItemUnit.SetImageGUI_toMaterial(_GUI_ItemUnit._GUI.color_Default);
    }

    internal static void SetGUI_OnDrag(this RDM_ShopSC _ShopSC, IDragDropObj _IDragDropObj)
    {
        GUI_ItemUnit _GUI_ItemUnit = _IDragDropObj.GetTransform_ItemGUI().GetComponent<GUI_ItemUnit>();
        if (_GUI_ItemUnit._GUI.color_Cash != null)
        {
            return;
        }

        _GUI_ItemUnit._GUI.color_Cash = _GUI_ItemUnit.GetImageGUI_Material();
        _GUI_ItemUnit.SetImageGUI_toMaterial(_GUI_ItemUnit._GUI.color_Default);
    }

    internal static void SetGUI_EndDrag(this RDM_ShopSC _ShopSC, IDragDropObj _IDragDropObj)
    {
        GUI_ItemUnit _GUI_ItemUnit = _IDragDropObj.GetTransform_ItemGUI().GetComponent<GUI_ItemUnit>();
        if (_GUI_ItemUnit._GUI.color_Cash == null)
        {
            return;
        }

        _GUI_ItemUnit.SetImageGUI_toMaterial(_GUI_ItemUnit._GUI.color_Cash);
        _GUI_ItemUnit._GUI.color_Cash = null;
    }
}
