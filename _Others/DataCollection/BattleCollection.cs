using EnumCollection;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCollection
{

    public class SkillForm
    {
        public Dictionary<Language, string> name;
        public List<Dictionary<Language, string>> explain;
        public SkillCategori categori;
        public float cooltime = 8f;
        public bool isTargetEnemy = false;
        public List<List<SkillEffect>> effects;
        public bool isAnim;
        public List<string> visualEffect;
        public bool isPre = false;
        public SkillForm SetName(Dictionary<Language, string> _name)
        {
            name = _name;
            return this;
        }
        public SkillForm SetExplain(List<Dictionary<Language, string>> _explain)
        {
            explain = _explain;
            return this;
        }
        public SkillForm SetCategori(SkillCategori _categori)
        {
            categori = _categori;
            return this;
        }
        public SkillForm SetCooltime(float _cooltime)
        {
            cooltime = _cooltime;
            return this;
        }
        public SkillForm SetIsTargetEnemy(bool _isTargetEnemy)
        {
            isTargetEnemy = _isTargetEnemy;
            return this;
        }
        public SkillForm SetEffects(List<List<SkillEffect>> _effects)
        {
            effects = _effects;
            return this;
        }
        public SkillForm SetIsAnim(bool _isAnim)
        {
            isAnim = _isAnim;
            return this;
        }
        public SkillForm SetSkillEffect(List<string> _visualEffect)
        {
            visualEffect = _visualEffect;
            return this;
        }
        public SkillForm SetIsPre(bool _isPre)
        {
            isPre = _isPre;
            return this;
        }
    }
    [Serializable]
    public class Skill
    {
        public Dictionary<Language, string> name;
        public Dictionary<Language, string> explain;
        public SkillCategori categori;
        public float cooltime;
        public List<SkillEffect> effects;
        public bool isTargetEnemy;
        public bool isAnim;
        public VisualEffect visualEffect;
        public static float defaultAttackCooltime = 3f;
        public bool isPre;
        public Skill(SkillForm _skillForm, byte _level)//Skill
        {
            name = _skillForm.name;
            if (_skillForm.explain != null)
                explain = _skillForm.explain[_level];
            categori = _skillForm.categori;
            cooltime = _skillForm.cooltime;
            isTargetEnemy = _skillForm.isTargetEnemy;
            effects = new();
            effects = _skillForm.effects[_level];
            isAnim = _skillForm.isAnim;
            if (_skillForm.visualEffect != null)
            {
                string veName = _skillForm.visualEffect[_level];
                if (veName != string.Empty)
                    visualEffect = LoadManager.loadManager.skillVisualEffectDict[veName];
            }
            isPre = _skillForm.isPre;
        }
        public Skill()//Default Attack
        {
            name = new() { { Language.En, "Default Attack" }, { Language.Ko, "기본 공격" } };
            explain = new() { { Language.En, "Default Attack" }, { Language.Ko, "기본 공격" } };
            cooltime = defaultAttackCooltime;
            isTargetEnemy = true;
            isAnim = true;
            isPre = false;
            //skillEffect = LoadManager.loadManager.skillEffectDict["arrowFire_01"];
            effects = new()
            {
                new SkillEffect().
                SetType(EffectType.Damage).
                SetCount(1).
                SetIsConst(false).
                SetValue(1f).
                SetDelay(0.5f).
                SetRange(EffectRange.Dot).
                SetIsPassive(false).
                SetVamp(0f).
                SetByAtt(false)
            };
        }
        public Skill(List<SkillEffect> _effects)
        {
            effects = new();
            foreach (var jobEffect in _effects)
            {
                SkillEffect skillEffet = new();
                skillEffet.SetIsConst(true).SetRange(EffectRange.Self).SetIsPassive(true).SetType(jobEffect.type).SetValue(jobEffect.value).SetByAtt(jobEffect.byAtt);
                effects.Add(skillEffet);
            }

        }
    }
    [Serializable]
    public class SkillEffect
    {
        public float value = 1f;
        public int count = 1;
        public EffectType type;
        public bool isConst = false;
        public float delay = 0f;
        public EffectRange range = EffectRange.Dot;
        public bool isPassive = false;
        public float vamp = 0f;
        public bool byAtt = false;
        public SkillEffect SetValue(float _value)
        {
            value = _value;
            return this;
        }
        public SkillEffect SetCount(int _count)
        {
            count = _count;
            return this;
        }
        public SkillEffect SetType(EffectType _type)
        {
            type = _type;
            return this;
        }
        public SkillEffect SetIsConst(bool _isConst)
        {
            isConst = _isConst;
            return this;
        }
        public SkillEffect SetDelay(float _delay)
        {
            delay = _delay;
            return this;
        }
        public SkillEffect SetRange(EffectRange _range)
        {
            range = _range;
            return this;
        }
        public SkillEffect SetIsPassive(bool _isPassive)
        {
            isPassive = _isPassive;
            return this;
        }
        public SkillEffect SetVamp(float _vamp)
        {
            vamp = _vamp;
            return this;
        }
        public SkillEffect SetByAtt(bool _byAtt)
        {
            byAtt = _byAtt;
            return this;
        }
    }
    [Serializable]
    public class JobClass
    {
        public Dictionary<Language, string> name = null;

        public List<SkillEffect> effects;
        public JobClass SetName(Dictionary<Language, string> _name)
        {
            name = _name;
            return this;
        }
        public JobClass SetEffects(List<SkillEffect> _effects)
        {
            effects = _effects;
            return this;
        }
    }
    public class EnemyClass
    {
        public Dictionary<Language, string> name;
        public float ability;
        public float hp;
        public float resist;
        public List<Skill> skills;
        public float speed;
        public bool isMonster;
        public string type;
        public int enemyLevel;
        public EnemyClass SetName(Dictionary<Language, string> _name)
        {
            name = _name;
            return this;
        }
        public EnemyClass SetAbility(float _ability)
        {
            ability = _ability;
            return this;
        }
        public EnemyClass SetHp(float _hp)
        {
            hp = _hp;
            return this;
        }
        public EnemyClass SetResist(float _resist)
        {
            resist = _resist;
            return this;
        }
        public EnemyClass SetSkills(List<Skill> _skills)
        {
            skills = _skills;
            return this;
        }
        public EnemyClass SetSpeed(float _speed)
        {
            speed = _speed;
            return this;
        }
        public EnemyClass SetIsMonster(bool _isMonster)
        {
            isMonster = _isMonster;
            return this;
        }
        public EnemyClass SetType(string _type)
        {
            type = _type;
            return this;
        }
        public EnemyClass SetEnemyLevel(int _enemyLevel)
        {
            enemyLevel = _enemyLevel;
            return this;
        }
    }
    public class EnemyCase
    {
        public List<EnemyPieceForm> pieces;//id, index
        public List<int> levelRange;
        public EnemyCase SetEnemies(List<EnemyPieceForm> _pieces)
        {
            pieces = _pieces;
            return this;
        }
        public EnemyCase SetLevelRange(List<int> _levelRange)
        {
            levelRange = _levelRange;
            return this;
        }
    }
    public class EnemyPieceForm
    {
        //셋 중 하나만 있어야 함
        public string id;
        public string type;
        public int enemyLevel = -1;
        //반드시 필요함
        public int index;
        public EnemyPieceForm SetId(string _id)
        {
            id = _id;
            return this;
        }
        public  EnemyPieceForm SetLevel(int _enemyLevel)
        {
            enemyLevel = _enemyLevel;
            return this;
        }
        public EnemyPieceForm SetType(string _type)
        {
            type = _type;
            return this;
        }
        public EnemyPieceForm SetIndex(int _index)
        {
            index = _index;
            return this;
        }
    }
    public class EnemyPiece
    {
        public string id;
        public int index;
        public EnemyPiece(string _id, int _index)
        {
            id = _id;
            index = _index;
        }
    }

    public class WeaponClass
    {
        public string id;
        public WeaponType type;
        public WeaponGrade grade;
        public List<SkillEffect> effects;
        public float ability, hp, resist, speed;
        public Sprite sprite;
        public VisualEffect defaultVisualEffect;
        public VisualEffect skillVisualEffect;
        public WeaponClass SetId(string _id)
        {
            id = _id;
            return this;
        }
        public WeaponClass SetType(WeaponType _type)
        {
            type = _type;
            return this;
        }
        public WeaponClass SetGrade(WeaponGrade _grade)
        {
            grade = _grade;
            return this;
        }
        public WeaponClass SetStatus(float _ability, float _hp, float _resist, float _speed)
        {
            ability = _ability;
            hp = _hp;
            resist = _resist;
            speed = _speed;
            return this;
        }
        public WeaponClass SetEffects(List<SkillEffect> _effects )
        {
            effects = _effects;
            return this;
        }
        public WeaponClass SetSprite(Sprite _sprite)
        {
            sprite = _sprite;
            return this;
        }
        public WeaponClass SetVisualEffect(VisualEffect _defaultVisualEffect, VisualEffect _skillVisualEffect)
        {
            defaultVisualEffect = _defaultVisualEffect;
            skillVisualEffect = _skillVisualEffect;
            return this;
        }
    }
    public class VisualEffect
    {
        public GameObject effectObject;
        public float duration;
        public string sound;
        public bool fromRoot;
        public VisualEffect(GameObject _effectObject, float _duration, string _sound, bool _fromRoot)
        {
            effectObject = _effectObject;
            duration = _duration;
            sound = _sound;
            fromRoot = _fromRoot;
        }
    }
}