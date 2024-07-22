using System;
using Unity.VisualScripting;
using UnityEngine;

public class TouchableNode : MonoBehaviour
{
    [SerializeField] internal BoxCollider2D coll;
    [SerializeField] internal int index;
    Action<int> onClick;


    public void Setting(int _index, Action<int> _onClick)
    {
        coll = transform.AddComponent<BoxCollider2D>();
        coll.size *= 2.5f;
        onClick = _onClick;
        index = _index;
    }

    private void OnMouseUp()
    {
        onClick(index);
    }

    public void Destroy()
    {
        Destroy(coll);
        Destroy(this);
    }
}

