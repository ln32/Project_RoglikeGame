using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

internal static class CreateStageScenarioTools
{
    internal static void setInitData(this CreateStageScenario createSC)
    {
        EventNodeDataToPlace temp = createSC.createFloatingNode.initTree();
        if (temp == null)
        {
            return;
        }

        createSC.createBackGroundSector.InitSettingEventPos_Root(temp);
    }

    internal static void setVisualObject(this CreateStageScenario createSC)
    {
        //DEBUG
        createSC.focusingNode = createSC.createBackGroundSector.GetFocusTransform();
        createSC.focusingGridPos = createSC.createFloatingNode.getFocusGridPos();

        Transform focusingNode = createSC.createBackGroundSector.GetFocusTransform();

        createSC.MapCamera.transform.position += new Vector3(
                focusingNode.position.x, focusingNode.position.y, focusingNode.position.y);
    }

    internal static void settingNextDestination(this List<TouchableNode> currSelectable, CreateMapVisual createBackGroundSector, CreateFloatingNode createNodeSector, Action<int> onClick)
    {
        if (currSelectable.Count == 0)
            createBackGroundSector.eventObjectList[0].nodeList[0].ActiveFocsed();

        for (int i = 0; i < currSelectable.Count; i++)
        {
            Transform targetTrans = currSelectable[i].transform;
            if(targetTrans != createBackGroundSector.GetFocusTransform())
                targetTrans.GetComponent<NodeController>().TurnOffFunc();
            else
                targetTrans.GetComponent<NodeController>().ActiveFocsed();

            currSelectable[i].Destroy();
        }

        if (currSelectable.Count > 0)
            currSelectable.Clear();

        List<Vector2Int> temp = createNodeSector.getNextChilds();
        for (int i = 0; i < temp.Count; i++)
        {
            Transform tempTrans = createBackGroundSector.GetNodeTransformByGrid(temp[i]);
            currSelectable.Add(tempTrans.AddComponent<TouchableNode>());
            currSelectable[i].Setting(i, onClick);
        }
    }

    internal static void settingNextDestination(this List<TouchableNode> currSelectable, CreateMapVisual createBackGroundSector, CreateFloatingNode createNodeSector,int input)
    {
        for (int i = 0; i < currSelectable.Count; i++)
        {
            if (i != input)
                currSelectable[i].transform.GetComponent<NodeController>().TurnOffFunc();

            currSelectable[i].Destroy();
        }

        if (currSelectable.Count > 0)
            currSelectable.Clear();

        List<Vector2Int> temp = createNodeSector.getNextChilds();

        for (int i = 0; i < temp.Count; i++)
        {
            Transform tempTrans = createBackGroundSector.GetNodeTransformByGrid(temp[i]);
            currSelectable.Add(tempTrans.AddComponent<TouchableNode>());
        }
    }



    internal static void buildStem_byInit(this CreateStageScenario _cs)
    {
        EventNodeDataToPlace treeData = _cs.createFloatingNode.buildStem();
        _cs.createBackGroundSector.InitSettingEventPos(treeData, -1);
        _cs.createBackGroundSector.FillEnv(true);
    }

    internal static void buildLeaf_byInit(this CreateStageScenario _cs,int input = -1, bool isBoss = false)
    {
        EventNodeDataToPlace treeData = _cs.createFloatingNode.buildTree(input);
        _cs.createBackGroundSector.InitSettingEventPos(treeData, input);
        _cs.createBackGroundSector.FillEnv();
    }

    internal static void buildLeaf_byHistory(this CreateStageScenario _cs,int input, bool isBoss)
    {
        EventNodeDataToPlace treeData = _cs.createFloatingNode.buildTree(input);
        if (isBoss)
        {
            _cs.createBackGroundSector.InitSettingEventPos_BOSS_byHistory(treeData, input);
            _cs.createBackGroundSector.FillEnv();
            return;
        }
        else
            _cs.createBackGroundSector.InitSettingEventPos(treeData, input);

        _cs.createBackGroundSector.FillEnv();
    }

    // 시간차 생성
    internal static void buildLeaf(this CreateStageScenario _cs, ref GUI_MapScenario.ProgressMap_preInput task, int input = -1)
    {
        EventNodeDataToPlace treeData = _cs.createFloatingNode.buildTree(input);
        if (_cs.createFloatingNode.createNodeValues.maxLevel - _cs.focusingGridPos.x == 2)
        {
            _cs.createBackGroundSector.InitSettingEventPos_BOSS(treeData, input, ref task);
        }
        else
            _cs.createBackGroundSector.InitSettingEventPos(treeData, input, ref task);

        task += () => _cs.createBackGroundSector.FillEnv();
    }
}