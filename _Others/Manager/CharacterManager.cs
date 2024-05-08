using EnumCollection;
using BattleCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private List<CharacterData> characterDataList;//DocId는 내부에 존재
    public static CharacterManager characterManager;
    public int testNum = 1;
    private void Awake()
    {
        testNum = 1;
        characterDataList = new();
        if (!characterManager)
        {
            characterManager = this;
        }
    }
    public List<CharacterData> GetCharacters() => characterDataList;
    public CharacterData GetCharacter(int _index) => characterDataList[_index];

    public void SetCharacters(List<CharacterData> _characterDataDict) => characterDataList = _characterDataDict;

    [ContextMenu("ExplainTest")]
    public void ExplainTest()
    {
        System.Tuple<string, float> skill0 = characterDataList[testNum].GetSkillExplainAndCoolTime(0);
        if (skill0 != null)
        {
            Debug.Log("Explain0 : " + skill0.Item1);
            Debug.Log("Cooltime0 : " + skill0.Item2);
        }

        System.Tuple<string, float> skill1 = characterDataList[testNum].GetSkillExplainAndCoolTime(1);
        if (skill1 != null)
        {
            Debug.Log("Explain1 : " + skill1.Item1);
            Debug.Log("Cooltime1 : " + skill1.Item2);
        }
    }
}
