using BattleCollection;
using CharacterCollection;
using EnumCollection;
using Firebase.Firestore;
using LobbyCollection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public GameObject uiCamera;
    public Difficulty difficulty;
    public static Language language;
    private string uid;
    public string Uid
    {
        get { return uid; }
    }
    public Dictionary<string, object> userDoc;
    public Dictionary<string, object> progressDoc;
    public static int seed { get; private set; }
    public int fame;
    public float gold;
    public float fameAscend = 0f;
    public float goldAscend = 0f;
    public Dictionary<string, int> upgradeLevelDict = new();//DocId : Level
    public Dictionary<UpgradeEffectType, float> upgradeValueDict = new();
    private static bool isPaused = false;
    public static bool IsPaused
    {
        get { return isPaused; }
        set { Time.timeScale = value ? 0 : 1; isPaused = value; }
    }
    public static BattleScenario battleScenario;
    public static LobbyScenario lobbyScenario;
    public static StartScenario startScenario;
    public GameObject CharacterTemplate;
    #region CanvasGrid
    public Transform canvasGrid;
    public GameObject prefabHpBar;
    public GameObject prefabGridObject;
    #endregion
    public static readonly Color[] talentColors = new Color[4] { Color.blue, Color.green, Color.yellow, Color.red };
    EventTrigger eventTrigger;
    public int nodeLevel;
    public int stage;
    public string scene;
    public string history;
    public string invenData;
    void Awake()//매니저 세팅은 Awake
    {
        if (!gameManager)
        {
            gameManager = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(uiCamera);
            DontDestroyOnLoad(canvasGrid);
            InitGrids();
            uiCamera.SetActive(false);
            //Until Steam API
            uid = "KF5U1XMs5cy7n13dgKjF";//종현
            //uid = "FMefxTlgP9aHsgfE0Grc";
        }
    }
    async void Start()
    {
        progressDoc = await DataManager.dataManager.GetField("Progress", Uid);
        BattleScenario.characters = new();
        BattleScenario.enemies = new();
    }
    public async Task LoadUserDoc()
    {
        userDoc = await DataManager.dataManager.GetField("User", Uid);
        Dictionary<string, object> upgradeDict;
        if (userDoc.TryGetValue("Upgrade", out object upgradeObj))
        {
            upgradeDict = upgradeObj as Dictionary<string, object>;
        }
        else
        {
            upgradeDict = new();
        }
        fame = (int)(long)userDoc["Fame"];
        bool needToSet = false;

        foreach (KeyValuePair<string, UpgradeClass> kvp in LoadManager.loadManager.upgradeDict)
        {
            if (upgradeDict.TryGetValue(kvp.Key, out object userObj))
            {
                int level = (int)(long)userObj;
                upgradeLevelDict.Add(kvp.Key, level);
                if (level != 0)
                {
                    UpgradeEffectType type = kvp.Value.type;
                    if (upgradeValueDict.ContainsKey(type))
                        upgradeValueDict[type] += kvp.Value.content[level - 1].value;
                    else
                    {

                        upgradeValueDict.Add(type, kvp.Value.content[level - 1].value);//1 Level이면 0번 째 Value를 챙겨야 함
                    }
                }
            }
            else
            {
                needToSet = true;
                upgradeLevelDict.Add(kvp.Key, 0);
            }
        }
        if (needToSet)
        {
            Dictionary<string, object> objDict = DataManager.dataManager.ConvertToObjDictionary(upgradeLevelDict);
            DataManager.dataManager.SetDocumentData("Upgrade", objDict, "User", Uid);
        }

    }
    private void OnSceneLoaded(Scene _arg0, LoadSceneMode _arg1)
    {
        if (!(_arg0.name != "Awake" || _arg0.name != "Start"))
            DataManager.dataManager.SetDocumentData("Scene", _arg0.name, "Progress", Uid);
        if (_arg0.name == "Battle")
            uiCamera.SetActive(true);
        else
            uiCamera.SetActive(false);
    }
    private void InitGrids()
    {
        canvasGrid.gameObject.SetActive(false);
        Transform panelCharacter = canvasGrid.GetChild(0);

        GridLayoutGroup groupFrinedly = panelCharacter.GetComponent<GridLayoutGroup>();
        panelCharacter.GetComponent<RectTransform>().sizeDelta = new Vector2(groupFrinedly.cellSize.x * 3 + BattleScenario.gridCorrection, groupFrinedly.cellSize.y * 3 + BattleScenario.gridCorrection);
        EventTrigger trigger = panelCharacter.gameObject.AddComponent<EventTrigger>();

        Entry enterEntry = new();
        trigger.triggers.Add(enterEntry);
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((data) =>
        {
            battleScenario.isInCharacter = true;
        });

        Entry exitEntry = new();
        trigger.triggers.Add(exitEntry);
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) =>
        {
            if (!battleScenario.isDragging) return;
            battleScenario.isInCharacter = false;
        });


        Transform panelEnemy = canvasGrid.GetChild(1);

        eventTrigger = panelCharacter.gameObject.AddComponent<EventTrigger>();
        Entry downEntry = new();
        eventTrigger.triggers.Add(downEntry);
        downEntry.eventID = EventTriggerType.PointerDown;
        // Button 이벤트 추가
        gameObject.AddComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Down");
        });


        for (int i = 0; i < 9; i++)
        {
            GameObject characterObject = Instantiate(prefabGridObject, panelCharacter);
            GridObject characterGrid = characterObject.GetComponent<GridObject>();
            characterGrid.InitObject();
            characterGrid.isEnemy = false;
            characterGrid.index = i;
            BattleScenario.CharacterGrids.Add(characterGrid);
            characterGrid.SetClickEvent().SetDownEvent().SetDragEvent().SetEnterEvent().SetExitEvent().SetUpEvent();

            GameObject enemyObject = Instantiate(prefabGridObject, panelEnemy);
            GridObject enemyGrid = enemyObject.GetComponent<GridObject>();
            enemyGrid.InitObject();
            enemyGrid.isEnemy = true;
            enemyGrid.index = i;
            BattleScenario.EnemyGrids.Add(enemyGrid);
            enemyGrid.SetClickEvent().SetDownEvent().SetDragEvent().SetEnterEvent().SetExitEvent().SetUpEvent();
        }
    }

    public async void LoadGame()
    {
        seed = (int)(long)progressDoc["Seed"];
        Random.InitState(seed);
        scene = (string)progressDoc["Scene"];
        nodeLevel = (int)(long)progressDoc["NodeLevel"];
        gold = GetFloatValue(progressDoc["Gold"]);
        stage = (int)(long)progressDoc["Stage"];
        await LoadCharacter();
        switch (scene)
        {
            case "Map":
                scene = $"Stage {stage}";
                break;
            case "Battle":
                await BattleScenario.LoadEnemyByProgressAsync();
                break;
        }
        invenData = (string)progressDoc["InvenData"];
        SceneManager.LoadScene(scene);
    }
    public void InitProgress()
    {
        //temp
        nodeLevel = 0;
        stage = 0;
        gold = 0f;
        Dictionary<string, object> dict = new();
        InitSeed();
        dict.Add("Seed", seed);
        dict.Add("NodeLevel", nodeLevel);
        dict.Add("Stage", stage);
        dict.Add("Gold", gold);
        dict.Add("Scene", "Map");
        dict.Add("InvenData", string.Empty);
        DataManager.dataManager.SetDocumentData(dict, "Progress", Uid);
    }

    public async Task LoadCharacter(int _battleLevel =-1)
    {
        foreach (var x in BattleScenario.CharacterGrids)
        {
            x.gameObject.SetActive(true);
        }
        List<DocumentSnapshot> characterDocs;
        if (_battleLevel == -1)
            characterDocs = await DataManager.dataManager.GetDocumentSnapshots(string.Format("{0}/{1}/{2}", "Progress", gameManager.uid, "Characters"));
        else
            characterDocs = await DataManager.dataManager.GetDocumentSnapshots($"SimulationCharacterInfo/Simulation_{_battleLevel}/Characters");
        if (characterDocs.Count == 0)
        {
            Debug.LogError("No Characters");
        }
        List<CharacterData> characterDataDict = new();
        foreach (DocumentSnapshot snapShot in characterDocs)
        {
            Dictionary<string, object> tempDict = snapShot.ToDictionary();
            object obj;
            float ability;
            float maxHp;
            float hp;
            float resist;
            float speed;
            string[] skillNames = new string[2];
            string weaponId;

            Sprite hair = null,
            faceHair = null,
            eyesFront = null,
            eyesBack = null,
            head = null,
            armL = null,
            armR = null;
            Color hairColor = Color.black;
            if (tempDict.TryGetValue("Ability", out obj))
            {
                ability = GetFloatValue(obj);
            }
            else
            {
                ability = 0;
            }
            if (tempDict.TryGetValue("MaxHp", out obj))
            {
                maxHp = GetFloatValue(obj);
            }
            else
            {
                maxHp = 10000f;
            }
            if (tempDict.TryGetValue("Hp", out obj))
            {
                hp = GetFloatValue(obj);
            }
            else
            {
                hp = 10000f;
            }
            if (tempDict.TryGetValue("Resist", out obj))
            {
                resist = GetFloatValue(obj);
            }
            else
            {
                resist = 0;
            }
            if (tempDict.TryGetValue("Speed", out obj))
            {
                speed = GetFloatValue(obj);
            }
            else
            {
                speed = 1f;
            }
            for (int i = 0; i < 2; i++)
            {
                if (tempDict.TryGetValue(string.Format("Skill_{0}", i), out obj))
                {
                    skillNames[i] = ((string)obj);
                }
            }
            if (tempDict.TryGetValue("Body", out obj))
            {
                object obj1;
                Species species = Species.Human;
                Dictionary<string, object> bodyDict = obj as Dictionary<string, object>;
                if (bodyDict.TryGetValue("Species", out obj1))
                {
                    switch ((string)obj1)
                    {
                        case "Elf":
                            species = Species.Elf;
                            break;
                        case "Devil":
                            species = Species.Devil;
                            break;
                        case "Skelton":
                            species = Species.Skelton;
                            break;
                        case "Orc":
                            species = Species.Orc;
                            break;
                    }
                }
                if (bodyDict.TryGetValue("Hair", out obj1))
                {
                    string hairStr = (string)obj1;
                    if (hairStr == string.Empty)
                    {
                        hair = null;
                    }
                    else
                        hair = LoadManager.loadManager.hairDict[hairStr];
                }
                if (bodyDict.TryGetValue("FaceHair", out obj1))
                {
                    string faceHairStr = (string)obj1;
                    if (faceHairStr == string.Empty)
                    {
                        faceHair = null;
                    }
                    else
                        faceHair = LoadManager.loadManager.faceHairDict[faceHairStr];
                }
                if (bodyDict.TryGetValue("Eye", out obj1))
                {
                    EyeClass eye = LoadManager.loadManager.EyeDict[species][(string)obj1];
                    eyesFront = eye.front;
                    eyesBack = eye.back;
                }
                if (bodyDict.TryGetValue("Body", out obj1))
                {
                    BodyPartClass bodyPart = LoadManager.loadManager.BodyPartDict[species][(string)obj1];
                    head = bodyPart.head;
                    armL = bodyPart.armL;
                    armR = bodyPart.armR;
                }
                if (bodyDict.TryGetValue("HairColor", out obj1))
                {
                    Dictionary<string, object> colorDict = obj1 as Dictionary<string, object>;
                    float red = GetFloatValue(colorDict["R"]);
                    float green = GetFloatValue(colorDict["G"]);
                    float blue = GetFloatValue(colorDict["B"]);
                    hairColor = new Color(red, green, blue);
                }
            }
            if (tempDict.TryGetValue("WeaponId", out obj))
            {
                weaponId = (string)obj;
                WeaponType weaponType;
                string weaponName;
                string[] splittedStr = weaponId.Split(":::");
                switch (splittedStr[0])
                {
                    default:
                        weaponType = WeaponType.Sword;
                        break;
                    case "Bow":
                        weaponType = WeaponType.Bow;
                        break;
                    case "Magic":
                        weaponType = WeaponType.Magic;
                        break;
                    case "Club":
                        weaponType = WeaponType.Club;
                        break;
                }
                weaponName = splittedStr[1];
                WeaponClass weapon = LoadManager.loadManager.weaponDict[weaponType][weaponName];
                hp += weapon.hp;
                ability += weapon.ability;
                speed += weapon.speed;
                resist += weapon.resist;
            }
            else
            {
                weaponId = string.Empty;
            }
            GridObject _grid = BattleScenario.CharacterGrids[(int)(long)tempDict["Index"]];
            string jobId = GetJobId(skillNames);

            GameObject characterObject = Instantiate(CharacterTemplate);
            //CharacterHierarchy
            CharacterHierarchy characterHierarchy = characterObject.transform.GetChild(0).GetComponent<CharacterHierarchy>();
            characterHierarchy.SetBodySprite(hair, faceHair, eyesFront, eyesBack, head, armL, armR, hairColor);
            //CharacterAtBattle, Applicant는 가질 필요없기 때문에 미리 갖고 있지 않는다.
            CharacterAtBattle characterAtBattle = characterObject.AddComponent<CharacterAtBattle>();
            characterAtBattle.InitCharacter(snapShot.Id, _grid);
            BattleScenario.characters.Add(characterAtBattle);
            //CharacterData, 위와 동일
            CharacterData characterData = characterObject.AddComponent<CharacterData>();
            characterData.InitCharacterData(snapShot.Id, jobId, maxHp, hp, ability, resist, speed, (int)(long)tempDict["Index"], skillNames, weaponId);
            characterDataDict.Add(characterData);
        }
        CharacterManager.characterManager.SetCharacters(characterDataDict);
    }
    public string GetJobId(string[] _skillNames)
    {
        int job = 0;
        int num = 0;
        foreach (var x in _skillNames)
        {
            if (string.IsNullOrEmpty(x))
                continue;
            switch (x.Split("_")[0])
            {
                case "Power":
                    job += 100;
                    num++;
                    break;
                case "Sustain":
                    job += 10;
                    num++;
                    break;
                case "Util":
                    job += 1;
                    num++;
                    break;
            }
        }
        if (num == 2)
        {
            return AddZero(job);
        }
        else
            return "000";
    }

    public static Skill LocalizeSkill(string x1)//Skill_n/n 형태의 x1을 기반으로 LoadManager에 있는 EffectForm을 가진 SkillStruct 접근해서 Effect를 가진 Skill를 리턴
    {
        string skillID;
        byte skillLevel;
        if (x1.Contains(":::"))
        {
            skillID = x1.Split(":::")[0];
            skillLevel = byte.Parse(x1.Split(":::")[1]);
        }
        else
        {
            skillID = x1;
            skillLevel = 0;
        }
        SkillForm tempSkillForm = LoadManager.loadManager.skillsDict[skillID];
        return new Skill(tempSkillForm, skillLevel);
    }


    public GameObject GetEnemyPrefab(string _characterId, bool _isMonster = false)
    {
        GameObject characterObject = Instantiate(Resources.Load<GameObject>(string.Format("Prefab/Enemy/" + _characterId)));
        if (_isMonster)
        {
            Transform objTransform = characterObject.transform;
            Transform body = objTransform.GetChild(0);
            body.localScale = Vector3.one * 50f;
            var sortingGroup = body.gameObject.AddComponent<SortingGroup>();
            sortingGroup.sortingOrder = 0;
            Vector3 curRot = objTransform.eulerAngles;
            curRot.y = 180f;
            objTransform.eulerAngles = curRot;
        }
        else
        {
            characterObject.transform.GetChild(0).localScale = Vector3.one * 17f;

            RectTransform rectTransform = characterObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
            rectTransform.localScale = new Vector2(1, 1);
        }
        return characterObject;
    }
    float GetFloatValue(object _obj)
    {
        if (_obj is long)
            return (int)(long)_obj;
        else
            return (float)(double)_obj;
    }
    private static string AddZero(int _num)
    {
        if (_num < 10)
            return "00" + _num;
        else if (_num < 100)
            return "0" + _num;
        else
            return _num.ToString();
    }
    public static bool CalculateProbability(float _probability)
    {
        return Random.Range(0f, 1f) <= Mathf.Clamp(_probability, 0f, 1f);
    }
    public static int AllocateProbability(params float[] probabilities)
    {
        // 확률의 총합 계산
        float totalProbability = 0;
        foreach (float probability in probabilities)
        {
            totalProbability += probability;
        }

        // 확률의 총합이 1이 아니면 에러 발생
        if (System.Math.Abs(totalProbability - 1f) > 0.00001f)
        {
            throw new System.ArgumentException("확률의 합이 1이 아닙니다.");
        }

        // 0과 1 사이의 랜덤 값을 생성
        float randomValue = Random.Range(0f, 1f);

        // 누적 확률을 사용하여 결과를 결정
        float cumulativeProbability = 0;
        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randomValue < cumulativeProbability)
            {
                return i; // 해당 결과를 반환
            }
        }

        // 여기에 도달하는 경우는 없지만, 컴파일러의 경고를 제거하기 위해 기본적으로 0을 반환
        return 0;
    }
    public async void GameOver()
    {


        await FirebaseFirestore.DefaultInstance.RunTransactionAsync(async transaction =>
        {
            await battleScenario.ClearCharacterAsync();
            await battleScenario.ClearEnemyAsync();
            DocumentReference documentRef = FirebaseFirestore.DefaultInstance.Collection("Progress").Document(Uid);
            await documentRef.DeleteAsync();
        });
        StartCoroutine(GameOverCor());

        IEnumerator GameOverCor()
        {
            foreach (var x in BattleScenario.enemies)
                x.StopBattle();
            yield return new WaitForSeconds(5f);
            canvasGrid.gameObject.SetActive(false);
            scene = null;
            progressDoc = null;
            battleScenario.panelGameOver.gameObject.SetActive(true);
        }
    }
    public void InitSeed()
    {
        seed = (int)System.DateTime.Now.Ticks;
        Random.InitState(seed);
        DataManager.dataManager.SetDocumentData("Seed", seed, "Progress", Uid);
        Debug.Log("Seed : " + seed);
    }
    public async void SetInvenData(string _invenData)
    {
        invenData = _invenData;
        await DataManager.dataManager.SetDocumentData("InvenData", _invenData, string.Format("{0}/{1}", "Progress", Uid));
    }
    public static float GetRandomNumber(float _mean, float _standardDeviation)
    {
        System.Random random = new();
        double u1 = 1.0 - random.NextDouble(); // 난수 생성
        double u2 = 1.0 - random.NextDouble();
        double randStdNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) * System.Math.Sin(2.0 * System.Math.PI * u2); // 정규 분포를 따르는 값 생성
        float randNormal = _mean + _standardDeviation * (float)randStdNormal; // 평균과 표준 편차 적용

        // 평균 값의 최소 50%, 최대 200%로 값을 제한
        randNormal = Mathf.Clamp(randNormal, _mean * 0.5f, _mean * 2f);

        return randNormal;
    }
}