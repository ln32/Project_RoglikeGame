using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _SlotItemGuiEvent : MonoBehaviour
{
    public GameObject _OnSet;
    public GameObject _OnDepose;

    public void SetGui_byIsNull(bool isNull)
    {
        if (isNull)
        {
            SetGui_byIsNull_OnDepose();
        }
        else
        {
            SetGui_byIsNull_OnSet();
        }
    }

    void SetGui_byIsNull_OnSet()
    {
        _OnSet.gameObject.SetActive(true);
        _OnDepose.gameObject.SetActive(false);
    }

    void SetGui_byIsNull_OnDepose()
    {
        _OnSet.gameObject.SetActive(false);
        _OnDepose.gameObject.SetActive(true);
    }
}
