    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] internal List<GameCharactor> _data;//DocId는 내부에 존재 _data;
    private static CharacterData _ins;
    public List<CharEquipZone> viewData;

    static public CharacterData GetSGT()
    {
        return _ins;
    }

    public void InitData()
    {
        if (_ins == null)
        {
            _ins = this;

            for (int i = 0; i < _data.Count; i++)
            {
                _data[i].equipData[0] = UnityEngine.Random.Range(0, 5);
            }
        }
    }

    public GameCharactor GetCharData(int _value)
    {
        return _ins._data[_value];
    }

    public void Event_SpendItem(int _value)
    {
        if (_ins == null)
            return;
        if (_ins.viewData[_value] == null)
            return;

        _ins.viewData[_value].RefreshText();
    }
}


[Serializable]
public class Charactor
{
    public string charName;
    public float maxHp;
    public float hp;
    public float ability;
    public float resist;
    public float speed;
    public int[] equipData= new int[10];

    public void setCharactor(string _charName, int _hp, int _attackPower)
    {
        charName = _charName;
        hp = _hp;
        ability = _attackPower;
    }

    public bool IsAlive()
    {
        return hp > 0;
    }
}


[Serializable]
public class GameCharactor : Charactor
{
    public string jobId;
    public int index;

    public void setGameCharactor(string _charName, string _jobId, int _hp, int _attackPower, int _index)
    {
        setCharactor(_charName, _hp, _attackPower);
        jobId = _jobId;
        index = _index;
    }

    public void InteractFood()
    {        
        // TODO : Detail interact with food
        maxHp += 2;
        hp += 1;
    }

    public void SetRandomFace()
    {
        // TODO : Reroll OutFace
    }
}