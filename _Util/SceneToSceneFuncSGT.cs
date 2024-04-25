using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using static GUI_MapScenario;

public class SceneToSceneFuncSGT : MonoBehaviour
{
    public GameObject EventSysyemObj;
    public static SceneToSceneFuncSGT Instance = null;
    public Material _ArriveMaterial_Map, _ExitMaterial_Map;
    public GameObject _ClosedSceneObj_Map, _ArriveObj_Map, _ExitObj_Map;
    public float reachTime = 0;
    public float timer = 0;
    public GameObject myChild_toActive;
    public Camera _myCamera;

    ProgressMap_preInput returnFunc;

    static public void InitSingleton(ref SceneToSceneFuncSGT target)
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(target);
            Instance = target;
        }
        else
        {
            if (Instance == target)
                return;

            GameObject temp = target.gameObject;
            Instance.gameObject.SetActive(true);
            target = Instance;
            Destroy(temp);
        }
    }

    static public SceneToSceneFuncSGT InitSingleton()
    {
        return Instance;
    }

    static public void ArriveScene_Map(ProgressMap_preInput _returnFunc = null)
    {

        if (Instance == null)
        {
            Debug.Log("Tlqkf null"); return;
        }

        if (Instance.gameObject.activeSelf == false)
            Instance.gameObject.SetActive(true);

        Instance._ArriveScene_Map(_returnFunc);
    }

    static public void ExitScene_Map(ProgressMap_preInput _returnFunc = null)
    {
        if (Instance.gameObject.activeSelf == false)
            Instance.gameObject.SetActive(true);

        Instance._ExitScene_Map(_returnFunc);
    }

    void _ArriveScene_Map(ProgressMap_preInput _returnFunc = null)
    {
        if (_returnFunc == null)
        {
            _returnFunc = () => {;};
        }

        returnFunc = _returnFunc;
        
        _myCamera.enabled = (true);
        EventSysyemObj.SetActive(false);

        returnFunc += () => _myCamera.enabled = (false);
        returnFunc += () => EventSysyemObj.SetActive(true);

        StartCoroutine(ArriveSceneCor());

        IEnumerator ArriveSceneCor()
        {
            timer = 0;
            while (true)
            {
                timer += Time.deltaTime;
                if (timer > (reachTime / 2))
                    break;

                yield return new WaitForSeconds(0.02f);
            }

            _ArriveMaterial_Map.SetFloat("_Seed", Random.Range(0.0f, 1.0f));
            _ArriveMaterial_Map.SetFloat("_RevertCoex", 0);
            _ArriveObj_Map.SetActive(true);
            _ClosedSceneObj_Map.SetActive(false);

            timer = 0;
            while (true)
            {
                timer += Time.deltaTime;
                float coef = timer / reachTime;
                _ArriveMaterial_Map.SetFloat("_RevertCoex", Mathf.Pow(coef, 0.7f));
                if (timer > reachTime)
                    break;

                yield return new WaitForSeconds(0.02f);
            }

            timer = 0;
            _ArriveObj_Map.SetActive(false);
            EventSysyemObj.SetActive(true);
            returnFunc();
            returnFunc = null;
            yield return null;
        }
    }

    void _ExitScene_Map(ProgressMap_preInput _returnFunc = null)
    {
        EventSysyemObj.SetActive(false);

        if (_returnFunc == null)
        {
            _returnFunc = () => {; };
        }

        returnFunc = _returnFunc;

        _myCamera.enabled = (true);
        myChild_toActive.SetActive(true);
        //returnFunc += () => _myCamera.enabled = (false);
        returnFunc += () => Instance.gameObject.SetActive(false);
        returnFunc += () => gameObject.SetActive(false);
        StartCoroutine(ExitSceneCor());

        IEnumerator ExitSceneCor()
        {
            timer = 0;

            _ExitMaterial_Map.SetFloat("_Seed", Random.Range(0.0f, 1.0f));
            _ExitMaterial_Map.SetFloat("_RevertCoex", 0);
            _ExitObj_Map.SetActive(true);

            timer = 0;
            while (true)
            {
                timer += Time.deltaTime;
                float coef = timer / reachTime;
                _ExitMaterial_Map.SetFloat("_RevertCoex", Mathf.Pow(coef, 1f));

                if (timer > reachTime)
                    break;

                yield return new WaitForSeconds(0.05f);
            }

            timer = 0;
            returnFunc();//12
            _ExitObj_Map.SetActive(false);
            _ClosedSceneObj_Map.SetActive(true);
            EventSysyemObj.SetActive(false);
            yield return null;
        }
    }


    /*
    static public void ArriveScene_Debug()
    {
        if (Instance == null)
        {
            Debug.Log("Tlqkf null"); return;
        }

        Instance._ArriveScene_Debug();
    }

    static public void ExitScene_Debug()
    {
        if (Instance == null)
        {
            Debug.Log("Tlqkf null"); return;
        }

        Instance._ExitScene_Debug();
    }

    public void _ArriveScene_Debug()
    {
        _ClosedSceneObj_Map.SetActive(false);
        _ArriveObj_Map.SetActive(false);

        EventSysyemObj.SetActive(true);
    }

    public void _ExitScene_Debug()
    {
        _ClosedSceneObj_Map.SetActive(true);

        EventSysyemObj.SetActive(false);
    }*/
}
