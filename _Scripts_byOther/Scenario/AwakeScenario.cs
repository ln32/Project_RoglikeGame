using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AwakeScenario : MonoBehaviour
{
    public Image logoImage;
    private readonly float fadeInTime = 1f;
    private readonly float fadeOutTime = 1f;
    private readonly float totalPlayTime = 5f;
    private readonly float waitTime = 1f;
    private void Awake()
    {
        Color imageColor = logoImage.color;
        imageColor.a = 0f;
        logoImage.color = imageColor;
        StartCoroutine(ShowJhLogo());
    }

    private void Start()
    {
        SettingManager.settingManager.buttonSetting.SetActive(false);
        SceneManager.LoadScene("Start");
    }

    private IEnumerator ShowJhLogo()
    {
        Sprite sprite = Resources.Load<Sprite>("Image/JhLogo");
        if (sprite != null)
        {
            logoImage.sprite = sprite;
            yield return new WaitForSeconds(waitTime); // 추가: FadeIn 이전에 waitTime 동안 대기
            yield return StartCoroutine(FadeInJhLogo());
            yield return new WaitForSeconds(totalPlayTime - fadeInTime - fadeOutTime);
            yield return StartCoroutine(FadeOutJhLogo());
            SceneManager.LoadScene("Start");
        }
        else
        {
            Debug.LogError("Load logo fail");
        }
    }

    IEnumerator FadeInJhLogo()
    {
        Color imageColor = logoImage.color;
        imageColor.a = 0f;
        logoImage.color = imageColor;

        while (logoImage.color.a < 1f)
        {
            imageColor.a += Time.deltaTime / fadeInTime;
            logoImage.color = imageColor;

            yield return null;
        }
    }

    IEnumerator FadeOutJhLogo()
    {
        Color imageColor = logoImage.color;

        while (logoImage.color.a > 0f)
        {
            imageColor.a -= Time.deltaTime / fadeOutTime;
            logoImage.color = imageColor;

            yield return null;
        }
    }
}
