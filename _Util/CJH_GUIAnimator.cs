using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CJH_GUIAnimator : MonoBehaviour
{
    public RectTransform rect;
    public Vector3 pos_Dst;
    public List<Vector3> myGraph;
    public float timeCoef;
    [ContextMenu("Debug_SetDstPos")]
    public void Debug_SetDstPos()
    {
        pos_Dst = rect.anchoredPosition;
    }

    [ContextMenu("Debug_GoDst")]
    public void Debug_GoDst()
    {
        AnimateTool temp = new(this);
        temp.dstPos = pos_Dst;
        StartCoroutine(temp.Aminating());
    }
}

public class AnimateTool
{
    public RectTransform rect;
    public Vector3 srcPos, dstPos;
    public List<Vector3> myGraph;
    public Vector2 posCoefV2;
    public float powCoef;
    public float timeCoef;

    internal AnimateTool(CJH_GUIAnimator _ui)
    {
        myGraph = _ui.myGraph;
        if (myGraph[myGraph.Count - 1] != Vector3.one)
            myGraph.Add(Vector3.one);

        timeCoef = _ui.timeCoef;
        rect = _ui.rect;
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