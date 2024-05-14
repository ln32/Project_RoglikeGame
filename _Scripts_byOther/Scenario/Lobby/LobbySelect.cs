using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySelect : MonoBehaviour
{
    public LobbyCase lobbyCase;
    private GameObject selectLight;
    private void Start()
    {
        selectLight = GameManager.lobbyScenario.selectLight;
    }
    public void OnPointerEnter()
    {
        selectLight.SetActive(true);
        selectLight.transform.SetParent(transform);
        selectLight.transform.localPosition = Vector3.zero;
    }
    public void OnPointerExit()
    {
        selectLight.SetActive(false);
    }
    public void OnPointerClick()
    {
        GameManager.lobbyScenario.OnPointerClick(lobbyCase);   
    }
}
