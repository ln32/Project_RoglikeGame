using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeSlot : MonoBehaviour
{
    public int index;
    public TMP_Text textName;
    public TMP_Text textLv;
    public GameObject button_Up;
    public string curId;

    public void OnUpBtnClicked()
    {
        GameManager.lobbyScenario.OnUpBtnClicked(this);
    }
    public void OnPointerEnter()
    {
        GameManager.lobbyScenario.OnPointerEnter_Slot(this);
    }
    public void OnPointerExit()
    {
        GameManager.lobbyScenario.OnPointerExit_Slot();
    }
}
