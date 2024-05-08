using EnumCollection;
using Firebase.Firestore;
using LobbyCollection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyScenario : MonoBehaviour
{
    public GameObject selectLight;
    public GameObject buttonNext;
    public List<GameObject> phaseList;
    public UpgradeUi pubUi;
    public UpgradeUi guildUi;
    public RecruitUi recruitUi;
    public DepartUi departUi;
    public Image[] mediumImage_0 = new Image[2];
    public Image[] mediumImage_1 = new Image[2];
    private LobbyCase curCase;
    private Dictionary<TMP_Text, Dictionary<Language, string>> texts = new();
    public GameObject layBlock;
    #region Phase_0
    public TMP_Text text_Fame;
    private Dictionary<string, UpgradeClass> upgrade_Pub;
    private Dictionary<string, UpgradeClass> upgrade_Guild;
    public ExplainPanel explainPanel;
    
    #endregion
    #region Phase_1
    public Transform parentApplicant;
    public List<ApplicantSlot> applicantSlots;
    private List<ApplicantSlot> selectedSlots = new();


    public static readonly float defaultHp = 100f;
    public static readonly float defaultAbility = 10f;
    public static readonly float defaultSpeed = 1f;
    public static readonly float defaultResist = 10f;

    public static readonly float hpSd = 10f;
    public static readonly float abilitySd = 1f;
    public static readonly float speedSd = 0.1f;
    public static readonly float resistSd = 1f;

    public TextStatus textStatusHp;
    public TextStatus textStatusAbility;
    public TextStatus textStatusSpeed;
    public TextStatus textStatusResist;
    #endregion
    private void Awake()
    {
        GameManager.lobbyScenario = this;
        #region UiSet
        layBlock.SetActive(false);
        selectLight.SetActive(false);
        phaseList[0].SetActive(true);
        phaseList[1].SetActive(false);
        pubUi.gameObject.SetActive(false);
        guildUi.gameObject.SetActive(false);
        recruitUi.gameObject.SetActive(false);
        departUi.gameObject.SetActive(false);
        #endregion
        text_Fame.text = GameManager.gameManager.fame.ToString();
        curCase = LobbyCase.None;
        upgrade_Pub = LoadManager.loadManager.upgradeDict.Where(item => item.Value.lobbyCase == "Pub").ToDictionary(item => item.Key, item => item.Value);
        upgrade_Guild = LoadManager.loadManager.upgradeDict.Where(item => item.Value.lobbyCase == "Guild").ToDictionary(item => item.Key, item => item.Value);

        SettingManager.LanguageChangeEvent += OnLanguageChange;
        explainPanel.gameObject.SetActive(false);
        InitUpgradeUi(LobbyCase.Pub);
        InitUpgradeUi(LobbyCase.Guild);

    }
    public void InitUpgradeUi(LobbyCase _lobbyCase)
    {
        Dictionary<string, UpgradeClass> curUpgrade;
        UpgradeUi curUi;
        switch (_lobbyCase)
        {
            default:
                curUpgrade = upgrade_Pub;
                curUi = pubUi;
                break;
            case LobbyCase.Guild:
                curUpgrade = upgrade_Guild;
                curUi = guildUi;
                break;
        }
        TMP_Text titleText = curUi.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        Dictionary<Language, string> langDict = new();
        switch (_lobbyCase)
        {
            default:
                langDict.Add(Language.En, "Pub");
                langDict.Add(Language.Ko, "주점");
                break;
            case LobbyCase.Guild:
                langDict.Add(Language.En, "Guild");
                langDict.Add(Language.Ko, "길드");
                break;
        }
        titleText.text = langDict[GameManager.language];
        texts.Add(titleText, langDict);
        for (int i = 0; i < 3; i++)
        {
            if (i < curUpgrade.Count)
            {
                curUi.slots[i].gameObject.SetActive(true);
                KeyValuePair<string, UpgradeClass> upgradeKvp = curUpgrade.Where(item => item.Value.index == i).FirstOrDefault();//index가 i인 클래스
                curUi.slots[i].textName.text = upgradeKvp.Value.name[GameManager.language];
                int level = GameManager.gameManager.upgradeLevelDict[upgradeKvp.Key];
                curUi.slots[i].textLv.text = "Lv. " + level;
                curUi.slots[i].curId = upgradeKvp.Key;
                if (level == upgradeKvp.Value.content.Count)
                {
                    curUi.slots[i].button_Up.SetActive(false);
                }
                else
                {
                    curUi.slots[i].button_Up.SetActive(true);
                }
                Dictionary<Language, string> str_name = new() { { Language.Ko, upgradeKvp.Value.name[Language.Ko] }, { Language.En, upgradeKvp.Value.name[Language.En] } };
                texts.Add(curUi.slots[i].textName, str_name);
            }
            else
            {
                curUi.slots[i].gameObject.SetActive(false);
            }
        }
    }
    public void InitRecruitUi()
    {
        TMP_Text titleText = recruitUi.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        Dictionary<Language, string> langDict = new();
        langDict.Add(Language.En, "Recruit");
        langDict.Add(Language.Ko, "모집");
        titleText.text = langDict[GameManager.language];
        texts.Add(titleText, langDict);
    }
    public void OnPointerClick(LobbyCase _lobbyCase)
    {
        layBlock.SetActive(true);
        Debug.Log(_lobbyCase + " Clicked");
        curCase = _lobbyCase;
        switch (_lobbyCase)
        {
            case LobbyCase.Pub:
                PubCase();
                break;
            case LobbyCase.Guild:
                GuildCase();
                break;
            case LobbyCase.Recruit:
                RecruitCase();
                break;
            case LobbyCase.Depart:
                DepartCase();
                break;
        }
        SetMediumImage(false);
    }
    private void PubCase()
    {
        pubUi.gameObject.SetActive(true);
        explainPanel.gameObject.SetActive(false);
    }

    private void GuildCase()
    {
        guildUi.gameObject.SetActive(true);
        explainPanel.gameObject.SetActive(false);
    }
    private void RecruitCase()
    {
        recruitUi.gameObject.SetActive(true);
    }
    private void DepartCase()
    {
        departUi.gameObject.SetActive(true);
    }
    public void NextPhase()
    {
        SetMediumImage(true);
        phaseList[0].SetActive(false);
        phaseList[1].SetActive(true);
        buttonNext.SetActive(false);
        AllocateApplicant();
    }
    public void SetMediumImage(bool _isActive)
    {
        switch (curCase)
        {
            case LobbyCase.Pub:
            case LobbyCase.Guild:
                foreach (var x in mediumImage_0)
                {
                    x.enabled = _isActive;
                }
                break;
            case LobbyCase.Recruit:
            case LobbyCase.Depart:
                foreach (var x in mediumImage_1)
                {
                    x.enabled = _isActive;
                }
                break;
        }
    }


    public void OnUpBtnClicked(UpgradeSlot _upgradeSlot)
    {
        UpgradeClass upgradeClass = LoadManager.loadManager.upgradeDict[_upgradeSlot.curId];
        int level = GameManager.gameManager.upgradeLevelDict[_upgradeSlot.curId];
        if (level == upgradeClass.content.Count)
        {
            Debug.Log("최고 레벨");
            return;
        }
        int fameResult = GameManager.gameManager.fame - upgradeClass.content[level].price;
        if (fameResult >= 0)//구매 가능하다면
        {
            //클라이언트 Fame 계산
            GameManager.gameManager.fame = fameResult;
            //클라이언트 능력 적용
            float valueBefore;
            if (level == 0)
                valueBefore = 0f;
            else
                valueBefore = upgradeClass.content[level - 1].value;
            float valueAfter = upgradeClass.content[level].value;
            float valueResult = valueAfter - valueBefore;
            if (!GameManager.gameManager.upgradeValueDict.ContainsKey(upgradeClass.type))
                GameManager.gameManager.upgradeValueDict.Add(upgradeClass.type, 0f);
            GameManager.gameManager.upgradeValueDict[upgradeClass.type] += valueResult;
            GameManager.gameManager.upgradeLevelDict[_upgradeSlot.curId]++;
            text_Fame.text = GameManager.gameManager.fame.ToString();
            //Firestore
            DataManager.dataManager.SetDocumentData("Fame", fameResult, "User", GameManager.gameManager.Uid);
            Dictionary<string, object> objDict = DataManager.dataManager.ConvertToObjDictionary(GameManager.gameManager.upgradeLevelDict);
            DataManager.dataManager.SetDocumentData("Upgrade", objDict, "User", GameManager.gameManager.Uid);
        }
        else
        {
            Debug.Log("비용 부족");
            return;
        }
        level++;
        if (level == upgradeClass.content.Count)
        {
            _upgradeSlot.button_Up.SetActive(false);
        }
        _upgradeSlot.textLv.text = "Lv. " + level;
    }
    public void OnPointerEnter_Slot(UpgradeSlot _upgradeSlot)
    {
        explainPanel.gameObject.SetActive(true);
        explainPanel.gameObject.transform.position = _upgradeSlot.transform.position + new Vector3(-0.2f, 0f, 0f);

        UpgradeClass upgradeClass = LoadManager.loadManager.upgradeDict[_upgradeSlot.curId];
        int level = GameManager.gameManager.upgradeLevelDict[_upgradeSlot.curId];
        explainPanel.SetExplain(upgradeClass.explain[GameManager.language]);

        string cur;
        if (level != 0)
        {
            cur = upgradeClass.info[GameManager.language].Replace("{Value}", upgradeClass.content[level - 1].value.ToString());
        }
        else
        {
            cur = string.Empty;
        }
        string next;
        if (level != upgradeClass.content.Count)
        {
            next = upgradeClass.info[GameManager.language].Replace("{Value}", upgradeClass.content[level].value.ToString());
        }
        else
        {
            next = string.Empty;
        }
        explainPanel.SetInfo(cur, next);

        explainPanel.SetSize();
    }
    public void OnPointerExit_Slot()
    {
        explainPanel.gameObject.SetActive(false);
    }
    public void LayBlockClicked()
    {
        layBlock.SetActive(false);
        LobbyUiBase curUi = null;
        switch (curCase)
        {
            case LobbyCase.Pub:
                curUi = pubUi;
                break;
            case LobbyCase.Guild:
                curUi = guildUi;
                break;
            case LobbyCase.Recruit:
                curUi = recruitUi;
                break;
            case LobbyCase.Depart:
                curUi = departUi;
                break;
        }
        curUi.ExitBtnClicked();
    }

    private void OnLanguageChange(Language _language)
    {
        foreach (KeyValuePair<TMP_Text, Dictionary<Language, string>> keyValue in texts)
        {
            keyValue.Key.text = keyValue.Value[_language];
        }
    }
    public void AllocateApplicant()
    {
        float upValue;
        if (GameManager.gameManager.upgradeValueDict.ContainsKey(UpgradeEffectType.AllocateNumberUp))
            upValue = GameManager.gameManager.upgradeValueDict[UpgradeEffectType.AllocateNumberUp];
        else
            upValue = 0f;
        for (int i = 0; i < 6; i++)
        {
            if (i < 3 + upValue)
            {
                applicantSlots[i].gameObject.SetActive(true);
                applicantSlots[i].InitApplicantSlot();
            }
            else
            {
                applicantSlots[i].gameObject.SetActive(false);
            }
        }
    }
    public void SetStatusText(float _hp, float _ability, float _speed, float _resist)
    {
        textStatusHp.textValue.text = _hp.ToString("F0");
        textStatusAbility.textValue.text = _ability.ToString("F0");
        textStatusSpeed.textValue.text = _speed.ToString("F1");
        textStatusResist.textValue.text = _resist.ToString("F0");
    }
    public void InitStatusText()
    {
        textStatusHp.textValue.text =
        textStatusAbility.textValue.text =
        textStatusSpeed.textValue.text =
        textStatusResist.textValue.text =
        "-";
    }
    public void InactiveEnterBtns()
    {
        foreach (ApplicantSlot slot in applicantSlots)
        {
            if (slot.gameObject.activeSelf)
                slot.IsActived = false;
        }
    }
    public bool AddSelectedSlot(ApplicantSlot _slot)
    {
        if (selectedSlots.Count < 3)
        {
            selectedSlots.Add(_slot);
            return true;
        }
        else
        {
            return false;                                                                                                    
        }
    }
    public void RemoveSelectedSlot(ApplicantSlot _slot) => selectedSlots.Remove(_slot);
    public async void DepartAsync()
    {
        if(selectedSlots.Count < 3)
        {
            return;
        }
        await FirebaseFirestore.DefaultInstance.RunTransactionAsync(async transaction =>
        {
            await FromSlotToCharacter();
            GameManager.gameManager.InitProgress();
        });

        SceneManager.LoadScene("Stage 0");
        
    }
    async Task FromSlotToCharacter()
    {
        List<CharacterData> characterDataList = new();
        for (int i = 0; i < selectedSlots.Count; i++)
        {
            ApplicantSlot _slot = applicantSlots[i];
            _slot.templateAnimator.speed = 1f;
            int gridIndex = i + 3;
            Dictionary<string, object> characterDict = new();

            characterDict.Add("MaxHp", _slot.Hp);
            characterDict.Add("Hp", _slot.Hp);
            characterDict.Add("Ability", _slot.Ability);
            characterDict.Add("Resist", _slot.Resist);
            characterDict.Add("Speed", _slot.Speed);
            characterDict.Add("Body", _slot.bodyDict);
            string weaponTypeStr = GameManager.CalculateProbability(0.5f) ? "Sword" : "Club";
            string weaponId = $"{weaponTypeStr}:::Default";
            characterDict.Add("WeaponId", weaponId);
            characterDict.Add("Index", gridIndex);


            string docId = await DataManager.dataManager.SetDocumentData(characterDict,$"Progress/{GameManager.gameManager.Uid}/Characters");

            CharacterAtBattle characterAtBattle = _slot.templateObject.AddComponent<CharacterAtBattle>();
            characterAtBattle.InitCharacter(docId, BattleScenario.CharacterGrids[gridIndex]);
            BattleScenario.characters.Add(characterAtBattle);

            CharacterData characterData = _slot.templateObject.AddComponent<CharacterData>();
            characterData.InitCharacterData(docId, "000", _slot.Hp, _slot.Hp, _slot.Ability, _slot.Resist, _slot.Speed, gridIndex, new string[] {string.Empty, string.Empty }, weaponId);
            characterDataList.Add(characterData);
        }
        CharacterManager.characterManager.SetCharacters(characterDataList);
    }
}
