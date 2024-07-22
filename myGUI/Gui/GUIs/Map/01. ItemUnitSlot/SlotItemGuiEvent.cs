using UnityEngine;

public class SlotItemGuiEvent : MonoBehaviour
{
    protected GameObject OnSet;
    protected GameObject OnDepose;

    public void SetGui_byIsNull(bool isNull)
    {
        if (isNull)
        {
            SetGui_byIsNull_OnDepose();
        }
        else
        {
            SetGui_byIsNull_OnSet();
        }
    }

    void SetGui_byIsNull_OnSet()
    {
        OnSet.gameObject.SetActive(true);
        OnDepose.gameObject.SetActive(false);
    }

    void SetGui_byIsNull_OnDepose()
    {
        OnSet.gameObject.SetActive(false);
        OnDepose.gameObject.SetActive(true);
    }
}
