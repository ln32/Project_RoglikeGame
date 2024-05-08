using EnumCollection;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public delegate void LanguageChange(Language language);
    public static event LanguageChange LanguageChangeEvent;

    public static SettingManager settingManager;
    private SettingClass settingClass;
    private TMP_Dropdown dropdownResolution;
    private TMP_Dropdown dropdownLanguage;
    public Toggle toggleFullScreen;
    public GameObject panelSetting;
    public GameObject buttonSetting;
    public Transform parentSlider;
    private Dictionary<VolumeType, VolumeSlider> volumeSliders = new();
    public List<VolumeType> volumeTypes;
    public AudioMixer masterMixer;
    private Dictionary<TMP_Text, Dictionary<Language, string>> texts;
    TMP_Text
        textResolution,
        textFullScreen,
        textConvenience,
        textQuickBattle,
        textVolume,
        textAll,
        textSfx,
        textBgm,
        textLanguage,
        textConfirm,
        textCancel;
    private void Awake()
    {
        if (!settingManager)
        {
            settingClass = new SettingClass();
            settingManager = this;
            DontDestroyOnLoad(GameObject.FindWithTag("CANVASSETTING"));
            volumeTypes = new List<VolumeType>((VolumeType[])Enum.GetValues(typeof(VolumeType)));
            panelSetting.SetActive(false);
            LanguageChangeEvent += OnLanguageChange;
            //UI초기화
            Transform panelLeft = panelSetting.transform.GetChild(0).GetChild(0);
            Transform panelRight = panelSetting.transform.GetChild(0).GetChild(1);
            textResolution = panelLeft.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            textFullScreen = panelLeft.GetChild(0).GetChild(2).GetChild(1).GetComponent<TMP_Text>();
            textConvenience = panelLeft.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
            textQuickBattle = panelLeft.GetChild(1).GetChild(1).GetChild(1).GetComponent<TMP_Text>();
            textVolume = panelRight.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            textAll = panelRight.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<TMP_Text>();
            textSfx = panelRight.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetComponent<TMP_Text>();
            textBgm = panelRight.GetChild(0).GetChild(1).GetChild(2).GetChild(1).GetComponent<TMP_Text>();
            textLanguage = panelRight.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
            textCancel = panelSetting.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>();
            textConfirm = panelSetting.transform.GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Text>();
            dropdownResolution = panelLeft.GetChild(0).GetChild(1).GetComponent<TMP_Dropdown>();
            dropdownLanguage = panelRight.GetChild(1).GetChild(1).GetComponent<TMP_Dropdown>();
            volumeSliders.Add(VolumeType.All, parentSlider.GetChild(0).GetComponent<VolumeSlider>());
            volumeSliders.Add(VolumeType.Sfx, parentSlider.GetChild(1).GetComponent<VolumeSlider>());
            volumeSliders.Add(VolumeType.Bgm, parentSlider.GetChild(2).GetComponent<VolumeSlider>());
            //텍스트 초기화
            texts =
                        new()
                        {
                            {
                                textResolution,
                                new()
                                {
                                    { Language.Ko, "해상도" },
                                    { Language.En, "Start" }
                                }
                            },
                            {
                                textFullScreen,
                                new()
                                {
                                    { Language.Ko, "전체 화면" },
                                    { Language.En, "Full Screen" }
                                }
                            },
                            {
                                textConvenience,
                                new()
                                {
                                    { Language.Ko, "편의 기능" },
                                    { Language.En, "Convenience" }
                                }
                            },
                            {
                                textQuickBattle,
                                new()
                                {
                                    { Language.Ko, "빠른 전투" },
                                    { Language.En, "Quick Battle" }
                                }
                            },
                            {
                                textVolume,
                                new()
                                {
                                    { Language.Ko, "음향" },
                                    { Language.En, "Volume" }
                                }
                            },
                            {
                                textAll,
                                new()
                                {
                                    { Language.Ko, "전체" },
                                    { Language.En, "ALL" }
                                }
                            },
                            {
                                textSfx,
                                new()
                                {
                                    { Language.Ko, "효과음" },
                                    { Language.En, "SFX" }
                                }
                            },
                            {
                                textBgm,
                                new()
                                {
                                    { Language.Ko, "배경음" },
                                    { Language.En, "BGM" }
                                }
                            },
                            {
                                textLanguage,
                                new()
                                {
                                    { Language.Ko, "언어" },
                                    { Language.En, "Language" }
                                }
                            },
                            {
                                textConfirm,
                                new()
                                {
                                    { Language.Ko, "확인" },
                                    { Language.En, "Confirm" }
                                }
                            },
                            {
                                textCancel,
                                new()
                                {
                                    { Language.Ko, "취소" },
                                    { Language.En, "Cancel" }
                                }
                            }
                        };


        }
    }
    private void OnLanguageChange(Language _language)
    {
        foreach (KeyValuePair<TMP_Text, Dictionary<Language, string>> keyValue in texts)
        {
            keyValue.Key.text = keyValue.Value[_language];
        }
    }
    private void Start()
    {
        settingClass.InitSettingClass();
    }
    public void ResolutionOptionChange(int x)
    {
        settingClass.ResolutionOptionChange(x);
    }
    public void LanguageOptionChange(int x)
    {
        settingClass.LanguageOptionChange((Language)x);
    }
    public void FullScreenToggle(bool isFull)
    {
        settingClass.FullScreenToggle(isFull);
    }
    public void ConfirmBtnClick()
    {
        settingClass.ConfirmBtnClick();
    }
    public void CancelBtnClick()
    {
        settingClass.CancelBtnClick();
    }
    public void SettingBtnClick()
    {
        settingClass.SettingBtnClick();
    }
    public void VolumeControl_Str(string _volumeStr)
    {
        switch (_volumeStr)
        {
            default:
                VolumeControl(VolumeType.All);
                break;
            case "Sfx":
                VolumeControl(VolumeType.Sfx);
                break;
            case "Bgm":
                VolumeControl(VolumeType.Bgm);
                break;

        }
    }
    public void InitVolumeSliders()
    {
        foreach (var x in volumeTypes)
            volumeSliders[x].VolumeControl();
    }
    public void VolumeControl(VolumeType _volumeType)
    {
        float volume = settingManager.volumeSliders[_volumeType].slider.value;
        settingClass.newSet.volume[_volumeType] = volume;
        if (!settingClass.newSet.onOff[_volumeType])
            return;
        string str;
        switch (_volumeType)
        {
            default:
                str = "Master";
                break;
            case VolumeType.Sfx:
                str = "SFX";
                break;
            case VolumeType.Bgm:
                str = "BGM";
                break;
        }
        if (volume == -30f)
            volume = -80f;
        masterMixer.SetFloat(str, volume);
    }
    public void OnOffBtnClicked(VolumeType _volumeType, bool _onOff)
    {
        settingClass.newSet.onOff[_volumeType] = _onOff;
        string str;
        switch (_volumeType)
        {
            default:
                str = "Master";
                break;
            case VolumeType.Sfx:
                str = "SFX";
                break;
            case VolumeType.Bgm:
                str = "BGM";
                break;
        }
        if (_onOff)
        {
            if (settingClass.newSet.volume[_volumeType] == -30f)
                masterMixer.SetFloat(str, -80f);
            else
                masterMixer.SetFloat(str, settingClass.newSet.volume[_volumeType]);
        }
        else
            masterMixer.SetFloat(str, -80f);
    }
    public void ExecuteLangaugeChange(Language _language) => LanguageChangeEvent(_language);
    internal class SettingClass
    {
        List<Resolution> resolutions = new();
        internal SettingSet originSet;
        internal SettingSet newSet;
        int originResolution;
        int newResolution;
        FullScreenMode originScreenMode;
        internal void InitSettingClass()
        {
            InitData();
            InitScreen();
            InitSound();
            InitLanguage();
        }
        private void InitData()
        {
            originSet = new SettingSet();
            
            #region Volume
            foreach (VolumeType type in settingManager.volumeTypes)
            {
                string str_Vol;
                switch (type)
                {
                    default:
                        str_Vol = "AllVolume";
                        break;
                    case VolumeType.Sfx:
                        str_Vol = "SfxVolume";
                        break;
                    case VolumeType.Bgm:
                        str_Vol = "BgmVolume";
                        break;
                }
                try
                {
                    originSet.volume[type] = float.Parse(DataManager.dataManager.GetConfigData(DataSection.SoundSetting, str_Vol));
                    
                }
                catch
                {
                    originSet.volume[type] = 15f;
                }
                string str_OnOff;
                switch (type)
                {
                    default:
                        str_OnOff = "AllOnOff";
                        break;
                    case VolumeType.Sfx:
                        str_OnOff = "SfxOnOff";
                        break;
                    case VolumeType.Bgm:
                        str_OnOff = "BgmOnOff";
                        break;
                }
                try
                {
                    originSet.onOff[type] = bool.Parse(DataManager.dataManager.GetConfigData(DataSection.SoundSetting, str_OnOff));
                }
                catch
                {
                    originSet.onOff[type] = true;
                }
            }
            #endregion
            try
            {
                string language = DataManager.dataManager.GetConfigData(DataSection.Language, "Language");
                switch (language)
                {
                    case "En":
                        originSet.language = Language.En;
                        break;
                    default:
                        originSet.language = Language.Ko;
                        break;
                }
            }
            catch
            {
                originSet.language = Language.Ko;
            }
            newSet = new(originSet);
        }

        private void InitScreen()
        {
            int curWidth = 0;
            int curHeight = 0;
            foreach (Resolution item in Screen.resolutions)
            {
                if (curWidth != item.width && curHeight != item.height)
                {
                    curWidth = item.width;
                    curHeight = item.height;
                    resolutions.Add(item);
                }
            }
            settingManager.dropdownResolution.options.Clear();

            int temp = 0;
            foreach (Resolution item in resolutions)
            {
                {
                    //옵션 세팅
                    TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
                    option.text = item.width + "x" + item.height;
                    settingManager.dropdownResolution.options.Add(option);
                    //로컬 값이 없다면 기본 화면 해상도를 선택

                    if (item.width == Screen.width && item.height == Screen.height)
                    {
                        settingManager.dropdownResolution.value = originResolution = temp;
                    }
                    temp++;
                }
            }
            settingManager.dropdownResolution.RefreshShownValue();
            settingManager.toggleFullScreen.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow);
            originScreenMode = Screen.fullScreenMode;
        }
        private void InitSound()
        {
            foreach (VolumeType type in settingManager.volumeTypes)
            {
                settingManager.volumeSliders[type].slider.value = originSet.volume[type];
                settingManager.volumeSliders[type].OnOff = originSet.onOff[type];
                settingManager.OnOffBtnClicked(type, originSet.onOff[type]);
            }
        }
        private void InitLanguage()
        {
            settingManager.ExecuteLangaugeChange(originSet.language);
            settingManager.dropdownLanguage.value = (int)originSet.language;
        }
        internal void ResolutionOptionChange(int _x)
        {
            Screen.SetResolution(resolutions[_x].width, resolutions[_x].height, originScreenMode);
            newResolution = _x;
        }
        internal void LanguageOptionChange(Language _language)
        {
            GameManager.language = _language;
            settingManager.ExecuteLangaugeChange(_language);
            newSet.language = _language;
        }
        internal void FullScreenToggle(bool _isFull)
        {
            Screen.SetResolution(Screen.width, Screen.height, _isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
        }
        internal void ConfirmBtnClick()
        {
            DataManager.dataManager.SetConfigData(DataSection.SoundSetting, "AllVolume", newSet.volume[VolumeType.All]);
            DataManager.dataManager.SetConfigData(DataSection.SoundSetting, "SfxVolume", newSet.volume[VolumeType.Sfx]);
            DataManager.dataManager.SetConfigData(DataSection.SoundSetting, "BgmVolume", newSet.volume[VolumeType.Bgm]);

            DataManager.dataManager.SetConfigData(DataSection.SoundSetting, "AllOnOff", newSet.onOff[VolumeType.All]);
            DataManager.dataManager.SetConfigData(DataSection.SoundSetting, "SfxOnOff", newSet.onOff[VolumeType.Sfx]);
            DataManager.dataManager.SetConfigData(DataSection.SoundSetting, "BgmOnOff", newSet.onOff[VolumeType.Bgm]);

            DataManager.dataManager.SetConfigData(DataSection.Language, "Language", newSet.language);

            settingManager.panelSetting.SetActive(false);
            originResolution = newResolution;
            originSet = newSet;
            originScreenMode = Screen.fullScreenMode;
        }
        internal void CancelBtnClick()
        {
            Screen.SetResolution(resolutions[originResolution].width, resolutions[originResolution].height, originScreenMode);
            settingManager.panelSetting.SetActive(false);
            settingManager.dropdownResolution.value = originResolution;
            settingManager.toggleFullScreen.isOn = originScreenMode == FullScreenMode.FullScreenWindow;

            settingManager.dropdownLanguage.value = (int)originSet.language;
            newSet = new(originSet);
            foreach (VolumeType type in settingManager.volumeTypes)
            {
                settingManager.volumeSliders[type].slider.value = originSet.volume[type];
                settingManager.VolumeControl(type);
                settingManager.volumeSliders[type].OnOff = originSet.onOff[type];
            }
            
        }

        internal void SettingBtnClick()
        {
            if (settingManager.panelSetting.activeSelf)
            {
                newSet = new(originSet);
            }
            settingManager.panelSetting.SetActive(!settingManager.panelSetting.activeSelf);
        }
    }
}



public class SettingSet
{
    public Language language;
    public Dictionary<VolumeType, float> volume = new();
    public Dictionary<VolumeType, bool> onOff = new();
    public SettingSet()
    {
    }
    public SettingSet(SettingSet _origin)
    {
        foreach (VolumeType type in SettingManager.settingManager.volumeTypes)
        {
            volume[type] = _origin.volume[type];
            onOff[type] = _origin.onOff[type];
        }
        language = _origin.language;
    }
}