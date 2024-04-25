using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateFloatingNode : MonoBehaviour
{
    public CreateNodeValues createNodeValues;
    public List<NodeSetPerLevel> levelNodeList = new();
    public bool DEBUG_buildObject;
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

        // 혹시 모를 예외처리
        {
            if (_focusNode.gridPos.x >= levelNodeList.Count)
                return null;

            if (_focusNode.myChilds.Count <= nextChild)
            {
                Debug.Log("build Tree index Error - " + _focusNode.gridPos +
                    "  - child count = " + _focusNode.myChilds.Count +
                    " , inputted index = " + nextChild);
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

        // 중복 생성이 예상됨
        if (_focusNode.myChilds.Count <= nextChild)
        {
            Debug.Log("build Tree index Error - " + _focusNode.gridPos +
                "  - child count = " + _focusNode.myChilds.Count +
                " , inputted index = " + nextChild);
            //return null;
        }

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

            // 이제 다음 자식 정보 전달 생각
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

    public Node getFocusNode()
    {
        return levelNodeList[createNodeValues.focusNodeGridPos.x].nodeList[createNodeValues.focusNodeGridPos.y];
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
public struct CreateNodeValues
{
    public int[] eventCoefTable;
    public int[] childCountTable;
    public int[] nodesPerLevel;

    public Vector2Int rangeMinMax;
    public int maxLevel;

    public Transform parentTransform;
    public Vector2Int focusNodeGridPos;
    public List<NodeSetPerLevel> levelList;

    public MapSettingValues initValues;
}

[Serializable]
public struct MapSettingValues
{
    public Vector3 INIT_terrainV3;
    public int INIT_difficulty;
    public float sadvalues;
}

[Serializable]
public class NodeSetPerLevel
{
    public int level;
    public List<Node> nodeList;
}

[Serializable]
public class Node
{
    public Vector2Int gridPos;

    public Vector3 terrainV3;    
    public int terrainLevel;

    public List<Node> myChilds;
    public Node shared_L;
    public Node shared_R;
    public int myEvent;
    public Transform myObject;
    public List<int> connData_focus = null;

    public Node(Vector2Int gridPos)
    {
        this.gridPos = gridPos;
        myChilds = new List<Node>();
    }

    public Node(Vector2Int _gridPos,int _myEvent)
    {
        gridPos = _gridPos;
        myEvent = _myEvent;
        myChilds = new List<Node>();
    }
}