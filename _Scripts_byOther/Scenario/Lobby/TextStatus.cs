using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextStatus : MonoBehaviour
{
    public TMP_Text textStatus;
    public TMP_Text textValue;
    private void Awake()
    {
        textStatus = GetComponent<TMP_Text>();
        textValue = transform.GetChild(0).GetComponent<TMP_Text>();
        textValue.text = "-";
    }
}
