using System;
using System.Collections.Generic;
using UnityEngine;

public class CreateStageScenario : MonoBehaviour
{
    internal MapScenario.OnClickFunc onClick;

    public CreateMap createBackGroundSector;
    public CreateFloatingNode createNodeSector;
    public Transform focusingNode;
    public Vector2Int focusingGridPos;

    public List<TouchableNode> currSelectable;     
    public List<EventNodeDataToPlace> toManaging;
    public Camera MapCamera;

    public void SetOnClickFunc(MapScenario.OnClickFunc _onClick)
    {
        onClick = _onClick;
    }

    public void NewGame(int seed)
    {
        UnityEngine.Random.InitState(seed);
        this.SetInitData();

        BuildStem_byInit();
        BuildLeaf_byInit();

        currSelectable.SettingNextDestination(createBackGroundSector, createNodeSector, onClick);
        this.SetVisualObject();
    }

    public void LoadGame(int seed,int[] history)
    {
        UnityEngine.Random.InitState(seed);
        this.SetInitData();

        BuildStem_byInit();
        BuildLeaf_byInit();

        for (int i = 0; i < history.Length; i++)
        {
            currSelectable.SettingNextDestination(createBackGroundSector, createNodeSector, history[i]);
            if(createNodeSector.createNodeValues.maxLevel == i)
            {
               // History Is Over, Fuck;
                return;
            }
            BuildLeaf_byHistory(history[i], 
                (createNodeSector.createNodeValues.maxLevel - i == 2)); // 보스 판단 
        }


        currSelectable.SettingNextDestination(createBackGroundSector, createNodeSector, onClick);
        this.SetVisualObject();
    }

    public Vector3 ProgressMap(int inputChildIndex, ref GUI_MapScenario.ProgressMap_preInput task)
    {
        if (!createNodeSector.IsInit())
            return Vector3.one*-1;

        if(createNodeSector.createNodeValues.maxLevel == focusingGridPos.x)
        {
            return Vector3.zero;
        }

        // 말단 생성 후, 해당 노드의 트랜스폼 갱신
        BuildLeaf(ref task, inputChildIndex);

        focusingNode = createBackGroundSector.GetFocusTransform();
        focusingGridPos = createNodeSector.getFocusGridPos();

        Vector2Int range_Curr = createBackGroundSector.eventObjectList.getChildRangeByGridPos_FocusStd(focusingGridPos);
        Vector3 rtnV3 = createBackGroundSector.getAxisX_CreatedNode();

        //맵 생성을 이후로 미룰려고 해둔짓
        task += () => currSelectable.SettingNextDestination(createBackGroundSector, createNodeSector, onClick);

        return rtnV3;
    }

    public void BuildStem_byInit()
    {
        EventNodeDataToPlace treeData = createNodeSector.buildStem();
        toManaging.Add(treeData);

        createBackGroundSector.InitSettingEventPos(treeData , -1);
        createBackGroundSector.FillEnv(true);
    }

    public void BuildLeaf_byInit(int input = -1, bool isBoss = false)
    {
        EventNodeDataToPlace treeData = createNodeSector.buildTree(input);
        toManaging.Add(treeData);

        createBackGroundSector.InitSettingEventPos(treeData, input);
        createBackGroundSector.FillEnv();
    }

    public void BuildLeaf_byHistory(int input, bool isBoss)
    {
        EventNodeDataToPlace treeData = createNodeSector.buildTree(input);
        toManaging.Add(treeData);

        if (isBoss)
        {
            createBackGroundSector.InitSettingEventPos_BOSS_byHistory(treeData, input);
            createBackGroundSector.FillEnv();
            return;
        }
        else
            createBackGroundSector.InitSettingEventPos(treeData, input);

        createBackGroundSector.FillEnv();
    }

    // 시간차 생성
    public void BuildLeaf(ref GUI_MapScenario.ProgressMap_preInput task, int input = -1)
    {
        EventNodeDataToPlace treeData = createNodeSector.buildTree(input);
        toManaging.Add(treeData);

        if(createNodeSector.createNodeValues.maxLevel - focusingGridPos.x == 2)
        {
            createBackGroundSector.InitSettingEventPos_BOSS(treeData, input, ref task);
        }
        else
            createBackGroundSector.InitSettingEventPos(treeData, input,ref task);

        task += () => createBackGroundSector.FillEnv();
    }

    public int GetIndex_atCurrFocusing()
    {
        Vector2Int temp = createBackGroundSector.focusingNode;
        return createBackGroundSector.eventObjectList[temp.x].eventIndex[temp.y];
    }
}

[Serializable]
public class EventNodeDataToPlace
{
    public Vector2Int focusGridPos;
    public int targetLevel;
    public List<int> nodeTreeData;
    public List<int> nodeEventData;
    public List<Vector3> nodeTerrainData;
}

