using System.Collections;
using System.Collections.Generic;
using System.Threading;
//using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _AsycLoadScene : MonoBehaviour
{
    public string nextSceneName = "NextScene";
    public AsyncOperation asyncLoad; public float output;

    public void LoadScene_Asyc(string target)
    {
        if (asyncLoad != null)
        {
            return;
        }

        // 비동기적으로 Scene을 불러오기 위해 Coroutine을 사용한다.
        StartCoroutine(LoadMyAsyncScene());

        IEnumerator LoadMyAsyncScene()
        {
            nextSceneName = target;

            asyncLoad = SceneManager.LoadSceneAsync(target);
            asyncLoad.allowSceneActivation = false;

            while (!(asyncLoad.isDone))
            {
                yield return new WaitForSeconds(0.02f);
            }

            yield return null;
        }
    }

    public void TimeToSwitchScene()
    {
        if (asyncLoad != null)
        {
            asyncLoad.allowSceneActivation = true;
        }
        else
            Debug.Log("sooo sad");
    }

    public bool IsLoadedScene()
    {
        return asyncLoad != null;
    }
}
