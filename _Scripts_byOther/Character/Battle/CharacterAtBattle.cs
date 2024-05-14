using EnumCollection;
using BattleCollection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LobbyCollection;
using CharacterCollection;

public class CharacterAtBattle : BaseAtBattle
{
    public static readonly Color TARGET_COLOR = new(0f, 0f, 1f, 0.5f);
    public bool isAct = false;
    public new string name;
    public List<TalentStruct> talents;

    public void InitCharacter(string _documentId, GridObject _grid)
    {
        InitBase(_grid);
        IsEnemy = false;

        transform.localScale = Vector3.one;
        transform.GetChild(0).localScale = Vector3.one * 100;
        transform.localPosition = Vector3.zero;
        skillTargetTransform = Instantiate(new GameObject("SkillTarget"), transform.GetChild(0)).transform;
        skillTargetTransform.localPosition = new Vector3(-0.4f, 0.6f, 0);
        skillTargetTransform.localScale = new Vector3(0.6f, 0.6f, 0);
        rootTargetTransform.localScale = new Vector3(0.6f, 0.6f, 0);
        rootTargetTransform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        skillTargetTransform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        documentId = _documentId;
    }
    [ContextMenu("DeadTest")]
    public override void OnDead()
    {
        GameManager.battleScenario.StartCoroutine(OnDead_Base());
        bool gameOverFlag = false;
        foreach (var x in BattleScenario.characters)
            if (!x.isDead)
                gameOverFlag = true;
        if (!gameOverFlag)
        {
            GameManager.gameManager.GameOver();//게임 오버
        }

    }

    public override void SetAnimParam()
    {

    }
}