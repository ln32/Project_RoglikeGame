using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _InsFunc_CookEffect : MonoBehaviour
{
    public List<Transform> transforms;
    public List<GameObject> myInsSlots;
    public GameObject onImg, offImg;
    public int myIndex = 0;

    [ContextMenu("insFuc")]
    public void insFuc()
    {
        for (int i = 0; i < myInsSlots.Count; i++)
        {
            DestroyImmediate(myInsSlots[i]);
        }
        myInsSlots.Clear();

        for (int i = 0; i < transforms.Count; i++)
        {
            if(myIndex == i)
                myInsSlots.Add(Instantiate(onImg,transforms[i]));
            else
                myInsSlots.Add(Instantiate(offImg, transforms[i]));
        }
    }
}
