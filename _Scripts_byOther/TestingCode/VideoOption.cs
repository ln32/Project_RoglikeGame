using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class VideoOption : MonoBehaviour
{
    List<Resolution> resolutions = new List<Resolution>();
    public TMP_Dropdown dropDown_Resolution;
    public Toggle toggle_FullScreen;
    [SerializeField] int resolutionNum;
    public GameObject panel_Setting;
    FullScreenMode screenMode;
    private void Start()
    {
        Debug.Log("width = " + Screen.width + ", Height = " + Screen.height + ", ScreenMode = " + Screen.fullScreenMode);
        InitUI();
        panel_Setting.SetActive(true);
    }
    void InitUI()
    {
        resolutions.AddRange(Screen.resolutions);
        dropDown_Resolution.options.Clear();

        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "x" + item.height;
            dropDown_Resolution.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
                dropDown_Resolution.value = optionNum;
            optionNum++;
        } 
        dropDown_Resolution.RefreshShownValue();
        toggle_FullScreen.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
        
    }
    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }
    public void FullScreenToggle(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
    public void ConfirmBtnClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
}