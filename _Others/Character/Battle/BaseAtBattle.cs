using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using BattleCollection;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Linq;

abstract public class BaseAtBattle : MonoBehaviour
{
    public JobClass job;
    public string documentId;
    public float maxHpInBattle;
    public float maxHp;
    [SerializeField] private float hp;
    public float Hp {
        get { return hp; }
        set
        {
            hp = Mathf.Clamp(value, 0, maxHpInBattle);
            float hpUpper = (hp + armor > maxHpInBattle) ? hp + armor : maxHpInBattle;
            hpBar.fillAmount = hp / hpUpper;
            armorBar.fillAmount = (hp + armor) / hpUpper;
            if (hp <= 0 && !isDead && BattleScenario.battlePatern == BattlePatern.Battle)
                OnDead();
        }
    }
    public float abilityInBattle;
    public float ability;
    public float resistInBattle;
    public float resist;
    public float speedInBattle;
    public float speed;
    public float armor = 0f;
    public bool isDead;
    public List<Skill> skills;
    public WeaponClass weapon;
    private Skill defaultAttack;
    public GameObject hpObject;
    public Image hpBar;
    public Image armorBar;
    public Dictionary<EffectType, float> TempEffects = new();//전투동안 지속되는 효과
    public Dictionary<EffectType, float> EffectsByAtt = new();//대미지를 입히면 적용시키는 효과, 비중첩
    public BaseAtBattle targetOpponent;
    public BaseAtBattle targetAlly;
    public bool IsEnemy { get; protected set; }
    public bool isMonster { get; protected set; }
    //스킬 정보
    private List<SkillActiveForm> skillQueue = new();
    private List<EffectPassiveForm> passiveEffectsAtGrid = new();
    private List<EffectPassiveFormDot> passiveAtDotOpponent = new();
    private List<EffectPassiveFormDot> passiveAtDotAlly = new();
    public Animator animator;
    public GridObject grid;
    private Coroutine moveCoroutine;
    private readonly float ARRIVAL_TIME = 2f;
    public Transform rootTargetTransform;
    public Transform skillTargetTransform;
    public abstract void SetAnimParam();
    protected void InitBase(GridObject _grid)
    {
        transform.SetParent(_grid.transform);
        rootTargetTransform = Instantiate(new GameObject("RootTarget"), transform.GetChild(0)).transform;
        hpObject = Instantiate(GameManager.gameManager.prefabHpBar, transform);
        hpObject.transform.localScale = Vector3.one;
        hpObject.transform.GetComponent<RectTransform>().localPosition = new(0f, -15f, 0f);

        hpBar = hpObject.transform.GetChild(1).GetComponent<Image>();
        armorBar = hpObject.transform.GetChild(0).GetComponent<Image>();
        if (isMonster)
            animator = GetComponent<Animator>();
        else
            animator = transform.GetComponentInChildren<Animator>();
        defaultAttack = new Skill();
    }
    public void InBattleFieldZero()
    {
        abilityInBattle = maxHpInBattle = resistInBattle = speedInBattle = 0f;
    }
    public float CalcEffectValueByType(float calcValue, EffectType _type)
    {
        switch (_type)//Value 보정값 설정
        {
            case EffectType.Damage:
                calcValue += GetRegularValue(EffectType.Enchant);
                float incrementValue = GetRegularValue(EffectType.AttAscend) - GetRegularValue(EffectType.AttDescend);
                calcValue *= Mathf.Max(1 + incrementValue, 0);
                if (GameManager.CalculateProbability(GetRegularValue(EffectType.Critical)))
                {
                    //치명타 판정
                    calcValue *= 2;
                }
                break;
            case EffectType.Bleed:
                calcValue *= 1 + GetRegularValue(EffectType.AttAscend);
                calcValue *= 1 - GetRegularValue(EffectType.AttDescend);
                break;
            case EffectType.Heal:
                calcValue *= 1 + GetRegularValue(EffectType.HealAscend);
                calcValue *= 1 + GetRegularValue(EffectType.ResilienceAscend);
                break;

            case EffectType.AttAscend:
            case EffectType.ResistAscend:
            case EffectType.Enchant:
                calcValue *= 1 + GetRegularValue(EffectType.BuffAscend);
                break;
            case EffectType.AttDescend:
            case EffectType.ResistDescend:
                calcValue *= 1 + GetRegularValue(EffectType.DebuffAscend);
                break;
        }

        return calcValue;
    }
    public void MoveToTargetGrid(GridObject _grid, bool _isInstant = false)
    {
        bool isInstant;
        if (!_isInstant)
            isInstant = (grid == _grid) || BattleScenario.battlePatern == BattlePatern.OnReady;
        else
            isInstant = true;
        float distance = Mathf.Abs(grid.index / 3 - _grid.index / 3) + Mathf.Abs(grid.index % 3 - _grid.index % 3);

        if (grid != _grid)
        {
            if (grid.ExitOnGrid != null)
            {
                grid.ExitOnGrid(this);
            }
            foreach (EffectPassiveForm passive in passiveEffectsAtGrid)
            {
                passive.UnsetPassiveEffect();
            }
            grid = _grid;
            _grid.owner = this;

            FindNewTargetAlly();
            FindNewTargetOpponent();

            if (grid.EnterOnGrid != null)
            {
                grid.EnterOnGrid(this);
            }
            foreach (EffectPassiveForm passive in passiveEffectsAtGrid)
            {
                passive.SetPassiveEffect();
            }
            foreach (BaseAtBattle x in IsEnemy ? BattleScenario.characters : BattleScenario.enemies)
            {
                x.FindNewTargetOpponent();
            }
            foreach (BaseAtBattle x in IsEnemy ? BattleScenario.enemies : BattleScenario.characters)
            {
                x.FindNewTargetAlly();
            }


        }

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        if (isInstant)
        {
            transform.SetParent(_grid.transform);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            moveCoroutine = StartCoroutine(MoveCharacterCoroutine(_grid.transform, distance / 8));
        }
    }
    private IEnumerator MoveCharacterCoroutine(Transform _targetTransform, float _speed)
    {
        if (isMonster)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetFloat("RunState", _speed);
        }


        transform.localPosition = Vector3.zero;
        transform.SetParent(_targetTransform);
        Vector3 initialPosition = transform.localPosition;
        Vector3 targetPosition = Vector3.zero;
        float distanceToTarget = Vector3.Distance(initialPosition, targetPosition);
        float moveSpeed = distanceToTarget / ARRIVAL_TIME; // ARRIVAL_TIME은 도착하는 데 걸리는 시간을 나타내는 상수로 설정

        float startTime = Time.time;
        while (Time.time - startTime < ARRIVAL_TIME)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfDistance = distanceCovered / distanceToTarget;
            transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, fractionOfDistance);
            yield return null;
        }

        transform.localPosition = targetPosition;
        moveCoroutine = null;
        if (isMonster)
        {
            animator.SetBool("Run", false);
        }
        else
        {
            animator.SetFloat("RunState", 0f);
        }

    }
    public abstract void OnDead();
    public float GetRegularValue(EffectType _type)
    {
        float returnValue = 0f;
        if (TempEffects.TryGetValue(_type, out float value0))
        {
            returnValue += value0;
        }
        return returnValue;
    }
    public void ActiveRegularEffect()
    {
        Hp -= GetRegularValue(EffectType.Bleed);
    }
    void ApplyValue(float _value, EffectType _effectType, bool _byAtt = false)
    {
        if (_byAtt)
        {
            if (!EffectsByAtt.ContainsKey(_effectType))
            {
                EffectsByAtt.Add(_effectType, new());
            }
            EffectsByAtt[_effectType] += _value;
        }
        else
        {
            switch (_effectType)
            {
                default:
                    if (!TempEffects.ContainsKey(_effectType))
                    {
                        TempEffects.Add(_effectType, new());
                    }
                    TempEffects[_effectType] += _value;
                    break;
                case EffectType.Necro:
                case EffectType.BleedTransfer:
                case EffectType.CorpseExplo:
                    if (!TempEffects.ContainsKey(_effectType))
                    {
                        TempEffects.Add(_effectType, new());
                    }
                    TempEffects[_effectType] = Mathf.Max(TempEffects[_effectType], _value);
                    break;
                case EffectType.Damage:
                    float temp = armor - _value;
                    if (temp < 0)
                    {
                        armor = 0f;
                        Hp += temp;
                    }
                    else
                    {
                        armor = temp;
                    }
                    break;
                case EffectType.Curse:
                    hp -= _value;
                    break;
                case EffectType.Heal:
                    Hp = Mathf.Min(Hp + _value, maxHpInBattle);
                    break;
                case EffectType.Armor:
                    armor += _value;
                    break;
                case EffectType.AbilityVamp:
                    abilityInBattle -= _value * abilityInBattle;
                    Debug.Log("피격자 : " + abilityInBattle);
                    break;
                case EffectType.Restoration:
                    Hp += (maxHpInBattle - Hp) * _value;
                    break;
                case EffectType.AbilityAscend:
                    abilityInBattle *= 1 + _value;
                    break;

                case EffectType.ResistAscend:
                    resistInBattle += _value * (1 + GetRegularValue(EffectType.ResistAscend_P));
                    break;
                case EffectType.ResistDescend:
                    resistInBattle -= _value;
                    break;
                case EffectType.SpeedAscend:
                    speedInBattle *= 1 + _value;
                    break;
                case EffectType.SpeedDescend:
                    speedInBattle *= Mathf.Max(1 - _value, 0.1f);
                    break;
                case EffectType.RewardAscend:
                    GameManager.battleScenario.RewardAscend += _value;
                    break;
            }
        }
    }
    protected IEnumerator OnDead_Base()
    {
        animator.SetTrigger("Die");
        isDead = true;
        NewTargetForOther();




        yield return new WaitForSeconds(1f);

        foreach (var ally in IsEnemy ? BattleScenario.enemies : BattleScenario.characters)
        {
            float value = ally.GetRegularValue(EffectType.Revive);
            if (value > 0)
            {
                ally.TempEffects.Remove(EffectType.Revive);
                ReviveMethod(value);
                yield break;
            }
        }
        float bleedTransfer = GetRegularValue(EffectType.BleedTransfer);
        List<BaseAtBattle> characters = new();
        if (bleedTransfer > 0)
        {
            foreach (var character in IsEnemy ? BattleScenario.enemies : BattleScenario.characters)
            {
                if (character != this)
                    characters.Add(character);
            }
            BaseAtBattle target = characters[Random.Range(0, characters.Count)];
            target.ApplyValue(GetRegularValue(EffectType.Bleed) * bleedTransfer, EffectType.Bleed);
            Debug.Log("BleedTransfer : " + target.grid.index);
        }
        float exploValue = GetRegularValue(EffectType.CorpseExplo);
        characters = new(IsEnemy ? BattleScenario.enemies : BattleScenario.characters);
        if (exploValue > 0)
        {
            foreach (BaseAtBattle character in characters)
            {
                if (character != this)
                {
                    character.ApplyValue(maxHpInBattle * exploValue, EffectType.Damage);
                }
            }
        }
        float necro = GetRegularValue(EffectType.Necro);
        if (necro > 0)
        {
            grid.owner = null;
            List<GridObject> gridCandiBase = IsEnemy ? BattleScenario.CharacterGrids : BattleScenario.EnemyGrids;
            List<GridObject> gridCandi = gridCandiBase.Where(item => item.owner == null).ToList();
            if (gridCandi.Count > 0)
            {
                (IsEnemy ? BattleScenario.enemies : BattleScenario.characters).Remove(this);
                (IsEnemy ? BattleScenario.characters : BattleScenario.enemies).Add(this);
                GridObject targetGrid = gridCandi[Random.Range(0, gridCandi.Count)];
                MoveToTargetGrid(targetGrid, true);
                IsEnemy = !IsEnemy;
                foreach (EffectPassiveForm passive in passiveEffectsAtGrid)
                {
                    passive.UnsetPassiveEffect();
                }
                StopAllCoroutines();
                StartCoroutine(new SkillActiveForm(this, defaultAttack).StartQueueCycle());
                ReviveMethod(necro);
                Vector3 temp = transform.GetChild(0).rotation.eulerAngles;
                temp.y += 180f;
                transform.GetChild(0).rotation = Quaternion.Euler(temp);
                yield break;
            }
        }
        foreach (EffectPassiveForm passive in passiveEffectsAtGrid)
        {
            passive.UnsetPassiveEffect();
        }
        gameObject.SetActive(false);


        void ReviveMethod(float value)
        {
            animator.SetTrigger("Revive");
            isDead = false;
            maxHpInBattle = Hp = maxHpInBattle * value;
            abilityInBattle = ability * value;
            NewTargetForOther();
            FindNewTargetAlly();
            FindNewTargetOpponent();

        }
        void NewTargetForOther()
        {
            List<BaseAtBattle> enemiesBase = BattleScenario.enemies.Where(item => !item.isDead).ToList();
            List<BaseAtBattle> charactersBase = BattleScenario.characters.Where(item => !item.isDead).ToList();
            foreach (BaseAtBattle x in IsEnemy ? enemiesBase : charactersBase)
            {
                if (x != this)
                    x.FindNewTargetAlly();
            }
            foreach (BaseAtBattle x in IsEnemy ? charactersBase : enemiesBase)
            {
                x.FindNewTargetOpponent();
            }
        }
    }



    public IEnumerator SetSkillsAndStart()
    {
        List<SkillActiveForm> skillActiveForms = new();
        List<Skill> skillsAndDa = new(skills);
        skillsAndDa.Add(defaultAttack);//기본 공격
        foreach (Skill skill in skillsAndDa)
        {
            SkillActiveForm skillActiveForm = null;
            foreach (SkillEffect effect in skill.effects)
            {
                if (effect.isPassive)//패시브 스킬
                {
                    float value = effect.value;
                    if (!effect.isConst)
                        value *= abilityInBattle;
                    value = CalcEffectValueByType(value, effect.type);
                    switch (effect.range)
                    {
                        case EffectRange.Dot:
                            if (skill.isTargetEnemy)//가장 가까운 적
                            {
                                EffectPassiveFormDot formDot = new(effect.type, value);
                                passiveAtDotOpponent.Add(formDot);
                                formDot.ApplyEffect(targetOpponent);
                            }
                            else//가장 가까운 아군
                            {
                                if (targetAlly)
                                {
                                    EffectPassiveFormDot formDot = new(effect.type, value);
                                    passiveAtDotAlly.Add(formDot);
                                    formDot.ApplyEffect(targetAlly);
                                }
                            }
                            break;
                        case EffectRange.Self:
                            {
                                ApplyValue(value, effect.type, effect.byAtt);
                            }
                            break;
                        default:
                            EffectPassiveForm form = new(effect, skill.isTargetEnemy, this, value);
                            passiveEffectsAtGrid.Add(form);
                            form.SetPassiveEffect();
                            break;

                    }
                }
                else//액티브 스킬
                {
                    if (skillActiveForm == null)
                    {
                        skillActiveForm = new SkillActiveForm(this, skill);
                    }
                    skillActiveForm.actvieEffects.Add(new EffectActiveForm(effect, skill.isTargetEnemy, this));
                }
            }
            if (skillActiveForm != null)
            {
                skillActiveForms.Add(skillActiveForm);
            }
            yield return null;
        }
        abilityInBattle = ability;
        speedInBattle = speed;
        resistInBattle = resist;
        foreach (SkillActiveForm x in skillActiveForms)
        {
            StartCoroutine(x.StartQueueCycle());
        }
    }
    public void FindNewTargetOpponent()
    {
        foreach (EffectPassiveFormDot passiveDot in passiveAtDotOpponent)
        {
            if (targetOpponent)
                passiveDot.DeapplyEffect(targetOpponent);
        }
        List<BaseAtBattle> targetByColumn;
        int targetColumn;
        List<BaseAtBattle> targetsBase = (IsEnemy ? BattleScenario.characters : BattleScenario.enemies).Where(item => !item.isDead).ToList();
        if (targetsBase.Count == 0)
        {
            targetOpponent = null;
            return;
        }
        if (IsEnemy)
        {
            targetColumn = targetsBase.Max(item => item.grid.index % 3);
        }
        else
        {
            targetColumn = targetsBase.Min(item => item.grid.index % 3);
        }
        targetByColumn = targetsBase.Where(item => item.grid.index % 3 == targetColumn).ToList();
        if (targetByColumn.Count == 1)
        {
            targetOpponent = targetByColumn[0];
        }
        else
        {
            int RowDist = targetByColumn.Min(item => Mathf.Abs(item.grid.index / 3 - grid.index / 3));
            var temp = targetByColumn.Where(item => Mathf.Abs(item.grid.index / 3 - grid.index / 3) == RowDist).ToList();
            targetOpponent = temp[Random.Range(0, temp.Count)];
        }
        foreach (EffectPassiveFormDot passiveDot in passiveAtDotOpponent)
        {
            passiveDot.ApplyEffect(targetOpponent);
        }
    }
    public void FindNewTargetAlly()
    {
        foreach (EffectPassiveFormDot passiveDot in passiveAtDotAlly)
        {
            passiveDot.DeapplyEffect(targetAlly);
        }
        List<BaseAtBattle> targetsBase = (IsEnemy ? BattleScenario.enemies : BattleScenario.characters).Where(item => !item.isDead).ToList();
        targetsBase.Remove(this);
        if (targetsBase.Count == 0)
        {
            targetAlly = null;
            return;
        }
        int minDist = targetsBase.Min(item => GetDistance(grid.index, item.grid.index));
        List<BaseAtBattle> targetCandi = targetsBase.Where(item => GetDistance(grid.index, item.grid.index) == minDist).ToList();
        targetAlly = targetCandi[Random.Range(0, targetCandi.Count)];
        int GetDistance(int _index0, int _index1)
        {
            int rowDist = Mathf.Abs(_index0 % 3 - _index1 % 3);
            int ColumnDist = Mathf.Abs(_index0 / 3 - _index1 / 3);
            return rowDist + ColumnDist;
        }
    }
    public void StopBattle()
    {
        grid.owner = null;
        StopAllCoroutines();
        skillQueue.Clear();
    }
    public void StartBattle()
    {
        FindNewTargetAlly();
        FindNewTargetOpponent();
        SetAnimParam();
        StartCoroutine(SetSkillsAndStart());
    }
    public class EffectPassiveFormDot
    {
        EffectType type;
        float value;
        public EffectPassiveFormDot(EffectType _type, float _value)
        {
            type = _type;
            value = _value;
        }
        public void ApplyEffect(BaseAtBattle _target)
        {
            _target.ApplyValue(value, type);
        }
        public void DeapplyEffect(BaseAtBattle _target)
        {
            _target.ApplyValue(value * -1, type);
        }
    }

    #region EffectPassiveForm
    public class EffectPassiveForm
    {
        SkillEffect effect;
        float value;//최초 Value 고정
        BaseAtBattle caster;
        public List<GridObject> targetGrids = new();
        bool isTargetEnemy;
        public EffectPassiveForm(SkillEffect _effect, bool _isTargetEnemy, BaseAtBattle _caster, float _value)
        {
            effect = _effect;
            caster = _caster;
            isTargetEnemy = _isTargetEnemy;
            value = _value;
        }
        public void SetPassiveEffect()
        {
            targetGrids = GetGridsByRange(effect.range, caster, isTargetEnemy);
            foreach (var x in targetGrids)
            {
                x.EnterOnGrid += EnterOnGrid;
                if (x.owner != null)
                {
                    EnterOnGrid(x.owner);
                }
                x.ExitOnGrid += ExitOnGrid;
            }
        }
        public void UnsetPassiveEffect()
        {
            foreach (var x in targetGrids)
            {
                x.EnterOnGrid -= EnterOnGrid;
                if (x.owner != null)
                {
                    ExitOnGrid(x.owner);
                }
                x.ExitOnGrid -= ExitOnGrid;
            }
        }
        void EnterOnGrid(BaseAtBattle _target)
        {
            _target.ApplyValue(value, effect.type);
        }
        void ExitOnGrid(BaseAtBattle _target)
        {
            var tempValue = value * -1;
            _target.ApplyValue(tempValue, effect.type);
        }
    }


    public static List<GridObject> GetGridsByRange(EffectRange _range, BaseAtBattle _target, bool _isTargetEnemy)
    {
        List<GridObject> targetGrids = null;
        bool orderDir = _isTargetEnemy ^ _target.IsEnemy;
        List<GridObject> gridsBase = (orderDir ? BattleScenario.EnemyGrids : BattleScenario.CharacterGrids);
        switch (_range)
        {
            case EffectRange.Row:
                targetGrids = gridsBase.Where(item => item.index / 3 == _target.grid.index / 3).ToList();
                break;
            case EffectRange.Column:
                targetGrids = gridsBase.Where(item => item.index % 3 == _target.grid.index % 3).ToList();
                break;
            case EffectRange.Behind:
                if (orderDir)
                    targetGrids = gridsBase.Where(item => item.index % 3 > _target.grid.index % 3).ToList();
                else
                    targetGrids = gridsBase.Where(item => item.index % 3 < _target.grid.index % 3).ToList();
                break;
            case EffectRange.Front:
                if (orderDir)
                    targetGrids = gridsBase.Where(item => item.index % 3 < _target.grid.index % 3).ToList();
                else
                    targetGrids = gridsBase.Where(item => item.index % 3 > _target.grid.index % 3).ToList();
                break;
        }

        return targetGrids;
    }
    #endregion

    #region SkillActiveForm
    public class SkillActiveForm
    {
        public BaseAtBattle caster;
        public List<EffectActiveForm> actvieEffects = new();
        readonly float skillCastTime = 1f;
        public Skill skill;
        public SkillActiveForm(BaseAtBattle _caster, Skill _skill)
        {
            skill = _skill;
            caster = _caster;
        }
        private void StartSkillCor(IEnumerator _coroutine) => caster.StartCoroutine(_coroutine);
        public IEnumerator StartQueueCycle()
        {
            yield return new WaitForSeconds(skill.cooltime / caster.speedInBattle);
            caster.skillQueue.Add(this);
            if (caster.skillQueue.Count == 1)
            {
                StartSkillCor(ActiveSkill());
            }

        }
        public IEnumerator ActiveSkill()
        {
            BaseAtBattle confusedTarget = null;
            bool isParalyze = false;
            float confuseProb = caster.GetRegularValue(EffectType.Confuse);
            if (GameManager.CalculateProbability(confuseProb))
            {
                List<BaseAtBattle> confusedTargetsBase = (caster.IsEnemy ^ skill.isTargetEnemy ? BattleScenario.characters : BattleScenario.enemies).Where(item => !item.isDead).ToList();
                confusedTarget = confusedTargetsBase[Random.Range(0, confusedTargetsBase.Count)];
            }
            else
            {
                float paralyzeProb = caster.GetRegularValue(EffectType.Paralyze);
                isParalyze = GameManager.CalculateProbability(paralyzeProb);
            }

            if (!caster.isDead)
            {
                if (isParalyze)
                {
                    //마비마비맨
                }
                else
                {
                    List<GridObject> targetGrids = new();
                    foreach (EffectActiveForm effectForm in actvieEffects)
                    {
                        BaseAtBattle effectTarget;
                        effectTarget = GetTarget(effectForm);
                        List<GridObject> tempGrids = BattleScenario.GetTargetGridsByRange(effectForm.effect.range, effectTarget.grid);
                        foreach (GridObject grid in tempGrids)
                        {
                            if (!targetGrids.Contains(grid))
                            {
                                targetGrids.Add(grid);
                            }
                        }
                    }
                    if (skill.isPre)
                    {
                        foreach (var x in targetGrids)
                        {
                            x.PreActive();
                        }
                        yield return new WaitForSeconds(3f);
                    }
                    float repeatValue = caster.GetRegularValue(EffectType.Repeat);
                    for (int i = 0; i < ((repeatValue > 0) ? 2 : 1); i++)
                    {
                        
                        foreach (EffectActiveForm effectForm in actvieEffects)
                        {
                            BaseAtBattle effectTarget;
                            effectTarget = GetTarget(effectForm);
                            List<BaseAtBattle> targets = BattleScenario.GetTargetsByRange(effectForm.effect.range, effectTarget);
                            foreach (BaseAtBattle x in targets)
                            {
                                effectForm.ActiveEffect0nTarget(x);
                            }
                        }
                    }
                    for (int i = 0; i < ((repeatValue > 0) ? 2 : 1); i++)
                    {
                        //Skill
                        if (skill.visualEffect != null)
                        {
                            foreach (GridObject grid in targetGrids)
                                if(grid.owner)
                                GameManager.battleScenario.CreateVisualEffect(skill.visualEffect, grid.owner, true);
                        }
                        //Weapon
                        if (caster.weapon != null)
                            caster.StartCoroutine(WeaponVisualEffect());
                        if (skill.isAnim)
                        {
                            SkillAnim();
                            yield return new WaitForSeconds(skillCastTime);
                        }
                    }
                    foreach (GridObject x in targetGrids)
                    {
                        x.PreInactive();
                    }
                }
            }
            caster.skillQueue.Remove(this);

            //Next Skill
            if (caster.skillQueue.Count > 0)
            {

                StartSkillCor(caster.skillQueue[0].ActiveSkill());
            }

            yield return new WaitForSeconds(skill.cooltime / caster.speedInBattle);
            caster.skillQueue.Add(this);
            if (caster.skillQueue.Count == 1)
            {
                StartSkillCor(ActiveSkill());
            }




            void SkillAnim()
            {
                float minValue = Skill.defaultAttackCooltime;
                float maxValue = 8;
                if (!caster.isMonster)
                    caster.animator.SetFloat("AttackState", (skill.cooltime - minValue) / (maxValue - minValue));
                //caster.animator.SetFloat("NormalState", 0.5f);
                //caster.animator.SetFloat("SkillState", 0.5f);
                if (skill.isTargetEnemy)
                    caster.animator.SetTrigger("Attack");
                else
                    caster.animator.SetTrigger("Buff");
            }

            IEnumerator WeaponVisualEffect()
            {
                switch (caster.weapon.type)
                {
                    case WeaponType.Bow:
                        yield return new WaitForSeconds(0.3f);
                        break;
                }
                if (skill.categori == SkillCategori.Default)
                    GameManager.battleScenario.CreateVisualEffect(caster.weapon.defaultVisualEffect, caster, false);
                else
                    GameManager.battleScenario.CreateVisualEffect(caster.weapon.skillVisualEffect, caster, false);
            }

            BaseAtBattle GetTarget(EffectActiveForm effectForm)
            {
                BaseAtBattle effectTarget;
                switch (effectForm.isTargetEnemy)
                {
                    case true:
                        switch (effectForm.effect.range)
                        {
                            default:
                                effectTarget = caster.targetOpponent;
                                break;
                        }
                        break;
                    case false:
                        switch (effectForm.effect.range)
                        {
                            default:
                                effectTarget = caster;
                                break;
                            case EffectRange.Dot:
                                effectTarget = caster.targetAlly;
                                break;
                        }
                        break;
                }

                return effectTarget;
            }
        }
    }
    #endregion

    #region EffectActiveForm
    public class EffectActiveForm
    {//뷁
        public SkillEffect effect;
        public BaseAtBattle caster;
        public bool isTargetEnemy;

        public EffectActiveForm(SkillEffect _effect, bool _isTargetEnemy, BaseAtBattle _caster)
        {
            effect = _effect;
            caster = _caster;
            isTargetEnemy = _isTargetEnemy;
        }
        public void ActiveEffect0nTarget(BaseAtBattle target)
        {
            float calcValue = effect.value;
            if (!effect.isConst)
                calcValue *= caster.abilityInBattle;
            calcValue = caster.CalcEffectValueByType(calcValue, effect.type);
            float calcTemp = calcValue;

            calcValue = calcTemp;
            switch (effect.type)
            {
                case EffectType.Damage://타겟에 대한 보정값
                                       //calcValue 
                    float hpRatio = Mathf.Clamp01(1 - (target.hp / target.maxHpInBattle));
                    hpRatio = 0.1f + 0.9f * hpRatio; // 0에서 1까지의 범위를 0.1에서 1로 이동
                    calcValue *= 1f + hpRatio * caster.GetRegularValue(EffectType.AttAscend_Torment);
                    calcValue *= 100f / (100f + target.resistInBattle);
                    calcValue -= target.GetRegularValue(EffectType.Reduce);
                    calcValue = Mathf.Max(calcValue, 0);
                    foreach (KeyValuePair<EffectType, float> kvp in caster.EffectsByAtt)
                    {
                        target.ApplyValue(kvp.Value, kvp.Key);
                    }
                    break;
                case EffectType.Curse:
                    calcValue *= target.hp;
                    break;
            }
            caster.StartCoroutine(RoopEffect(calcValue, target));
        }
    




        private IEnumerator RoopEffect(float calcValue, BaseAtBattle _target)
        {
            for (int i = 0; i < effect.count; i++)
            {
                //능력치 흡수
                if (effect.type == EffectType.AbilityVamp)
                {
                    caster.abilityInBattle += _target.abilityInBattle * calcValue;
                }
                _target.ApplyValue(calcValue, effect.type);//핵심

                //대미지 후속작업
                if (effect.type == EffectType.Damage)
                {
                    float vampValue = effect.vamp + caster.GetRegularValue(EffectType.Vamp);
                    vampValue *= 1 + caster.GetRegularValue(EffectType.ResilienceAscend);
                    //흡수하는 채력
                    if (vampValue > 0)
                    {
                        caster.Hp += vampValue * calcValue;
                    }
                    //흡수되는 능력치
                    if (_target.GetRegularValue(EffectType.ResistByDamage) > 0)
                    {
                        float resistValue = caster.resistInBattle * _target.GetRegularValue(EffectType.ResistByDamage);
                        caster.resistInBattle -= resistValue;
                        _target.resistInBattle += resistValue;
                    }
                    if (_target.GetRegularValue(EffectType.Reflect) > 0)
                    {
                        caster.Hp -= calcValue * _target.GetRegularValue(EffectType.Reflect);
                    }
                }
                yield return new WaitForSeconds(effect.delay);
            }
        }
    }
    #endregion
}