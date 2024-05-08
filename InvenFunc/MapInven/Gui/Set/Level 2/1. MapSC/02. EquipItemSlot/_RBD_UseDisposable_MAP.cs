using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class _RBD_UseDisposable_MAP
{
    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, RBD_UseDisposable _dst)
    {
        if ((_inven as RDM_MapSC == true))
        {
            if (_src._itemGUI._myData.itemData[0] == 0)
            {
                var _CharacterData = CharacterData.getSGT();
                var tempChar = _CharacterData.getCharData(_dst.myIndex);
                float _hp = tempChar.hp; float _maxHp = tempChar.maxHp;
                if (true)
                {
                    Debug.Log(_dst.myIndex + " is use " + _src._itemGUI.GetNameText());
                    tempChar.resist+= (_src._itemGUI._myData.itemData[1] * 2 + 1);
                }
                int targetHp = (int)tempChar.hp+ (_src._itemGUI._myData.itemData[1] * 2 + 1);
                tempChar.hp = Mathf.Min(tempChar.maxHp,targetHp);

                _CharacterData.SetEvent(_dst.myIndex);
                _src.SetItemData_byData(null);
                return;
            }
        }
    }

}
