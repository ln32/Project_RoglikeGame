using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncSet_Camp : MonoBehaviour
{
    [SerializeField]internal CanvasGroup cg;
    public float temp = 0.5f;
    public float coefPow = 0.5f;
    private void Start()
    {
        StartCoroutine(StartEffect());

        IEnumerator StartEffect()
        {
            float timer = 0;

            while (true)
            {
                timer += Time.deltaTime;
   
                if (timer > temp)
                    break;

                cg.alpha = Mathf.Pow(timer / temp, coefPow);
                yield return new WaitForSeconds(0.02f);
            }

            yield return null;
        }
    }

    // Start is called before the first frame update
    public void SetArrive()
    {
        cg.interactable = true;
        cg.alpha = 1.0f;

    }

    public void SetExit()
    {
        StartCoroutine(StartEffect());

        IEnumerator StartEffect()
        {
            float timer = 0;

            while (true)
            {
                timer += Time.deltaTime;

                if (timer > temp)
                    break;

                cg.alpha = 1 - (timer / temp)/0.5f;
                yield return new WaitForSeconds(0.02f);
            }

            yield return null;
        }
    }
}
