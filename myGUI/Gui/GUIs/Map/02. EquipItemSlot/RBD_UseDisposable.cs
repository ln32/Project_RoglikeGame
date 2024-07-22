using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBD_UseDisposable : MonoBehaviour, IResponedByDrop
{
    [SerializeField]internal int myIndex;

    private iRoot_DDO_Manager cash = null;
    public iRoot_DDO_Manager GetDDO_Manager()
    {
        if (cash != null)
            return cash;

        return transform.root.GetComponent<iRoot_DDO_Manager>();
    }


    [SerializeField] private GUI_Ctrl myGUI_CTRL;
    public iSlotGUI GetTargetSlotGUI()
    {
        return myGUI_CTRL;
    }
}
