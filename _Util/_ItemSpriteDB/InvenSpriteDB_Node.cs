using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSpriteDB_Node : MonoBehaviour
{
    public string _name;
    [SerializeField] internal List<InvenSpriteDB_Node> myChild;
    [SerializeField] internal List<Sprite> myData;

    public bool isLeaf()
    {
        return (myChild != null) && (myChild.Count == 0);
    }
}