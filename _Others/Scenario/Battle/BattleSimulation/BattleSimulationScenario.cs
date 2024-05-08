using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSimulationScenario : MonoBehaviour
{
    public List<SimulationButton> simulationButtons;
    private void Awake()
    {
        InitSimulationButtons();
        if (!BattleScenario.battleSimulator)
        {
            GameObject simulatorObj =  new GameObject("BattleSimulator");
            BattleSimulator battleSimulator = simulatorObj.AddComponent<BattleSimulator>();
            BattleScenario.battleSimulator = battleSimulator;
            DontDestroyOnLoad(simulatorObj);
        }
    }
    private void InitSimulationButtons()
    {
        for (int i = 0; i < simulationButtons.Count; i++)
        {
            SimulationButton button = simulationButtons[i];
            button.simulateLevel = i;
        }
    }
}
