using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EnumCollection;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class BattleScenarioTest : MonoBehaviour
{
    public Transform canvasTest;
    private TMP_Text botText;
    private TMP_Text moveText;
    public int moveTargetIndex;
    private BattleScenario battleScenario;
    private void Start()
    {
        battleScenario = GameManager.battleScenario;
        for (int i = 0; i < 9; i++)
        {
            int index = i;
            var friendlyGrid = BattleScenario.CharacterGrids[i];
            friendlyGrid.GetComponent<Button>().onClick.AddListener(() => OnCharacterGridClicked(BattleScenario.CharacterGrids[index], false));
            var enemyGrid = BattleScenario.EnemyGrids[i];
            enemyGrid.GetComponent<Button>().onClick.AddListener(() => OnCharacterGridClicked(BattleScenario.EnemyGrids[index], true));
        }

    }
    public enum TestPattern
    {
        Default, Bot, Move
    }
    public TestPattern testPattern = TestPattern.Default;
    public void BotTest()
    {
        SwitchTest(TestPattern.Bot);
    }

    private void SwitchTest(TestPattern _testPatern)
    {
        moveTargetIndex = -1;
        RefreshTest_Text();
        if (testPattern == _testPatern)
        {
            testPattern = TestPattern.Default;
        }
        else
        {
            testPattern = _testPatern;
            switch (_testPatern)
            {
                case TestPattern.Bot:
                    botText.text = "봇 생성\n취소";
                    break;
                case TestPattern.Move:
                    moveText.text = "캐릭터\n이동 취소";
                    break;
            }
            
        }
    }
    private void OnCharacterGridClicked(GridObject _grid, bool _isEnemyGrid)
    {
        if (testPattern != TestPattern.Default)
        {
            switch (testPattern)
            {
                case TestPattern.Bot:
                    RefreshTest();
                    break;
                case TestPattern.Move:
                    if (moveTargetIndex > -1)
                    {
                        //battleScenario.MoveCharacter(moveTargetIndex, _gridIndex, _isEnemyGrid, false);
                    }
                    else
                    {
                    }
                    break;
            }
            return;
        }
    }
    public void MoveTest()
    {
        SwitchTest(TestPattern.Move);
    }
    public void RefreshTest()
    {
        testPattern = TestPattern.Default;
        moveTargetIndex = -1;
        RefreshTest_Text();
    }

    private void RefreshTest_Text()
    {
        botText.text = "봇 생성";
        moveText.text = "캐릭터\n이동";
    }
    public async void StageClearTest()
    {
        await battleScenario.StageClearAsync();
    }
    public void GameOverTest()
    {
        GameManager.gameManager.GameOver();
    }
    public void BattleSimulationAsync()
    {
        battleScenario.GoBattleSimulation();
    }
}
