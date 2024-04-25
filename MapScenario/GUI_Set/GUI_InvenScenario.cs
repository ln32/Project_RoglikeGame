using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GUI_MapScenario;

public class GUI_InvenScenario : MonoBehaviour
{
    public GUI_SelectedNode _selectGUI;
    public Camera CameraComp;
    public Transform CameraTrans;

    public float reachTime;
    public float timer;

    public Vector3 stdCamPosV3;
    public Material _ArriveMaterial, _ExitMaterial;

    public GameObject ClosedScene;
    public GameObject _ArriveMapScearioEffect;
    public Image myBlur;

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
            Debug.Log("tq 2");
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