using EnumCollection;
using BattleCollection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EnemyAtBattle : BaseAtBattle
{
    public static readonly Color TARGET_COLOR = new(1f, 0f, 0f, 0.5f);
    public static readonly float DEFAULT_PROB = 0.6f;
    public static readonly float INCREASE_PROB = 0.1f;

    public void InitEnemy(EnemyClass _enemyClass, GridObject _grid, bool _isMonster)
    {
        InitBase(_grid);
        IsEnemy = true;
        isMonster = _isMonster;
        
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        skillTargetTransform = transform.GetChild(0).GetChild(0);
        rootTargetTransform.localScale = skillTargetTransform.localScale = new Vector3(1f, 1f, 0f);
        skillTargetTransform.localScale = skillTargetTransform.localScale = new Vector3(1f, 1f, 0f);
        maxHp = Hp = maxHpInBattle =  _enemyClass.hp;
        ability = abilityInBattle = _enemyClass.ability;
        resist = resistInBattle = _enemyClass.resist;
        speed = speedInBattle = _enemyClass.speed;
        skills = _enemyClass.skills;
        grid = _grid;
        grid.owner = this;
    }
    public override void OnDead()
    {
        StartCoroutine(OnDead_Base());
        bool gameOverFlag = false;
        foreach (BaseAtBattle enemy in BattleScenario.enemies)
        {
            if (!enemy.isDead)
                gameOverFlag = true;
        }
        if (!gameOverFlag)
            GameManager.battleScenario.StageClearAsync();
    }

    public override void SetAnimParam()
    {
    }
}