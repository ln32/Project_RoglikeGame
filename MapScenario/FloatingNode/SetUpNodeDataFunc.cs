using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

internal static class SetUpNodeDataFunc
{
    static public EventNodeDataToPlace initTree(this CreateFloatingNode cfn)
    {
        if (cfn.IsInit())
            return null;

        cfn.levelNodeList = cfn.createNodeValues.levelList;
        cfn.createNodeValues.InitNodeSetting(); 
        cfn.SetInit(true);

        EventNodeDataToPlace rtnData = setDataByRoot();
        cfn.createNodeValues.FinishUpNodeSetting();

        return rtnData;

        EventNodeDataToPlace setDataByRoot()
        {
            EventNodeDataToPlace rtnData = new();
            rtnData.nodeTreeData = null;
            rtnData.focusGridPos = Vector2Int.zero;
            rtnData.targetLevel = 0;

            rtnData.nodeEventData = new() { cfn.createNodeValues.levelList[0].nodeList[0].myEvent };
            rtnData.nodeTerrainData = new() { cfn.createNodeValues.levelList[0].nodeList[0].terrainV3 };
            return rtnData;
        }
    }

    internal static void FinishUpNodeSetting(this CreateNodeValues values)
    {
        SetFinishNode(values);
        return;

        static void SetFinishNode(CreateNodeValues values)
        {
            {
                NodeSetPerLevel prev = values.levelList[values.levelList.Count - 1];
                NodeSetPerLevel curr = new(); curr.nodeList = new();
                curr.level = prev.level + 1;
                values.levelList.Add(curr);

                for (int i = 0; i < 2; i++)
                {
                    curr.nodeList.Add(new Node(new Vector2Int(curr.level, i))
                    {
                        terrainV3 = values.getRnDmV3(),
                        myEvent = 0
                    });
                }
            }

            {
                NodeSetPerLevel prev = values.levelList[values.levelList.Count - 1];
                NodeSetPerLevel curr = new(); curr.nodeList = new();
                curr.level = prev.level + 1;
                values.levelList.Add(curr);

                for (int i = 0; i < 1; i++)
                {
                    curr.nodeList.Add(new Node(new Vector2Int(curr.level, i))
                    {
                        terrainV3 = values.getRnDmV3(),
                        myEvent = 0
                    });
                }
            }
        }
    }
}
