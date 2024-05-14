using EnumCollection;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScenario : MonoBehaviour
{
    public Transform canvasStart;
    private Transform panelMainMenu;
    private Transform panelDifficultySelect;
    private Dictionary<TMP_Text, Dictionary<Language, string>> texts;
    TMP_Text
        textNewGame,
        textLoadGame,
        textSetting,
        textExit,
        textEasy,
        textNormal,
        textHard;

    private void Awake()
    {
        GameManager.startScenario = this;
        panelMainMenu = canvasStart.GetChild(1);
        panelDifficultySelect = canvasStart.GetChild(2);

        textNewGame = panelMainMenu.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        textLoadGame = panelMainMenu.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        textSetting = panelMainMenu.GetChild(2).GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        textExit = panelMainMenu.GetChild(3).GetChild(1).GetChild(0).GetComponent<TMP_Text>();


        textEasy = panelDifficultySelect.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        textNormal = panelDifficultySelect.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        textHard = panelDifficultySelect.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<TMP_Text>();

        panelDifficultySelect.gameObject.SetActive(false);
        texts =
                new()
                {
                    {
                        textNewGame,
                        new()
                        {
                            { Language.Ko, "새로하기" },
                            { Language.En, "New Game" }
                        }
                    },{
                        textLoadGame,
                        new()
                        {
                            { Language.Ko, "이어하기" },
                            { Language.En, "Load Game" }
                        }
                    },{
                        textSetting,
                        new()
                        {
                            { Language.Ko, "환경설정" },
                            { Language.En, "Setting" }
                        }
                    },{
                        textExit,
                        new()
                        {
                            { Language.Ko, "게임 종료" },
                            { Language.En, "Exit" }
                        }
                    },
                    {
                        textEasy,
                        new()
                        {
                            { Language.Ko, "쉬움" },
                            { Language.En, "Easy" }
                        }
                    },
                    {
                        textNormal,
                        new()
                        {
                            { Language.Ko, "보통" },
                            { Language.En, "Normal" }
                        }
                    },
                    {
                        textHard,
                        new()
                        {
                            { Language.Ko, "어려움" },
                            { Language.En, "Hard" }
                        }
                    },
                };
        SettingManager.LanguageChangeEvent += LanguageChange;
        LanguageChange(GameManager.language);
        SettingManager.settingManager.buttonSetting.SetActive(true);
        SettingManager.settingManager.InitVolumeSliders();
    }
    private void Start()
    {
        if (GameManager.gameManager.progressDoc != null)
        {
            ActiveLoadBtn(true);
        }
    }
    public void NewGameBtnClick()
    {
        panelDifficultySelect.gameObject.SetActive(true);
    }
    public async void DifficultySelect(int i)
    {
        //New Game
        DocumentReference docRef = DataManager.dataManager.GetDocumentReference($"Progress/{GameManager.gameManager.Uid}");
        Dictionary<string, object> newFieldDict = new();
        newFieldDict.Add("NodeLevel", 0);
        newFieldDict.Add("Stage", 0);
        await docRef.DeleteAsync();
        List<DocumentSnapshot> snapshots = await DataManager.dataManager.GetDocumentSnapshots($"Progress/{GameManager.gameManager.Uid}/Characters");
        foreach (DocumentSnapshot snapshot in snapshots)
        {
           await snapshot.Reference.DeleteAsync();
        }
        snapshots = await DataManager.dataManager.GetDocumentSnapshots($"Progress/{GameManager.gameManager.Uid}/Enemies");
        foreach (DocumentSnapshot snapshot in snapshots)
        {
            await snapshot.Reference.DeleteAsync();
        }
        GameManager.gameManager.difficulty = (Difficulty)i;
        SceneManager.LoadScene("Lobby");
    }
    public void CloseDifficulty()
    {
        panelDifficultySelect.gameObject.SetActive(false);
    }
    public void ExitBtnClick()
    {
        Application.Quit();
    }
    public void LoadBtnClick()
    {
        GameManager.gameManager.LoadGame();
    }
    private void LanguageChange(Language _language)
    {
        foreach (KeyValuePair<TMP_Text, Dictionary<Language, string>> keyValue in texts)
        {
            keyValue.Key.text = keyValue.Value[_language];
        }
    }
    public void ActiveLoadBtn(bool _isActive) => panelMainMenu.GetChild(1).GetComponent<Button>().enabled = _isActive;
    public void GotoBattleSimulation() => SceneManager.LoadScene("BattleSimulation");
}
