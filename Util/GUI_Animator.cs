using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Animator : MonoBehaviour
{
    [SerializeField] internal RectTransform rect;
    [SerializeField] internal Vector3 pos_Dst;
    [SerializeField] internal List<Vector3> myGraph;
    [SerializeField] internal float timeCoef;


    public void SetDstPos()
    {
        pos_Dst = rect.anchoredPosition;
    }

    public void GoDst()
    {
        AnimateTool temp = new(this);
        StartCoroutine(temp.Aminating());
    }
}

public class AnimateTool
{
    [SerializeField] protected RectTransform rect;
    [SerializeField] protected Vector3 srcPos, dstPos;
    [SerializeField] protected List<Vector3> myGraph;
    [SerializeField] protected Vector2 posCoefV2;
    [SerializeField] protected float powCoef;
    [SerializeField] protected float timeCoef;

    internal AnimateTool(GUI_Animator _ui)
    {
        myGraph = _ui.myGraph;
        if (myGraph[myGraph.Count - 1] != Vector3.one)
            myGraph.Add(Vector3.one);

        timeCoef = _ui.timeCoef;
        rect = _ui.rect;
        dstPos = _ui.pos_Dst;
    }

    internal IEnumerator Aminating()
    {
        srcPos = rect.anchoredPosition;
        float timer = 0;
        int currIndex = 0;
        Vector3 prevData = Vector3.zero;

        while (true)
        {
            float timeRatio = 0.02f;
            if (true)
            {
                if (timer > timeCoef)
                {
                    break;
                }

                timeRatio = timer / timeCoef;
            }

            float convPosRatio = 0;
            if (true)
            {
                if (timeRatio > myGraph[currIndex].x)
                {
                    while (true)
                    {
                        if (timeRatio < myGraph[currIndex].x)
                            break;

                        prevData = myGraph[currIndex];
                        currIndex++;
                    }
                }
                Vector3 currData = myGraph[currIndex];

                float currSrc = prevData.y;
                float currDst = currData.y;

                float currWholeTime = currData.x - prevData.x;
                float convTimeRatio;
                if (currWholeTime != 0)
                    convTimeRatio = (timeRatio - prevData.x) / currWholeTime;
                else
                    convTimeRatio = 0;

                convPosRatio = Mathf.Lerp(currSrc, currDst, convTimeRatio);
                if(convPosRatio != 0)
                    convPosRatio = Mathf.Pow(Mathf.Abs(convPosRatio), prevData.z) * Mathf.Sign(convPosRatio);
            }

            rect.anchoredPosition = Vector3.LerpUnclamped(srcPos, dstPos, convPosRatio);

            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        rect.anchoredPosition = dstPos;
    }
}