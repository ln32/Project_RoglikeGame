using System;
using System.Collections;
using UnityEngine;

public class GUI_MapScenario : MonoBehaviour
{
    [SerializeField] internal GUI_MapNodeInfo myGUI_Info;
    [SerializeField] internal GUI_SelectedNode _selectGUI;
    [SerializeField] internal Camera CameraComp;
    [SerializeField] internal Transform CameraTrans;

    [SerializeField] internal float reachTime;
    [SerializeField] internal float coef_spd;
    [SerializeField] internal float timer;

    [SerializeField] internal Vector3 stdCamPosV3;
    [SerializeField] internal Material _ArriveMaterial, _ExitMaterial;

    [SerializeField] internal Coroutine cameraMoveCoroutine;

    public void SetInitState()
    {
        _selectGUI.OnLoadedInit();
        timer = 0;
    }

    public void MoveCamera(Vector3 dstV3, Action progFunc)
    {
        if (dstV3 == Vector3.zero)
        {
            dstV3 = new Vector3(0, 5, -10);
        }
        else
        {
            dstV3.z = dstV3.y;
            dstV3 += stdCamPosV3;
        }

        if (cameraMoveCoroutine != null)
        {
            StopCoroutine(cameraMoveCoroutine);
        }

        cameraMoveCoroutine = StartCoroutine(MoveCameraCoroutine(dstV3, progFunc));
    }

    private IEnumerator MoveCameraCoroutine(Vector3 dstV3, Action func)
    {
        _ExitMaterial.SetFloat("_RevertCoex", 0);
        _ExitMaterial.SetFloat("_Seed", UnityEngine.Random.Range(0.0f, 1.0f));

        Vector3 src = CameraTrans.position;

        timer = 0;
        while (timer <= reachTime)
        {
            timer += Time.deltaTime;

            coef_spd = Mathf.Sin((timer / reachTime) * (Mathf.PI / 2));
            CameraTrans.position = Vector3.Lerp(src, dstV3, coef_spd);
            _ExitMaterial.SetFloat("_RevertCoex", timer / reachTime);

            yield return null;
        }

        func?.Invoke();
    }
}
