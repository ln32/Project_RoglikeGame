using BattleCollection;
using EnumCollection;
using Firebase.Firestore;
using LobbyCollection;
using CharacterCollection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class LoadManager : MonoBehaviour//Firestore에 있는 기초 데이터들 로딩해서 저장하고 있는 스크립트
{
    public static LoadManager loadManager;
    bool isInit = false;
    public Dictionary<string, SkillForm> skillsDict = new();//Key는 Document의 ID
    public Dictionary<string, JobClass> jobsDict = new();//Key는 스킬의 Index. Ex)200, 101
    public Dictionary<string, EnemyClass> enemyiesDict = new();//Key는 Document의 ID를 int로 parse
    public Dictionary<string, UpgradeClass> upgradeDict = new();
    public Dictionary<string, TalentFormStruct> talentDict = new();
    public Dictionary<string, EnemyCase> enemyCaseDict = new();
    public Dictionary<WeaponType, Dictionary<string, WeaponClass>> weaponDict = new();
    private readonly string visualEffectPath = "Prefab/VisualEffect";
    public Dictionary<string, VisualEffect> skillVisualEffectDict = new();
    public Dictionary<string, VisualEffect> weaponVisualEffectDict = new();
    public Dictionary<Species, Dictionary<string, BodyPartClass>> BodyPartDict = new();
    public Dictionary<Species, Dictionary<string, EyeClass>> EyeDict = new();
    public Dictionary<string, Sprite> hairDict = new();
    public Dictionary<string, Sprite> faceHairDict = new();
    public Dictionary<string, Dictionary<ClothesPart, Sprite >> clothesDict = new();
    private void Awake()
    {
        if (!loadManager)
        {
            loadManager = this;
        }
    }
    public void Start()
    {
        StartAsync();
    }
    async void StartAsync()
    {
        if (!isInit)
        {
            await LoadVisualEffect();
            await InitSkill();
            await Task.WhenAll(InitJob(), InitEnemy(), InitUpgrade(), InitTalent(), InitUserDoc(), InitEnemyCase(), InitWeapon());
            InitBodyPart(); InitEye(); InitFaceHair(); InitHair(); InitClothes();
            Debug.Log("LoadComplete");
            isInit = true;
        }
    }
    async Task LoadVisualEffect()
    {
        
        LoadVisualEffectType("Weapon", weaponVisualEffectDict);
        LoadVisualEffectType("Skill", skillVisualEffectDict);
        async void LoadVisualEffectType(string _type, Dictionary<string, VisualEffect> _dict)
        {
            Dictionary<string, object> durationData = await DataManager.dataManager.GetField("VisualEffect", _type);
            GameObject[] visualEffects = Resources.LoadAll<GameObject>(visualEffectPath + "/" + _type);
            
            foreach (GameObject visualEffect in visualEffects)
            {
                float duration = 1f;
                string sound = string.Empty;
                bool fromRoot = false;
                if (durationData.TryGetValue(visualEffect.name, out object obj0))
                {
                    object obj1;
                    Dictionary<string, object> ve = obj0 as Dictionary<string, object>;
                    if (ve.TryGetValue("Duration", out obj1))
                    {
                        duration = GetFloatValue(obj1);
                    }
                    if (ve.TryGetValue("Sound", out obj1))
                    {
                        sound = (string)obj1;
                    }
                    if (ve.TryGetValue("FromRoot", out obj1))
                    {
                        fromRoot = (bool)obj1;
                    }
                }
                _dict.Add(visualEffect.name, new VisualEffect(visualEffect, duration, sound, fromRoot));
            }
        }

    }
    private async Task InitSkill()
    {
        List<DocumentSnapshot> documents = await DataManager.dataManager.GetDocumentSnapshots("Skill");
        foreach (DocumentSnapshot doc in documents)
        {
            SkillForm skillForm = new();
            Dictionary<string, object> skillDict = doc.ToDictionary();//도큐먼트
            if ((string)skillDict["Categori"] == "Withhold")
                continue;

            List<Dictionary<Language, string>> explains;

            SkillCategori categori;
            switch ((string)skillDict["Categori"])
            {
                default:
                    categori = SkillCategori.Power;
                    break;
                case "Sustain":
                    categori = SkillCategori.Sustain;
                    break;
                case "Util":
                    categori = SkillCategori.Util;
                    break;
                case "Enemy":
                    categori = SkillCategori.Enemy;
                    break;
            }
            skillForm.SetCategori(categori);
            List<List<SkillEffect>> effects;
            if (categori == SkillCategori.Enemy)//적의 스킬
            {
                List<SkillEffect> effect = InitEffect("Effect", skillDict);
                effects = new() { effect };
                skillForm.SetEffects(effects);
                explains = null;
            }
            else//아군 캐릭터가 사용하는 스킬
            {
                Dictionary<Language, string> name = new();
                if (skillDict.TryGetValue("Name", out object nameField))
                {
                    foreach (var x in nameField as Dictionary<string, object>)
                    {
                        switch (x.Key)
                        {
                            default:
                                name.Add(Language.Ko, (string)x.Value);
                                break;
                            case "En":
                                name.Add(Language.En, (string)x.Value);
                                break;
                        }
                    }
                }
                skillForm.SetName(name);
                //Debug.Log("Skill Init : " + doc.Id + ", " + "Effect_0");
                List<SkillEffect> effect0 = InitEffect("Effect_0", skillDict);
                //Debug.Log("Skill Init : " + doc.Id + ", " + "Effect_1");
                List<SkillEffect> effect1 = InitEffect("Effect_1", skillDict);
                //Debug.Log("Skill Init : " + doc.Id + ", " + "Effect_2");
                List<SkillEffect> effect2 = InitEffect("Effect_2", skillDict);
                skillForm.SetEffects(new() { effect0, effect1, effect2 });
                explains = new();
                foreach (var x0 in skillDict["Explain"] as List<object>)
                {
                    Dictionary<Language, string> temp = new();
                    foreach (var x1 in x0 as Dictionary<string, object>)
                    {
                        switch (x1.Key)
                        {
                            default:
                                temp.Add(Language.Ko, (string)x1.Value);
                                break;
                            case "En":
                                temp.Add(Language.En, (string)x1.Value);
                                break;
                        }
                    }
                    explains.Add(temp);
                }
                skillForm.SetExplain(explains);
            }
            object obj;
            //CoolTime
            if (skillDict.TryGetValue("Cooltime", out obj))
                skillForm.SetCooltime(GetFloatValue(obj));
            //IsTargetEnemy
            bool isTargetEnemy;
            if (skillDict.TryGetValue("IsTargetEnemy", out obj))
                isTargetEnemy = (bool)obj;
            else
                isTargetEnemy = true;
            skillForm.SetIsTargetEnemy(isTargetEnemy);
            //IsAnim
            bool isAnim;
            if (skillDict.TryGetValue("IsAnim", out obj))
                isAnim = (bool)obj;
            else
                isAnim = true;
            skillForm.SetIsAnim(isAnim);
            //SkillEffect
            List<string> skillEffect = new();
            if (skillDict.TryGetValue("VisualEffect", out obj))
            {
                foreach (object x in (List<object>)obj)
                {
                    skillEffect.Add((string)x);
                }
            }
            else
                skillEffect = null;
            skillForm.SetSkillEffect(skillEffect);
            //IsPre
            bool isPre;
            if (skillDict.TryGetValue("IsPre", out obj))
                isPre = (bool)obj;
            else
                isPre = false;
            skillForm.SetIsPre(isPre);
            skillsDict.Add(doc.Id, skillForm);


        }
    }
    List<SkillEffect> InitEffect(string _effectStr, Dictionary<string, object> _skillDict)
    {
        List<SkillEffect> effects = new();
        if (!(_skillDict.TryGetValue(_effectStr, out object listObj)))
            return null;
        foreach (object docDict in listObj as List<object>)
        {
            SkillEffect effect = new();
            Dictionary<string, object> effectDict = docDict as Dictionary<string, object>;
            //Value
            object obj;
            if (effectDict.TryGetValue("Value", out obj))
                effect.SetValue(GetFloatValue(obj));
            //Count
            if (effectDict.TryGetValue("Count", out obj))
                effect.SetCount((int)(long)obj);
            //Range
            EffectRange range;
            if (effectDict.TryGetValue("Range", out obj))
            {
                switch (obj)
                {
                    default:
                        range = EffectRange.Dot;
                        break;
                    case "Self":
                        range = EffectRange.Self;
                        break;
                    case "Row":
                        range = EffectRange.Row;
                        break;
                    case "Column":
                        range = EffectRange.Column;
                        break;
                    case "Behind":
                        range = EffectRange.Behind;
                        break;
                    case "Front":
                        range = EffectRange.Front;
                        break;
                }
                effect.SetRange(range);
            }
            //IsConst
            if (effectDict.TryGetValue("IsConst", out obj))
                effect.SetIsConst((bool)effectDict["IsConst"]);
            //Type
            EffectType type;
            type = ParseEffectType((string)effectDict["Type"]);
            effect.SetType(type);
            //Delay
            if (effectDict.TryGetValue("Delay", out obj))
                effect.SetDelay(GetFloatValue(obj));
            //IsPassive
            if (effectDict.TryGetValue("IsPassive", out obj))
                effect.SetIsPassive((bool)obj);
            //Vamp
            if (effectDict.TryGetValue("Vamp", out obj))
                effect.SetVamp(GetFloatValue(obj));
            //ByAtt
            if (effectDict.TryGetValue("ByAtt", out obj))
                effect.SetByAtt(((bool)obj));
            //적용
            effects.Add(effect);
        }
        return effects;
    }
    private static EffectType ParseEffectType(string _typeName)
    {
        EffectType type;
        switch (_typeName)
        {
            case "AttAscend":
                type = EffectType.AttAscend;
                break;
            case "ResistAscend":
                type = EffectType.ResistAscend;
                break;
            case "AttDescend":
                type = EffectType.AttDescend;
                break;
            case "ResistDescend":
                type = EffectType.ResistDescend;
                break;
            case "SpeedDescend":
                type = EffectType.SpeedDescend;
                break;
            case "Bleed":
                type = EffectType.Bleed;
                break;
            case "Reflect":
                type = EffectType.Reflect;
                break;
            case "Paralyze":
                type = EffectType.Paralyze;
                break;
            case "Enchant":
                type = EffectType.Enchant;
                break;
            case "Repeat":
                type = EffectType.Repeat;
                break;
            case "Heal":
                type = EffectType.Heal;
                break;
            case "Restoration":
                type = EffectType.Restoration;
                break;
            case "Armor":
                type = EffectType.Armor;
                break;
            case "AbilityAscend":
                type = EffectType.AbilityAscend;
                break;
            case "BleedTransfer":
                type = EffectType.BleedTransfer;
                break;
            case "AbilityVamp":
                type = EffectType.AbilityVamp;
                break;
            case "AbilityByDamage":
                type = EffectType.ResistByDamage;
                break;
            case "Vamp":
                type = EffectType.Vamp;
                break;
            case "Confuse":
                type = EffectType.Confuse;
                break;
            case "Damage":
                type = EffectType.Damage;
                break;
            case "Reduce":
                type = EffectType.Reduce;
                break;
            case "Curse":
                type = EffectType.Curse;
                break;
            case "Revive":
                type = EffectType.Revive;
                break;
            case "ResistByDamage":
                type = EffectType.ResistByDamage;
                break;
            case "Necro":
                type = EffectType.Necro;
                break;
            case "GoldAscend":
                type = EffectType.GoldAscend;
                break;
            case "FameAscend":
                type = EffectType.FameAscend;
                break;
            case "Critical":
                type = EffectType.Critical;
                break;
            case "DebuffAscend":
                type = EffectType.DebuffAscend;
                break;
            case "HealAscend":
                type = EffectType.HealAscend;
                break;
            case "CorpseExplo":
                type = EffectType.CorpseExplo;
                break;
            case "ResistAscend_P":
                type = EffectType.ResistAscend_P;
                break;
            case "BuffAscend":
                type = EffectType.BuffAscend;
                break;
            case "RewardAscend":
                type = EffectType.RewardAscend;
                break;
            case "AttAscend_Torment":
                type = EffectType.AttAscend_Torment;
                break;
            case "ResilienceAscend":
                type = EffectType.ResilienceAscend;
                break;
            default:
                Debug.LogError("Init Effect By Default..." + _typeName);
                type = EffectType.Damage;
                break;
        }

        return type;
    }

    private async Task InitJob()
    {
        List<DocumentSnapshot> documents = await DataManager.dataManager.GetDocumentSnapshots("Job");
        foreach (DocumentSnapshot doc in documents)
        {
            object obj;
            Dictionary<string, object> dict = doc.ToDictionary();
            JobClass jobClass = new();
            if (dict.TryGetValue("Effect", out obj))
            {
                List<object> objList = obj as List<object>;
                List<SkillEffect> effects = new();
                foreach (object x in objList)
                {
                    Dictionary<string, object> effectDict = x as Dictionary<string, object>;
                    string typeStr = (string)effectDict["Type"];
                    bool byAtt;
                    if (effectDict.TryGetValue("ByAtt", out obj))
                    {
                        byAtt = (bool)obj;
                    }
                    else
                        byAtt = false;
                    EffectType effectType = ParseEffectType(typeStr);
                    float value;
                    if (effectDict.TryGetValue("Value", out obj))
                    {
                        value = GetFloatValue(obj);
                    }
                    else
                        value = 0f;
                    SkillEffect skillEffect = new SkillEffect().SetType(effectType).SetValue(value).SetByAtt(byAtt);
                    effects.Add(skillEffect);
                }
                jobClass.SetEffects(effects);
            }
            string jobId = (string)dict["JobId"];
            //Name
            Dictionary<Language, string> name = new();
            Dictionary<string, object> nameTemp = dict["Name"] as Dictionary<string, object>;
            name.Add(Language.Ko, (string)nameTemp["Ko"]);
            name.Add(Language.En, (string)nameTemp["En"]);
            jobClass.SetName(name);

            jobsDict.Add(jobId, jobClass);
        }
    }
    private async Task InitEnemy()
    {
        List<DocumentSnapshot> documents = await DataManager.dataManager.GetDocumentSnapshots("Enemy");
        foreach (DocumentSnapshot doc in documents)
        {
            try
            {
                EnemyClass enemyClass = new EnemyClass();
                Dictionary<string, object> dict = doc.ToDictionary();
                object obj;
                //Name
                Dictionary<Language, string> name = new();
                Dictionary<string, object> nameTemp = dict["Name"] as Dictionary<string, object>;
                name.Add(Language.Ko, (string)nameTemp["Ko"]);
                name.Add(Language.En, (string)nameTemp["En"]);
                enemyClass.SetName(name);

                //Skill
                List<Skill> skills = new();
                foreach (object x in dict["Skills"] as List<object>)
                {
                    skills.Add(GameManager.LocalizeSkill((string)x));
                }
                enemyClass.SetSkills(skills);
                //Ability
                if (dict.TryGetValue("Ability", out obj))
                {
                    enemyClass.SetAbility(GetFloatValue(obj));
                }
                //Hp
                if (dict.TryGetValue("Hp", out obj))
                {
                    enemyClass.SetHp(GetFloatValue(obj));
                }
                //Resist
                if (dict.TryGetValue("Resist", out obj))
                {
                    enemyClass.SetResist(GetFloatValue(obj));
                }
                //Speed
                if (dict.TryGetValue("Speed", out obj))
                {
                    enemyClass.SetSpeed(GetFloatValue(obj));
                }
                //IsMonster
                if (dict.TryGetValue("IsMonster", out obj))
                {
                    enemyClass.SetIsMonster((bool)obj);
                }
                //Type
                if (dict.TryGetValue("Type", out obj))
                {
                    enemyClass.SetType((string)obj);
                }
                //EnemyLevel
                if (dict.TryGetValue("EnemyLevel", out obj))
                {
                    enemyClass.SetEnemyLevel((int)(long)(obj));
                }
                enemyiesDict.Add(doc.Id, enemyClass);
            }
            catch
            {
                Debug.Log("Error At : " + doc.Id);
            }
        }
    }
    private async Task InitUpgrade()
    {
        List<DocumentSnapshot> documents = await DataManager.dataManager.GetDocumentSnapshots("Upgrade");
        foreach (DocumentSnapshot doc in documents)
        {
            Dictionary<string, object> dict = doc.ToDictionary();
            List<UpgradeContent> guildContents = new();
            foreach (object contentObj in dict["Contents"] as List<object>)
            {
                Dictionary<string, object> contentDict = contentObj as Dictionary<string, object>;
                float value = GetFloatValue(contentDict["Value"]);
                int price = (int)(long)contentDict["Price"];
                guildContents.Add(new(value, price));
            }
            Dictionary<Language, string> name = new();
            foreach (var x in dict["Name"] as Dictionary<string, object>)
            {
                switch (x.Key)
                {
                    default:
                        name.Add(Language.Ko, (string)x.Value);
                        break;
                    case "En":
                        name.Add(Language.En, (string)x.Value);
                        break;
                }
            }
            Dictionary<Language, string> explain = new();
            foreach (var x in dict["Explain"] as Dictionary<string, object>)
            {
                string value = (string)x.Value;
                value = value.Replace("<E>", "<color=#4C4CFF><size=0.3><b>");
                value = value.Replace("</E>", "</b></size></color>");
                value = value.Replace("\\n", "\n");
                switch (x.Key)
                {
                    default:
                        explain.Add(Language.Ko, value);
                        break;
                    case "En":
                        explain.Add(Language.En, value);
                        break;
                }
            }
            Dictionary<Language, string> info = new();
            foreach (var x in dict["Info"] as Dictionary<string, object>)
            {
                string value = (string)x.Value;
                switch (x.Key)
                {
                    default:
                        info.Add(Language.Ko, value);
                        break;
                    case "En":
                        info.Add(Language.En, value);
                        break;
                }
            }
            UpgradeEffectType type;
            switch (dict["Type"])
            {
                default:
                    Debug.LogError("InitDefault... " + dict["Type"]);
                    type = UpgradeEffectType.AllocateNumberUp;
                    break;
                case "AllocateNumberUp":
                    type = UpgradeEffectType.AllocateNumberUp;
                    break;
                case "TalentEffectUp":
                    type = UpgradeEffectType.TalentEffectUp;
                    break;
                case "TalentLevelUp":
                    type = UpgradeEffectType.TalentLevelUp;
                    break;
                case "StatusUp":
                    type = UpgradeEffectType.StatusUp;
                    break;
                case "FameUp":
                    type = UpgradeEffectType.FameUp;
                    break;
                case "GoldUp":
                    type = UpgradeEffectType.GoldUp;
                    break;
            }
            upgradeDict.Add(doc.Id, new UpgradeClass(name, (int)(long)dict["Index"], guildContents, explain, info, type, (string)dict["LobbyCase"]));
        }
    }
    private async Task InitTalent()
    {
        List<DocumentSnapshot> documents = await DataManager.dataManager.GetDocumentSnapshots("Talent");
        foreach (DocumentSnapshot doc in documents)
        {
            Dictionary<string, object> dict = doc.ToDictionary();
            Dictionary<Language, string> name = new();
            Dictionary<Language, string> explain = new();
            foreach (KeyValuePair<string, object> keyPair in dict["Name"] as Dictionary<string, object>)
            {
                switch (keyPair.Key)
                {
                    default:
                        name.Add(Language.Ko, (string)keyPair.Value);
                        break;
                    case "En":
                        name.Add(Language.En, (string)keyPair.Value);
                        break;
                }
            }
            foreach (KeyValuePair<string, object> keyPair in dict["Explain"] as Dictionary<string, object>)
            {
                switch (keyPair.Key)
                {
                    default:
                        explain.Add(Language.Ko, (string)keyPair.Value);
                        break;
                    case "En":
                        explain.Add(Language.En, (string)keyPair.Value);
                        break;
                }
            }
            List<TalentEffectForm> effects = new();
            {
                List<object> effectList = dict["Effect"] as List<object>;
                foreach (object x in effectList)
                {
                    Dictionary<string, object> effectDict = x as Dictionary<string, object>;
                    EffectType effectType = ParseEffectType((string)effectDict["Type"]);
                    effects.Add(new((string)effectDict["Value"], effectType));
                }
            }
            talentDict.Add(doc.Id, new TalentFormStruct(name, (int)(long)dict["Level"], explain, effects, (int)(long)dict["Order"]));
        }
    }

    private async Task InitUserDoc()
    {
        DocumentReference userRef = FirebaseFirestore.DefaultInstance.Collection("User").Document(GameManager.gameManager.Uid);
        DocumentSnapshot snapshot = await userRef.GetSnapshotAsync();

        await FirebaseFirestore.DefaultInstance.RunTransactionAsync(async transaction =>
        {
            if (snapshot.Exists)
            {
                await GameManager.gameManager.LoadUserDoc();
            }
            else
            {
                for (int i = 0; i < upgradeDict.Count; i++)
                {
                    DataManager.dataManager.SetDocumentData("Guild_" + i, 0, "User", GameManager.gameManager.Uid);
                }
                DataManager.dataManager.SetDocumentData("Fame", 0, "User", GameManager.gameManager.Uid);
                GameManager.gameManager.fame = 0;
            }
        });
    }
    private async Task InitEnemyCase()
    {
        List<DocumentSnapshot> documents = await DataManager.dataManager.GetDocumentSnapshots("EnemyCase");
        foreach (DocumentSnapshot doc in documents)
        {
            EnemyCase enemyCase = new EnemyCase();
            Dictionary<string, object> dict = doc.ToDictionary();
            //Enemies
            List<EnemyPieceForm> enemyPieces = new();//id, grid
            List<object> enemies = dict["Enemies"] as List<object>;
            foreach (object enemyObj in enemies)
            {
                Dictionary<string, object> pieceObj = enemyObj as Dictionary<string, object>;
                object obj;
                var piece = new EnemyPieceForm();
                if (pieceObj.TryGetValue("Id", out obj))
                {
                    piece.SetId((string)obj);
                }
                else if (pieceObj.TryGetValue("Type", out obj))
                {
                    piece.SetType((string)obj);
                }
                else if (pieceObj.TryGetValue("EnemyLevel", out obj))
                {
                    piece.SetLevel((int)(long)obj);
                }
                piece.SetIndex((int)(long)pieceObj["Index"]);
                enemyPieces.Add(piece);
            }
            enemyCase.SetEnemies(enemyPieces);
            //LevelRange
            List<int> levelRange = new();
            foreach (var x in dict["LevelRange"] as List<object>)
            {
                levelRange.Add((int)(long)x);
            }
            enemyCase.SetLevelRange(levelRange);
            enemyCaseDict.Add(doc.Id, enemyCase);
        }
    }
    private async Task InitWeapon()
    {
        await Task.WhenAll(InitWeaponOnType("Bow"),
            InitWeaponOnType("Magic"),
            InitWeaponOnType("Sword"),
            InitWeaponOnType("Club"));

        async Task InitWeaponOnType(string _weaponTypeStr)
        {
            List<DocumentSnapshot> documents = await DataManager.dataManager.GetDocumentSnapshots(string.Format("{0}/{1}/{2}", "Weapon", "Data", _weaponTypeStr));
            //Type
            WeaponType type;
            switch (_weaponTypeStr)
            {
                default:
                    type = WeaponType.Sword;
                    break;
                case "Bow":
                    type = WeaponType.Bow;
                    break;
                case "Magic":
                    type = WeaponType.Magic;
                    break;
                case "Club":
                    type = WeaponType.Club;
                    break;
            }
            weaponDict.Add(type, new());
            foreach (DocumentSnapshot doc in documents)
            {
                WeaponClass weaponClass = new WeaponClass();
                Dictionary<string, object> dict = doc.ToDictionary();
                //Effects
                List<SkillEffect> effects = InitEffect("Effects", dict);
                if (effects != null)
                    foreach (SkillEffect x in effects)
                    {
                        x.SetIsPassive(true);
                    }
                weaponClass.SetEffects(effects);
                //Grade
                WeaponGrade grade;
                switch (dict["Grade"])
                {
                    default:
                        grade = WeaponGrade.Normal;
                        break;
                    case "Rare":
                        grade = WeaponGrade.Rare;
                        break;
                    case "Unique":
                        grade = WeaponGrade.Unique;
                        break;
                }
                weaponClass.SetGrade(grade);
                //Id
                weaponClass.SetId(doc.Id);
                //Stat
                if (dict.TryGetValue("Status", out object obj))
                {
                    Dictionary<string, object> statusDict = obj as Dictionary<string, object>;
                    object obj1;
                    float ability;
                    if (statusDict.TryGetValue("Ability", out obj1))
                        ability = GetFloatValue(obj1);
                    else
                        ability = 0f;
                    float hp;
                    if (statusDict.TryGetValue("Hp", out obj1))
                        hp = GetFloatValue(obj1);
                    else
                        hp = 0f;
                    float resist;
                    if (statusDict.TryGetValue("Resist", out obj1))
                        resist = GetFloatValue(obj1);
                    else
                        resist = 0f;
                    float speed;
                    if (statusDict.TryGetValue("Speed", out obj1))
                        speed = GetFloatValue(obj1);
                    else
                        speed = 0f;
                    weaponClass.SetStatus(ability, hp, resist, speed);
                }
                weaponClass.SetType(type);
                //VisualEffect
                VisualEffect defaultVisualEffect = null;
                VisualEffect skillVisualEffect = null;
                if (dict.TryGetValue("VisualEffect", out obj))
                {
                    Dictionary<string, object> veDict = obj as Dictionary<string, object>;
                    defaultVisualEffect = weaponVisualEffectDict[(string)veDict["Default"]];
                    skillVisualEffect = weaponVisualEffectDict[(string)veDict["Skill"]];
                }
                weaponClass.SetVisualEffect(defaultVisualEffect, skillVisualEffect);
                //Sprite
                Sprite sprite = Resources.Load<Sprite>(string.Format("{0}/{1}/{2}/{3}", "Texture", "Weapon", _weaponTypeStr, doc.Id));
                weaponClass.SetSprite(sprite);

                weaponDict[type].Add(doc.Id, weaponClass);
            }
        }
    }
    private void InitBodyPart()
    {
        InitBySpecies(Species.Human);
        InitBySpecies(Species.Elf);
        InitBySpecies(Species.Devil);
        InitBySpecies(Species.Skelton);
        InitBySpecies(Species.Orc);

        void InitBySpecies(Species _species)
        {
            string speciesStr = string.Empty;
            BodyPartDict.Add(_species, new());
            switch (_species)
            {
                case Species.Human:
                    speciesStr = "0_Human";
                    break;
                case Species.Elf:
                    speciesStr = "1_Elf";
                    break;
                case Species.Devil:
                    speciesStr = "2_Devil";
                    break;
                case Species.Skelton:
                    speciesStr = "3_Skelton";
                    break;
                case Species.Orc:
                    speciesStr = "4_Orc";
                    break;
            }
            Sprite[] sprites = Resources.LoadAll<Sprite>("Texture/Character/Body/" + speciesStr);

            foreach (Sprite sprite in sprites)
            {
                if (!BodyPartDict[_species].ContainsKey(sprite.texture.name))
                {
                    BodyPartDict[_species].Add(sprite.texture.name, new());
                }
                switch (sprite.name)
                {
                    case "Arm_L":
                        BodyPartDict[_species][sprite.texture.name].armL = sprite;
                        break;
                    case "Arm_R":
                        BodyPartDict[_species][sprite.texture.name].armR = sprite;
                        break;
                    case "Head":
                        BodyPartDict[_species][sprite.texture.name].head = sprite;
                        break;
                    default:
                        continue;
                }
            }
        }
    }
    private void InitEye()
    {
        InitBySpecies(Species.Human);
        InitBySpecies(Species.Elf);
        InitBySpecies(Species.Devil);
        InitBySpecies(Species.Skelton);
        InitBySpecies(Species.Orc);

        void InitBySpecies(Species _species)
        {
            string speciesStr = string.Empty;
            EyeDict.Add(_species, new());
            switch (_species)
            {
                case Species.Human:
                    speciesStr = "0_Human";
                    break;
                case Species.Elf:
                    speciesStr = "1_Elf";
                    break;
                case Species.Devil:
                    speciesStr = "2_Devil";
                    break;
                case Species.Skelton:
                    speciesStr = "3_Skelton";
                    break;
                case Species.Orc:
                    speciesStr = "4_Orc";
                    break;
            }
            Sprite[] sprites = Resources.LoadAll<Sprite>("Texture/Character/Eye/" + speciesStr);

            foreach (Sprite sprite in sprites)
            {
                if (!EyeDict[_species].ContainsKey(sprite.texture.name))
                {
                    EyeDict[_species].Add(sprite.texture.name, new());
                }
                switch (sprite.name)
                {
                    case "Front":
                        EyeDict[_species][sprite.texture.name].front = sprite;
                        break;
                    case "Back":
                        EyeDict[_species][sprite.texture.name].back = sprite;
                        break;
                }
            }
        }
    }
    private void InitHair()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Texture/Character/Hair");
        foreach (Sprite sprite in sprites)
            hairDict.Add(sprite.name, sprite);
    }
    private void InitFaceHair()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Texture/Character/FaceHair");
        foreach (Sprite sprite in sprites)
            faceHairDict.Add(sprite.name, sprite);
    }
    float GetFloatValue(object _obj)
    {
        if (_obj is long)
            return (int)(long)_obj;
        else
            return (float)(double)_obj;
    }
    private void InitClothes()
    {
        InitClothesPiece("002");
        InitClothesPiece("011");
        InitClothesPiece("020");
        InitClothesPiece("101");
        InitClothesPiece("110");
        InitClothesPiece("200");
        void InitClothesPiece(string _job)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>($"Texture/Clothes/{_job}");
            Dictionary<ClothesPart, Sprite> tempDict = new();
            foreach (Sprite sprite in sprites)
            {
                ClothesPart clothesPart;
                switch (sprite.name)
                {
                    default:
                        clothesPart = ClothesPart.Back;
                        break;
                    case "ClothBody":
                        clothesPart = ClothesPart.ClothBody;
                        break;
                    case "ClothRight":
                        clothesPart = ClothesPart.ClothRight;
                        break;
                    case "ClothLeft":
                        clothesPart = ClothesPart.ClothLeft;
                        break;
                    case "ArmorBody":
                        clothesPart = ClothesPart.ArmorBody;
                        break;
                    case "ArmorRight":
                        clothesPart = ClothesPart.ArmorRight;
                        break;
                    case "ArmorLeft":
                        clothesPart = ClothesPart.ArmorLeft;
                        break;
                    case "Helmet":
                        clothesPart = ClothesPart.Helmet;
                        break;
                    case "FootRight":
                        clothesPart = ClothesPart.FootRight;
                        break;
                    case "FootLeft":
                        clothesPart = ClothesPart.FootLeft;
                        break;
                }
                tempDict.Add(clothesPart, sprite);
            }
            clothesDict.Add(_job, tempDict);
        }
    }
    [ContextMenu("SetDoc")]
    public async void SetDoc()
    {
        for (int i =0;i<9;i++)
        {
            List<DocumentSnapshot> docs = await DataManager.dataManager.GetDocumentSnapshots($"SimulationCharacterInfo/Simulation_{i}/Characters");
            for (int i1 = 0; i1 < docs.Count; i1++)
            {
                DocumentSnapshot x = docs[i1];
                Dictionary<string, object> dict = new();
                dict.Add("Ability", 10);
                dict.Add("Hp", 100);
                dict.Add("Index", i1+3);
                dict.Add("MaxHp", 100);
                dict.Add("Resist", 10);
                dict.Add("Skill_0", string.Empty);
                dict.Add("Skill_1", string.Empty);
                dict.Add("Speed", 1);
                dict.Add("WeaponId", "Sword:::Default");
                await x.Reference.UpdateAsync(dict);

            }
        }
    }
}