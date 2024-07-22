using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_MapNodeInfo : MonoBehaviour
{
    [SerializeField] internal Image img;
    [SerializeField] internal Image bar_1, bar_2, bar_3;

    [SerializeField] internal TextMeshProUGUI textGUI;
    [SerializeField] internal Transform _trans;

    public void SetGUI_toDefault()
    {
        _trans.localScale = Vector3.zero;
    }
    public void SetGUI_toActive()
    {
        ActivatingAction();
    }
    public void SetGUI_byNodeInfo(Image _img)
    {
        img.sprite = _img.sprite;
    }
    public void SetGUI_byImage(Sprite _sprite)
    {
        img.sprite = _sprite;
    }
    public void SetGUI_byVector3(Vector3 _str)
    {
        setColor(bar_1, _str.x);
        setColor(bar_2, _str.y);
        setColor(bar_3, _str.z);

        if (true) 
        {
            Vector3 tempV3 = _str;
            tempV3 *= 10; tempV3 += Vector3.one;
            tempV3 = new Vector3(
                Mathf.Round(Mathf.Log(tempV3.x, 1.3f)), 
                Mathf.Round(Mathf.Log(tempV3.y, 1.3f)), 
                Mathf.Round(Mathf.Log(tempV3.z, 1.3f))
                );
        }

        void setColor(Image targetImg, float value)
        {
            Color _color = targetImg.color;
            _color.a = value;
            targetImg.color = _color;
        }
    }

    public void SetGUI_byName(string _str)
    {
        textGUI.text = _str;
    }

    public void ActivatingAction()
    {
        StartCoroutine(moveBlockTime());

        IEnumerator moveBlockTime()
        {
            float lifeTime = 0.1f;
            float elapsedTime = 0.0f;

            while (true)
            {
                if (elapsedTime > lifeTime) break;

                _trans.localScale = Mathf.Lerp(1.03f, 1f, Mathf.Pow((elapsedTime / lifeTime), 4f)) * Vector3.one;
                elapsedTime += Time.deltaTime;

                yield return null;
            }
            _trans.localScale = Vector3.one;
        }
    }
}
