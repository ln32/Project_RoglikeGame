using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static GUI_MapScenario;

public class GUI_InvenScenario : MonoBehaviour
{
    [SerializeField] internal GUI_SelectedNode _selectGUI;
    [SerializeField] internal Camera CameraComp;
    [SerializeField] internal Transform CameraTrans;

    [SerializeField] internal Vector3 stdCamPosV3;
    [SerializeField] internal Material _ArriveMaterial, _ExitMaterial;

    [SerializeField] internal GameObject ClosedScene;
    [SerializeField] internal GameObject _ArriveMapScearioEffect;
    [SerializeField] internal Image myBlur;

    internal float reachTime = 1;
    internal float timer = 0;

    public void SetInitState()
    {
        StartCoroutine(enterSceneCor());
        _selectGUI.OnLoadedInit();
        timer = 0;
    }

    public void EffectScenechange_BlurMapFunc(ProgressMap_preInput progFunc)
    {
        if (enumerator != null)
            StopCoroutine(enumerator);

        StartCoroutine(EffectScenechange_BlurMap(progFunc));
    }

    IEnumerator EffectScenechange_BlurMap(ProgressMap_preInput func)
    {
        timer = 0;
        while (true)
        {
            timer += Time.deltaTime;

            if (timer > 0.5f)
                break;

            Color temp = new Color(myBlur.color.r, myBlur.color.g, myBlur.color.b, 0);
            temp.a = (timer / 0.5f) * 0.5f;
            myBlur.color = temp;

            yield return new WaitForSeconds(0.1f);
        }

        Color temp2 = new Color(myBlur.color.r, myBlur.color.g, myBlur.color.b, 0);
        temp2.a = 0.5f;
        myBlur.color = temp2;

        func();
        yield return null;
    }

    IEnumerator enumerator; 

    IEnumerator enterSceneCor()
    {
        timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
                break;

            yield return null;
        }

        _ArriveMaterial.SetFloat("_Seed", Random.Range(0.0f, 1.0f));
        _ArriveMaterial.SetFloat("_RevertCoex", 0);
        _ArriveMapScearioEffect.SetActive(true);
        ClosedScene.SetActive(false);

        timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            _ArriveMaterial.SetFloat("_RevertCoex", timer / reachTime);
            if (timer > reachTime)
                break;

            yield return null;
        }

        timer = 0;
        _ArriveMapScearioEffect.SetActive(false);
        yield return null;
    }
}