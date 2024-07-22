using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    [SerializeField] internal SpriteRenderer EventNode, myPoint;
    [SerializeField] internal Sprite sillhouette, detail,point;
    [SerializeField] internal List<Sprite> myDetailData;
    [SerializeField] internal Color Active,TurnOff;
    [SerializeField] internal List<GameObject> objList_Silhouette;
    [SerializeField] internal List<GameObject> objList_MyRoad;

    [SerializeField] internal ParticleSystem _ps1;
    [SerializeField] internal ParticleSystem _ps2;
    [SerializeField] internal Color _color;

    public void SetSprite()
    {
        EventNode.sprite = detail;
        myPoint.sprite = sillhouette;
        transform.name = "Silhouette";
        for (int i = 0; i < objList_Silhouette.Count; i++)
        {
            objList_Silhouette[i].SetActive(false);
        }
    }

    public List<GameObject> objList_Detailed;

    public void ActiveFocsed()
    {
        var temp = _ps1.main;
        temp.startColor = _color;
        transform.name = "Focused";
    }

    public void ActiveDetailed()
    {
        myPoint.sprite = point;
        transform.name = "Detail";

        EventNode.sprite = detail;
        for (int i = 0; i < objList_Silhouette.Count; i++)
        {
            objList_Silhouette[i].SetActive(false);
        }

        for (int i = 0; i < objList_Detailed.Count; i++)
        {
            objList_Detailed[i].SetActive(true);
        }

        if (_ps1 != null)
        {
            _ps1.Stop(true);
            _ps1.Clear(true);

            uint temp = (uint)Random.Range(0, 1000);
            _ps1.randomSeed = temp;
            _ps2.randomSeed = temp;
            _ps1.Play();
        }
    }

    public void TurnOffFunc()
    {
        transform.name = "Disabled";
        EventNode.color = TurnOff;

        for (int i = 0; i < objList_Detailed.Count; i++)
        {
            objList_Detailed[i].SetActive(false);
        }

        for (int i = 0; i < objList_MyRoad.Count; i++)
        {
            objList_MyRoad[i].SetActive(false);
        }

        for (int i = 0; i < objList_Silhouette.Count; i++)
        {
            objList_Silhouette[i].SetActive(true);
        }
    }
}
