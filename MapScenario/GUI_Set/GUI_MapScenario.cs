using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GUI_MapScenario : MonoBehaviour
{
    public delegate void ProgressMap_preInput();
    public GUI_MapNodeInfo myGUI_Info;
    public GUI_SelectedNode _selectGUI;
    public Camera CameraComp;
    public Transform CameraTrans;

    public float reachTime;
    public float coef_spd;
    public float timer;

    public Vector3 stdCamPosV3;
    public Material _ArriveMaterial, _ExitMaterial;

    public void SetInitState()
    {
        _selectGUI.OnLoadedInit();
        timer = 0;
    }

    public void moveCamFunc(Vector3 dstV3, ProgressMap_preInput progFunc)
    {
        if (dstV3 != Vector3.zero)
        {
            dstV3.z = dstV3.y;
            dstV3 += stdCamPosV3;

            if (enumerator != null)
                StopCoroutine(enumerator);

            StartCoroutine(moveCam(dstV3, progFunc));
        }else
        {
            dstV3 = new Vector3(0, 5, -10);

            if (enumerator != null)
                StopCoroutine(enumerator);

            StartCoroutine(moveCam(dstV3, progFunc));
        }

    }

    IEnumerator moveCam(Vector3 dstV3, ProgressMap_preInput func)
    {
        _ExitMaterial.SetFloat("_RevertCoex", 0);
        _ExitMaterial.SetFloat("_Seed", Random.Range(0.0f, 1.0f));

        Vector3 src = CameraTrans.position;

        timer = 0;
        while (true)
        {
            timer += Time.deltaTime;

            coef_spd = Mathf.Sin((timer / reachTime) * (Mathf.PI / 2));
            CameraTrans.position = Vector3.Lerp(src, dstV3, Mathf.Sin((timer / reachTime) * (Mathf.PI / 2)));
            _ExitMaterial.SetFloat("_RevertCoex", timer / reachTime);

            if (timer > reachTime)
                break;

            yield return null;
        }

        if(func!=null)
            func();

        yield return null;
    }

    IEnumerator enumerator;
}