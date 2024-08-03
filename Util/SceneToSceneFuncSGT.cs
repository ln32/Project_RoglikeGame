using System;
using System.Collections;
using UnityEngine;

public class SceneToSceneFuncSGT : MonoBehaviour
{
    private static SceneToSceneFuncSGT Instance = null;
    [SerializeField] internal GameObject EventSysyemObj;

    [SerializeField] protected GameObject _ClosedSceneObj_Map, _ArriveObj_Map, _ExitObj_Map;
    [SerializeField] protected GameObject myChild_toActive;

    [SerializeField] protected Material _ArriveMaterial_Map, _ExitMaterial_Map;
    [SerializeField] protected Camera _myCamera;

    [SerializeField] protected float reachTime = 0;
    [SerializeField] protected float timer = 0;

    Action returnFunc;

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

    static public SceneToSceneFuncSGT GetInstance()
    {
        return Instance;
    }
    static public void ArriveScene_Map(Action _returnFunc = null)
    {
        if (Instance == null)
        {
            return;
        }

        if (Instance.gameObject.activeSelf == false)
            Instance.gameObject.SetActive(true);

        Instance._ArriveScene_Map(_returnFunc);
    }

    static public void ExitScene_Map(Action _returnFunc = null)
    {
        if (Instance.gameObject.activeSelf == false)
            Instance.gameObject.SetActive(true);

        Instance._ExitScene_Map(_returnFunc);
    }

    void _ArriveScene_Map(Action _returnFunc = null)
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

            _ArriveMaterial_Map.SetFloat("_Seed", UnityEngine.Random.Range(0.0f, 1.0f));
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

    void _ExitScene_Map(Action _returnFunc = null)
    {
        EventSysyemObj.SetActive(false);

        if (_returnFunc == null)
        {
            _returnFunc = () => {; };
        }

        returnFunc = _returnFunc;

        _myCamera.enabled = (true);
        myChild_toActive.SetActive(true);
        returnFunc += () => _myCamera.enabled = (false);
        returnFunc += () => Instance.gameObject.SetActive(false);
        returnFunc += () => gameObject.SetActive(false);
        StartCoroutine(ExitSceneCor());

        IEnumerator ExitSceneCor()
        {
            timer = 0;

            _ExitMaterial_Map.SetFloat("_Seed", UnityEngine.Random.Range(0.0f, 1.0f));
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
            returnFunc();
            _ExitObj_Map.SetActive(false);
            _ClosedSceneObj_Map.SetActive(true);
            EventSysyemObj.SetActive(false);
            yield return null;
        }
    }
}