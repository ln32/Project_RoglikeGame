using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFloatingNode : MonoBehaviour
{
    internal CreateNodeValues createNodeValues;
    internal List<NodeSetPerLevel> levelNodeList = new();
    bool isInit = false;

    public bool IsInit()
    {
        return isInit;
    }

    public void SetInit(bool input)
    {
        isInit = input;
    }

    public EventNodeDataToPlace buildStem(int nextChild = -1)
    {
        Node _focusNode = createNodeValues.GetFocusNode();

        // 예외처리
        if(true){
            if (_focusNode.gridPos.x >= levelNodeList.Count)
                return null;

            if (_focusNode.myChilds.Count <= nextChild)
            {
                return null;
            }
        
            if (_focusNode.myChilds.Count != 0)
                return null;
        }

        createNodeValues.buildTree_Stem(_focusNode); 
        EventNodeDataToPlace rtnData = setDataByFocus(_focusNode);
        return rtnData;

        EventNodeDataToPlace setDataByFocus(Node _focusNode)
        {
            EventNodeDataToPlace rtnData = new();
            rtnData.nodeTreeData = _focusNode.connData_focus;
            rtnData.focusGridPos = _focusNode.gridPos;
            rtnData.targetLevel = 1;

            rtnData.nodeEventData = new();
            for (int i = 0; i < levelNodeList[_focusNode.gridPos.x + 1].nodeList.Count; i++)
            {
                rtnData.nodeEventData.Add(levelNodeList[_focusNode.gridPos.x + 1].nodeList[i].myEvent);
            }

            rtnData.nodeTerrainData = new();
            for (int i = 0; i < levelNodeList[_focusNode.gridPos.x + 1].nodeList.Count; i++)
            {
                rtnData.nodeTerrainData.Add(levelNodeList[_focusNode.gridPos.x + 1].nodeList[i].terrainV3);
            }

            return rtnData;
        }
    }

    public EventNodeDataToPlace buildTree(int nextChild = -1)
    {
        Node _focusNode = createNodeValues.GetFocusNode();

        // focus 좌표 이동 ( init 상황은 예외처리 if문 )
        if (nextChild != -1)
        {
            _focusNode = createNodeValues.GetFocusNode().myChilds[nextChild];
            createNodeValues.focusNodeGridPos = _focusNode.gridPos;
        }

        // 만약, 최초의 상황 시 생성
        if (_focusNode.myChilds.Count == 0)
            createNodeValues.buildTree_Stem(_focusNode);

        createNodeValues.buildTree_Leaf(_focusNode);
        EventNodeDataToPlace rtnData = setDataByFocus(_focusNode);

        return rtnData;

        EventNodeDataToPlace setDataByFocus(Node _focusNode)
        {
            EventNodeDataToPlace rtnData = new();
            rtnData.nodeTreeData = _focusNode.connData_focus;
            rtnData.focusGridPos = _focusNode.gridPos;

            rtnData.targetLevel = 2;

            // 이제 다음 자식
            if (_focusNode.gridPos.x + 2 >= levelNodeList.Count)
            {
                rtnData.targetLevel = -1;
            }

            rtnData.nodeEventData = new();
            for (int i = 0; i < checkChangeThenApply(_focusNode.gridPos.x, rtnData.nodeTreeData); i++)
            {
                rtnData.nodeEventData.Add(levelNodeList[_focusNode.gridPos.x + 2].nodeList[i].myEvent);
            }

            rtnData.nodeTerrainData = new();
            for (int i = 0; i < checkChangeThenApply(_focusNode.gridPos.x, rtnData.nodeTreeData); i++)
            {
                rtnData.nodeTerrainData.Add(levelNodeList[_focusNode.gridPos.x + 2].nodeList[i].terrainV3);
            }

            return rtnData;
        }

        int checkChangeThenApply(int gridPosX, List<int> rtnData)
        {
            if (rtnData == null)
                return 0;

            int count = 0;
            for (int i = 0; i < rtnData.Count; i++)
            {
                count += rtnData[i];
            }

            count += (rtnData.Count) - 1;

            return count;
        }
    }

    public Vector2Int getFocusGridPos()
    {
        return createNodeValues.focusNodeGridPos;
    }

    public List<Vector2Int> getNextChilds()
    {
        List<Vector2Int> result = new();

        for (int i = 0; i < createNodeValues.GetFocusNode().myChilds.Count; i++)
        {
            result.Add(createNodeValues.GetFocusNode().myChilds[i].gridPos);
        }

        return result;
    }
}

[Serializable]
internal struct CreateNodeValues
{
    internal int[] eventCoefTable;
    internal int[] childCountTable;
    internal int[] nodesPerLevel;

    internal Vector2Int rangeMinMax;
    internal int maxLevel;

    internal Transform parentTransform;
    internal Vector2Int focusNodeGridPos;
    internal List<NodeSetPerLevel> levelList;

    internal MapSettingValues initValues;
}

[Serializable]
internal struct MapSettingValues
{
    internal Vector3 Value_terrainV3;
    internal int Value_difficulty;
    internal float Value_SetIndex;
}

[Serializable]
internal class NodeSetPerLevel
{
    internal int level;
    internal List<Node> nodeList;
}

[Serializable]
internal class Node
{
    internal Vector2Int gridPos;

    internal Vector3 terrainV3;
    internal int terrainLevel;

    internal List<Node> myChilds;
    internal Node shared_L;
    internal Node shared_R;
    internal int myEvent;
    internal Transform myObject;
    internal List<int> connData_focus = null;

    internal Node(Vector2Int gridPos)
    {
        this.gridPos = gridPos;
        myChilds = new List<Node>();
    }

    internal Node(Vector2Int _gridPos,int _myEvent)
    {
        gridPos = _gridPos;
        myEvent = _myEvent;
        myChilds = new List<Node>();
    }
}