using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUiBase : MonoBehaviour
{
    public void ExitBtnClicked()
    {
        gameObject.SetActive(false);
        GameManager.lobbyScenario.SetMediumImage(true);
    }
}
