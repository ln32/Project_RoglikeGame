using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationButton : MonoBehaviour
{
    public int simulateLevel;
    public void GoToBattle()
    {
        BattleScenario.battleSimulator.currentLevel = simulateLevel;
        SceneManager.LoadScene("Battle");
    }
}
