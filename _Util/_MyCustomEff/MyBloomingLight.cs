using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyBloomingLight : MonoBehaviour
{
    [SerializeField] internal Transform img_BloomingTrans;
    [SerializeField] internal Image img_Blooming;
    public Vector2 MmVector2d_Total;
    public Vector2 MmVector2d_SubBloom;

    public void SetLight(float _Level, Color _color)
    {
        float newLevel = UnityEngine.Random.Range(0.9f, 1.1f) * _Level;
        img_Blooming.color = _color;
        img_BloomingTrans.localScale = Vector3.one * Mathf.Lerp(MmVector2d_SubBloom.x, MmVector2d_SubBloom.y, newLevel / 5.0f);
        transform.localScale = Vector3.one * Mathf.Lerp(MmVector2d_Total.x, MmVector2d_Total.y, newLevel / 5.0f);
    }

    public int ads;
    public Color sadC;
    [ContextMenu("sad")]
    public void SetLight_DEBUG()
    {
        SetLight(ads, sadC);
    }
}
