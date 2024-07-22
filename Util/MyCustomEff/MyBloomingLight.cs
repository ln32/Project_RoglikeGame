using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyBloomingLight : MonoBehaviour
{
    [SerializeField] internal Transform img_BloomingTrans;
    [SerializeField] internal Image img_Blooming;
    [SerializeField] internal Vector2 MmVector2d_Total;
    [SerializeField] internal Vector2 MmVector2d_SubBloom;

    public void SetLight(float _Level, Color _color)
    {
        float newLevel = UnityEngine.Random.Range(0.9f, 1.1f) * _Level;
        img_Blooming.color = _color;
        img_BloomingTrans.localScale = Vector3.one * Mathf.Lerp(MmVector2d_SubBloom.x, MmVector2d_SubBloom.y, newLevel / 5.0f);
        transform.localScale = Vector3.one * Mathf.Lerp(MmVector2d_Total.x, MmVector2d_Total.y, newLevel / 5.0f);
    }
}
