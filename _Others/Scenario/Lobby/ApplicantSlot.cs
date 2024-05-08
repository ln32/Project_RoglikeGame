using CharacterCollection;
using EnumCollection;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ApplicantSlot : MonoBehaviour
{
    public float Hp { get; private set; }
    public float Ability { get; private set; }
    public float Speed { get; private set; }
    public float Resist { get; private set; }
    public GameObject objectSelect;
    public SpriteRenderer rendererCheck;
    private TMP_Text textSelect;
    private bool isActived;
    private bool isSelected;
    public Animator templateAnimator;
    private readonly Color BlackHair = new Color(45f / 255f, 45f / 255f, 45f / 255f);
    private readonly Color BrownHair = new Color(80f / 255f, 45f / 255f, 0f);
    private readonly Color BlueHair = new Color(28f / 255f, 58f / 255f, 180f / 255f);
    private readonly Color RedHair = new Color(204f / 255f, 18f / 255f, 23f / 255f);
    private readonly Color YellowHair = new Color(1f, 1f, 0f);
    private readonly Color WhiteHair = new Color(1f, 1f, 1f);
    private readonly Color GreenHair = new Color(0f, 190f / 255f, 0f);
    public GameObject templateObject;

    public Dictionary<string, object> bodyDict { get; private set; } = new();
    private void Awake()
    {
        rendererCheck.enabled = false;
        IsActived = false;
        isSelected = false;
        textSelect = objectSelect.transform.GetChild(0).GetComponent<TMP_Text>();
    }
    public bool IsActived {
        get {
            return isActived;
        }
        set {
            isActived = value;
            objectSelect.SetActive(isActived);
            templateAnimator.speed = isActived ? 1f : 0f;
        }
    }
    public void InitApplicantSlot()
    {
        InitStatusInRange();
        InitCharacterTemplate();
    }
    private void InitStatusInRange()
    {
        isActived = false;
        objectSelect.SetActive(isActived);
        Hp = GetStatus(LobbyScenario.defaultHp, LobbyScenario.hpSd);
        Ability = GetStatus(LobbyScenario.defaultAbility, LobbyScenario.abilitySd);
        Speed = GetStatus(LobbyScenario.defaultSpeed, LobbyScenario.speedSd);
        Resist = GetStatus(LobbyScenario.defaultResist, LobbyScenario.resistSd);
    }
    private void InitCharacterTemplate()
    {
        templateObject = Instantiate(GameManager.gameManager.CharacterTemplate, transform);
        templateObject.transform.localPosition = new Vector3(0f, -0.5f, 0f);
        templateObject.transform.localScale = Vector3.one * 1.3f;
        templateAnimator = templateObject.transform.GetChild(0).GetComponent<Animator>();
        templateAnimator.speed = 0f;
        CharacterHierarchy characterHierarchy = templateObject.transform.GetChild(0).GetComponent<CharacterHierarchy>();
        characterHierarchy.shadow.SetActive(false);
        InitTemplateSprite(characterHierarchy);

        void InitTemplateSprite(CharacterHierarchy characterHierarchy)
        {
            Sprite hair;
            Sprite faceHair;
            Sprite head;
            Sprite eyeFront;
            Sprite eyeBack;
            Sprite armL;
            Sprite armR;
            Color hairColor;
            hair = faceHair =  null;
            hairColor = Color.black;
            //Species
            Species species;
            //Human, Elf, Devil, Skelton, Orc
            switch (GameManager.AllocateProbability(0.65f,0.1f, 0.05f, 0.1f, 0.1f ))
            {
                default:
                    species = Species.Human;
                    bodyDict.Add("Species", "Human");
                    break;
                case 1:
                    species = Species.Elf;
                    bodyDict.Add("Species", "Elf");
                    break;
                case 2:
                    species = Species.Devil;
                    bodyDict.Add("Species", "Devil");
                    break;
                case 3:
                    species = Species.Skelton;
                    bodyDict.Add("Species", "Skelton");
                    break;
                case 4:
                    species = Species.Orc;
                    bodyDict.Add("Species", "Orc");
                    break;
            }
            //Eye
            List<KeyValuePair<string, EyeClass>> eyeKvps = new(LoadManager.loadManager.EyeDict[species]);
            int eyeNum = Random.Range(0, eyeKvps.Count);
            KeyValuePair<string, EyeClass> eyeKvp = eyeKvps[eyeNum];
            eyeFront = eyeKvp.Value.front;
            eyeBack = eyeKvp.Value.back;
            bodyDict.Add("Eye", eyeKvp.Key);
            //BodyPart
            List<KeyValuePair<string, BodyPartClass>> bodyPartValues = new(LoadManager.loadManager.BodyPartDict[species]);
            int bodyPartNum = Random.Range(0, bodyPartValues.Count);
            KeyValuePair<string, BodyPartClass> bodyPartKvp = bodyPartValues[bodyPartNum];
            head = bodyPartKvp.Value.head;
            armL = bodyPartKvp.Value.armL;
            armR = bodyPartKvp.Value.armR;
            bodyDict.Add("Body", bodyPartKvp.Key);
            if (species == Species.Human || species == Species.Elf)
            {
                //Hair
                List<KeyValuePair<string, Sprite>> hairKvps = new(LoadManager.loadManager.hairDict);
                int hairNum = Random.Range(0, hairKvps.Count + 1);
                if (hairNum != hairKvps.Count)
                {
                    hair = hairKvps[hairNum].Value;
                    bodyDict.Add("Hair", hairKvps[hairNum].Key);
                }
                else
                {
                    hair = null;
                    bodyDict.Add("Hair", string.Empty);
                }
                //FaceHair
                if (GameManager.CalculateProbability(0.3f))
                {
                    List<KeyValuePair<string, Sprite>> faceHairKvps = new(LoadManager.loadManager.faceHairDict);
                    int faceHairNum = Random.Range(0, faceHairKvps.Count);
                    faceHair = faceHairKvps[faceHairNum].Value;
                    bodyDict.Add("FaceHair", faceHairKvps[faceHairNum].Key);
                }
                else
                {
                    faceHair = null;
                    bodyDict.Add("FaceHair", string.Empty);
                }
                
                //Haircolor
                switch (GameManager.AllocateProbability(0.5f, 0.2f, 0.05f, 0.05f, 0.05f, 0.1f, 0.05f))
                {
                    default:
                        hairColor = BlackHair;
                        break;
                    case 1:
                        hairColor = BrownHair;
                        break;
                    case 2:
                        hairColor = BlueHair;
                        break;
                    case 3:
                        hairColor = RedHair;
                        break;
                    case 4:
                        hairColor = YellowHair;
                        break;
                    case 5:
                        hairColor = WhiteHair;
                        break;
                    case 6:
                        hairColor = GreenHair;
                        break;

                }
                Dictionary<string, float> colorDict = new();
                colorDict.Add("R", hairColor.r);
                colorDict.Add("G", hairColor.g);
                colorDict.Add("B", hairColor.b);
                bodyDict.Add("HairColor", colorDict);
            }
            characterHierarchy.SetBodySprite(hair, faceHair, eyeFront, eyeBack, head, armL, armR, hairColor);
        }
    }
    public void SlotBtnClicked()
    {
        switch (isActived)
        {
            case true://활성화 돼있었다면 비활성화
                GameManager.lobbyScenario.InitStatusText();
                IsActived = false;
                break;
            case false://비활성화 돼있었다면 활성화
                GameManager.lobbyScenario.SetStatusText(Hp, Ability, Speed, Resist);
                GameManager.lobbyScenario.InactiveEnterBtns();
                IsActived = true;
                break;
        }

    }
    public void SelectBtnClicked()
    {
        GameManager.lobbyScenario.InitStatusText();
        IsActived = false;
        if (!isSelected)
        {
            if (GameManager.lobbyScenario.AddSelectedSlot(this))//성공 여부
            {
                isSelected = true;
                textSelect.text = "해제";
            }
        }
        else
        {
            GameManager.lobbyScenario.RemoveSelectedSlot(this);
            isSelected = false;
            textSelect.text = "선택";
        }
        rendererCheck.enabled = isSelected;
    }
    private float GetStatus(float _defaultStatus, float _standardDeviation)
    {
        float returnValue = GameManager.GetRandomNumber(_defaultStatus, _standardDeviation);
        if (GameManager.gameManager.upgradeValueDict.TryGetValue(UpgradeEffectType.StatusUp, out float statusUp))
        {
            returnValue *= 1f + statusUp;
        }
        return returnValue;
    }

    public void FronApplicantToCharacter()
    {
    
    }
}
