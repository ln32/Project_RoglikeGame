using UnityEngine;

internal static class _FuncSet_SetUpNodeData
{
    static internal EventNodeDataToPlace initTree(this CreateFloatingNode cfn)
    {
        if (cfn.IsInit())
            return null;

        // Initialize node values
        cfn.levelNodeList = cfn.createNodeValues.levelList;
        cfn.createNodeValues.InitNodeSetting();
        cfn.SetInit(true);

        EventNodeDataToPlace rtnData = CreateInitialNodeData();
        cfn.createNodeValues.FinishUpNodeSetting();

        return rtnData;

        // Local function to create initial node data
        EventNodeDataToPlace CreateInitialNodeData()
        {
            EventNodeDataToPlace rtnData = new EventNodeDataToPlace
            {
                nodeTreeData = null,
                focusGridPos = Vector2Int.zero,
                targetLevel = 0,
                nodeEventData = new() { cfn.createNodeValues.levelList[0].nodeList[0].myEvent },
                nodeTerrainData = new() { cfn.createNodeValues.levelList[0].nodeList[0].terrainV3 }
            };

            return rtnData;
        }
    }

    internal static void FinishUpNodeSetting(this CreateNodeValues values)
    {
        AddFinishNodes(values);

        // Local function to add finishing nodes
        static void AddFinishNodes(CreateNodeValues values)
        {
            // Add first set of finishing nodes
            NodeSetPerLevel prev = values.levelList[^1];
            NodeSetPerLevel firstNewLevel = new()
            {
                nodeList = new(),
                level = prev.level + 1
            };
            values.levelList.Add(firstNewLevel);

            for (int i = 0; i < 2; i++)
            {
                firstNewLevel.nodeList.Add(new Node(new Vector2Int(firstNewLevel.level, i))
                {
                    terrainV3 = values.getRnDmV3(),
                    myEvent = 0
                });
            }

            // Add second set of finishing nodes
            NodeSetPerLevel secondNewLevel = new()
            {
                nodeList = new(),
                level = prev.level + 1
            };
            values.levelList.Add(secondNewLevel);

            for (int i = 0; i < 1; i++)
            {
                secondNewLevel.nodeList.Add(new Node(new Vector2Int(secondNewLevel.level, i))
                {
                    terrainV3 = values.getRnDmV3(),
                    myEvent = 0
                });
            }
        }
    }
}
