using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepartUi : LobbyUiBase
{
    public void DepartSelected()
    {
        GameManager.lobbyScenario.DepartAsync();
    }
}
