using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EnumCollection;
public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private Dictionary<string, AudioClip> bgmList;
    [SerializeField] private Dictionary<string, AudioClip> sfxList;
    readonly string bgmPath = "Sound/Bgm";
    readonly string sfxPath = "Sound/Sfx";
    private void Awake()
    {
        if (!soundManager)
        {
            soundManager = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            bgmSource = GetComponents<AudioSource>()[0];
            sfxSource = GetComponents<AudioSource>()[1];
            bgmList = new ();
            foreach (AudioClip x in Resources.LoadAll<AudioClip>(bgmPath))
            {
                bgmList.Add(x.name, x);
            }
            sfxList = new ();
            foreach (AudioClip x in Resources.LoadAll<AudioClip>(sfxPath))
            {
                sfxList.Add(x.name, x);
            }
        }
    }

    public void OnOffSound(VolumeType eVolume, bool _onOff)
    {

    }
    public void ToggleAudioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (bgmList.TryGetValue(arg0.name, out AudioClip audioClip))
        {
            bgmSource.clip = audioClip;
            bgmSource.Play();
        }
    }
    public void SfxPlay(string _name)
    {
        sfxSource.PlayOneShot(sfxList[_name]);
    }

}