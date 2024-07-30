using System;
using System.Collections.Generic;
using UnityEngine;
using CreateMapTools;


public class CreateMapVisual : MonoBehaviour
{
    [SerializeField] private CreateMapBackgroundValues createMapBackgroundValues;
    [SerializeField] private CreateMapEventValues createMapEventValues;
    [SerializeField] private CreateRoadValues createRoadValues;
    [SerializeField] internal List<NodeScriptPerLevel> eventObjectList = new();
    [SerializeField] internal Vector2Int focusingNode;
    [SerializeField] internal _ValueSet_EventCoef _value;


    public void InitSettingEventPos_Root(EventNodeDataToPlace treeData)
    {
        NodeScriptPerLevel rootLevel = eventObjectList.tryAddLevel(0);
        rootLevel.nodeList = createMapEventValues.setEventArea_Root(rootLevel.transformList);
        rootLevel.nodeTerrainData.Add(treeData.nodeTerrainData[0]);
        rootLevel.eventIndex.Add(treeData.nodeEventData[0]);
        return;
    }

    public void InitSettingEventPos(EventNodeDataToPlace treeData, int index)
    {
        focusingNode = treeData.focusGridPos;
        int std = (int)(eventObjectList.getChildRangeByGridPos(new Vector2Int(treeData.focusGridPos.x, index)).x);
        eventObjectList[focusingNode.x].childStd = std;
        int level;

        if (treeData.targetLevel > 0)
            level = treeData.targetLevel + treeData.focusGridPos.x;
        else
        {
            NodeScriptPerLevel lastLevel = eventObjectList[eventObjectList.Count - 1];
            lastLevel.connectingData.setConnectedData(treeData.nodeTreeData);
        }

        level = treeData.targetLevel + treeData.focusGridPos.x;

        if (treeData.targetLevel < 0)
        {
            NodeScriptPerLevel lastlevel = eventObjectList[eventObjectList.Count - 1];

            foreach (var item in lastlevel.nodeList)
            {
                item.gameObject.SetActive(true);
            }

            return;
        }

        int aryIndex;
        if (index == -1)
            aryIndex = 0;
        else
            aryIndex = index;

        // 자식들 평균위치 계산
        Vector2 sum = GetFocusTransform().position;
        sum.x = createMapEventValues.GetChildSumOfAxis(eventObjectList, focusingNode, aryIndex);

        // 실질적 event pos 구현
        List<Vector2> createList = createMapEventValues.GetEventPointRow(treeData.nodeTreeData, level,
            sum + createMapEventValues.stdPos);

        // event data 처리하기 위한 대충 계산
        NodeScriptPerLevel currLevel = eventObjectList[focusingNode.x];
        NodeScriptPerLevel nextLevel = eventObjectList[level - 1];
        NodeScriptPerLevel createdLevel = eventObjectList.tryAddLevel(level);

        // event sprite 생성
        createdLevel.nodeList = createMapEventValues.setEventArea(createList, createdLevel.transformList, treeData.nodeEventData);

        if (true)
        {
            for (int i = 0; i < treeData.nodeEventData.Count; i++)
            {
                createdLevel.eventIndex.Add(treeData.nodeEventData[i]);
            }
        }

        if (true)
        {
            foreach (var item in createdLevel.nodeList)
            {
                item.gameObject.SetActive(false);
            }
        };

        createdLevel.nodeTerrainData = treeData.nodeTerrainData;
        nextLevel.connectingData.setConnectedData(treeData.nodeTreeData);

        // 카메라 이동 후 해야 할 작업 1. 활성화, 2. 길 연결, 3. 길 그림
        foreach (var item in createdLevel.nodeList)
        {
            item.gameObject.SetActive(true);
        }

        // build Road, ( need connectData => nextLevel.connData )
        createRoadValues.setRoadAreaSetBySet(nextLevel, createdLevel, currLevel.childStd);
        createMapBackgroundValues.setRoadSpriteByData(createdLevel);

        return;
    }

    // data 
    public void InitSettingEventPos(EventNodeDataToPlace treeData, int index, ref GUI_MapScenario.ProgressMap_preInput task)
    {
        //Event - Setting Node by Data
        focusingNode = treeData.focusGridPos;
        int std = (int)(eventObjectList.getChildRangeByGridPos(new Vector2Int(treeData.focusGridPos.x, index)).x);
        eventObjectList[focusingNode.x].childStd = std;

        int level;

        if (treeData.targetLevel > 0)
            level = treeData.targetLevel + treeData.focusGridPos.x;
        else
        {
            NodeScriptPerLevel lastLevel = eventObjectList[eventObjectList.Count - 1];
            lastLevel.connectingData.setConnectedData(treeData.nodeTreeData);
        }

        level = treeData.targetLevel + treeData.focusGridPos.x;

        if (treeData.targetLevel < 0)
        {
            NodeScriptPerLevel focuslevel = eventObjectList[eventObjectList.Count - 2];
            NodeScriptPerLevel lastlevel = eventObjectList[eventObjectList.Count - 1];
            task += () =>
            {
                foreach (var item in lastlevel.nodeList)
                {
                    item.gameObject.SetActive(true);
                }
            };

            return;
        }

        int aryIndex;
        if (index == -1)
            aryIndex = 0;
        else
            aryIndex = index;

        // 자식들 평균위치 계산
        Vector2 sum = GetFocusTransform().position;
        sum.x = createMapEventValues.GetChildSumOfAxis(eventObjectList, focusingNode, aryIndex);

        // 실질적 event pos 구현
        List<Vector2> createList = createMapEventValues.GetEventPointRow(treeData.nodeTreeData, level,
            sum + createMapEventValues.stdPos);

        // event data 처리하기 위한 대충 계산
        NodeScriptPerLevel currLevel = eventObjectList[focusingNode.x];
        NodeScriptPerLevel nextLevel = eventObjectList[level - 1];
        NodeScriptPerLevel createdLevel = eventObjectList.tryAddLevel(level);

        // event sprite 생성
        createdLevel.nodeList = createMapEventValues.setEventArea(createList, createdLevel.transformList, treeData.nodeEventData);

        if (true)
        {
            for (int i = 0; i < treeData.nodeEventData.Count; i++)
            {
                createdLevel.eventIndex.Add(treeData.nodeEventData[i]);
            }
        }

        if (true)
        {
            foreach (var item in createdLevel.nodeList)
            {
                item.gameObject.SetActive(false);
            }
        };

        createdLevel.nodeTerrainData = treeData.nodeTerrainData;
        nextLevel.connectingData.setConnectedData(treeData.nodeTreeData);


        // 카메라 이동 후 해야 할 작업 1. 활성화, 2. 길 연결, 3. 길 그림
        task += () =>
        {
            foreach (var item in createdLevel.nodeList)
            {
                item.gameObject.SetActive(true);
            }  
        };

        // build Road, ( need connectData => nextLevel.connData )
        task += () => createRoadValues.setRoadAreaSetBySet(nextLevel, createdLevel, currLevel.childStd);
        task += () => createMapBackgroundValues.setRoadSpriteByData(createdLevel);

        return;
    }

    public void InitSettingEventPos_BOSS_byHistory(EventNodeDataToPlace treeData, int index)
    {
        focusingNode = treeData.focusGridPos;
        int std = (int)(eventObjectList.getChildRangeByGridPos(new Vector2Int(treeData.focusGridPos.x, index)).x);
        eventObjectList[focusingNode.x].childStd = std;
        int level;


        if (treeData.targetLevel > 0)
            level = treeData.targetLevel + treeData.focusGridPos.x;
        else
        {
            NodeScriptPerLevel lastLevel = eventObjectList[eventObjectList.Count - 1];
            lastLevel.connectingData.setConnectedData(treeData.nodeTreeData);
        }

        level = treeData.targetLevel + treeData.focusGridPos.x;

        if (treeData.targetLevel < 0)
        {
            NodeScriptPerLevel focuslevel = eventObjectList[eventObjectList.Count - 2];
            NodeScriptPerLevel lastlevel = eventObjectList[eventObjectList.Count - 1];
            foreach (var item in lastlevel.nodeList)
            {
                item.gameObject.SetActive(true);
            }


            Debug.Log(treeData.targetLevel + " << Sorry i cut synario");
            return;
        }

        int aryIndex;
        if (index == -1)
            aryIndex = 0;
        else
            aryIndex = index;

        // 자식들 평균위치 계산
        Vector2 sum = GetFocusTransform().position;
        sum.x = createMapEventValues.GetChildSumOfAxis(eventObjectList, focusingNode, aryIndex);

        // 실질적 event pos 구현
        List<Vector2> createList = createMapEventValues.GetEventPointRow(treeData.nodeTreeData, level,
            sum + createMapEventValues.stdPos);

        for (int i = 0; i < createList.Count; i++)
        {
            createList[i] *= new Vector2(1f, 1.05f);
        }

        // event data 처리하기 위한 대충 계산
        NodeScriptPerLevel currLevel = eventObjectList[focusingNode.x];
        NodeScriptPerLevel nextLevel = eventObjectList[level - 1];
        NodeScriptPerLevel createdLevel = eventObjectList.tryAddLevel(level);

        // event sprite 생성
        createdLevel.nodeList = createMapEventValues.setEventArea_BOSS(createList, createdLevel.transformList, treeData.nodeEventData);
        {
            foreach (var item in createdLevel.nodeList)
            {
                item.gameObject.SetActive(false);
            }
        };

        createdLevel.nodeTerrainData = treeData.nodeTerrainData;
        nextLevel.connectingData.setConnectedData(treeData.nodeTreeData);


        // 카메라 이동 후 해야 할 작업 1. 활성화, 2. 길 연결, 3. 길 그림
        foreach (var item in createdLevel.nodeList)
        {
            item.gameObject.SetActive(true);
        }

        // build Road, ( need connectData => nextLevel.connData )
        createRoadValues.setRoadAreaSetBySet(nextLevel, createdLevel, currLevel.childStd);
        createMapBackgroundValues.setRoadSpriteByData(createdLevel);

        return;
    }

    public void InitSettingEventPos_BOSS(EventNodeDataToPlace treeData, int index, ref GUI_MapScenario.ProgressMap_preInput task)
    {
        focusingNode = treeData.focusGridPos;
        int std = (int)(eventObjectList.getChildRangeByGridPos(new Vector2Int(treeData.focusGridPos.x, index)).x);
        eventObjectList[focusingNode.x].childStd = std;
        int level;


        if (treeData.targetLevel > 0)
            level = treeData.targetLevel + treeData.focusGridPos.x;
        else
        {
            NodeScriptPerLevel lastLevel = eventObjectList[eventObjectList.Count - 1];
            lastLevel.connectingData.setConnectedData(treeData.nodeTreeData);
        }

        level = treeData.targetLevel + treeData.focusGridPos.x;

        if (treeData.targetLevel < 0)
        {
            NodeScriptPerLevel focuslevel = eventObjectList[eventObjectList.Count - 2];
            NodeScriptPerLevel lastlevel = eventObjectList[eventObjectList.Count - 1];
            task += () =>
            {
                foreach (var item in lastlevel.nodeList)
                {
                    item.gameObject.SetActive(true);
                }
            };

            return;
        }

        int aryIndex;
        if (index == -1)
            aryIndex = 0;
        else
            aryIndex = index;

        // 자식들 평균위치 계산
        Vector2 sum = GetFocusTransform().position;
        sum.x = createMapEventValues.GetChildSumOfAxis(eventObjectList, focusingNode, aryIndex);

        // 실질적 event pos 구현
        List<Vector2> createList = createMapEventValues.GetEventPointRow(treeData.nodeTreeData, level,
            sum + createMapEventValues.stdPos);

        for (int i = 0; i < createList.Count; i++)
        {
            createList[i] *= new Vector2(1f,1.05f);
        }

        // event data 처리하기 위한 대충 계산
        NodeScriptPerLevel currLevel = eventObjectList[focusingNode.x];
        NodeScriptPerLevel nextLevel = eventObjectList[level - 1];
        NodeScriptPerLevel createdLevel = eventObjectList.tryAddLevel(level);

        // event sprite 생성
        createdLevel.nodeList = createMapEventValues.setEventArea_BOSS(createList, createdLevel.transformList, treeData.nodeEventData);
        {
            foreach (var item in createdLevel.nodeList)
            {
                item.gameObject.SetActive(false);
            }
        };

        createdLevel.nodeTerrainData = treeData.nodeTerrainData;
        nextLevel.connectingData.setConnectedData(treeData.nodeTreeData);


        GUI_MapScenario.ProgressMap_preInput tempTask = new(() => {;});

        // 카메라 이동 후 해야 할 작업 1. 활성화, 2. 길 연결, 3. 길 그림
        tempTask += () =>
        {
            foreach (var item in createdLevel.nodeList)
            {
                item.gameObject.SetActive(true);
            }
        };

        // build Road, ( need connectData => nextLevel.connData )
        tempTask += () => createRoadValues.setRoadAreaSetBySet(nextLevel, createdLevel, currLevel.childStd);
        tempTask += () => createMapBackgroundValues.setRoadSpriteByData(createdLevel);

        if (task == null)
            tempTask();
        else
            task += tempTask;

        return;
    }

    public Vector2 GetAxisX_CreatedNode()
    {
        NodeScriptPerLevel createdLevel = eventObjectList[eventObjectList.Count - 1];

        float sumX = 0;
        int i = 0;
        for (; i < createdLevel.transformList.Count; i++)
        {
            sumX += createdLevel.transformList[i].position.x;
        }
        sumX /= i;

        return new Vector2(GetFocusTransform().position.x, GetFocusTransform().position.y);
    }

    public Transform GetNodeTransformByGrid(Vector2Int gridPos)
    {
        return eventObjectList.getTransformByGridPos(gridPos);
    }

    public Transform GetFocusTransform()
    {
        return eventObjectList.getTransformByGridPos(focusingNode);
    }

    public void FillEnv(bool isRoot = false)
    {
        if (isRoot)
            FillBG_OBJ_Root();
        else if (eventObjectList.Count <= focusingNode.x + 2)
            FinishUpEnv();
        else
            FillBG_OBJ(eventObjectList.Count - 2);

        void FillBG_OBJ(int level)
        {   
            if (level < 1)
                return;

            NodeScriptPerLevel currLayer = eventObjectList[level];
            NodeScriptPerLevel nextLayer = eventObjectList[level+1];

            List<NodeController> events = currLayer.nodeList;

            Vector2 range_Curr = eventObjectList.getChildRangeByGridPos_FocusStd(focusingNode);

            if (true)
            {
                this.setEvent_byPatrol(currLayer, range_Curr);  // 노드 선택에 따른 focus node 상호작용
                this.setEvent_byHistory(currLayer, range_Curr); // 현재 자식들을 데이터 참조 및 이벤트 리스트 계산
            }

            for (int i = (int)range_Curr.x; i < range_Curr.y + 1; i++)
            {
                eventObjectList.SetTerrain(focusingNode, new Vector2Int(focusingNode.x+1, i));
                List<SpriteRenderer> instantBG = createMapBackgroundValues.setBackgroundSprites(
                    events[i].transform.position,
                    CreateMapTools.CreateMapTools.terrainDataToIndex(currLayer.nodeTerrainData[i]),
                    level
                ); // << color

                List<SpriteRenderer> trashBin = new();

                if (true)
                {
                    events[i].ActiveDetailed_RealTime(currLayer.eventIndex[i]); // 할당후 해당 노드 이미지 변경
                    events[i].ActiveDetailed();  // 그리고 변경된 이미지를 적용
                }


                // 이벤트 적용(원형 삭제)
                DisableIntersectingSprites(instantBG, trashBin, events[i].EventNode, createMapEventValues.ereaseScale);

                //길 적용, focus - next
                DisableIntersectingSprites(instantBG, trashBin, currLayer.FindToDes(i), createRoadValues.ereaseScale);

                //길 적용, next - supers
                for (int j = 0; j < nextLayer.roadSetList.Count; j++)
                {
                    DisableIntersectingSprites(instantBG, trashBin, nextLayer.roadSetList[j].roadSet, createRoadValues.ereaseScale);
                }

                foreach (SpriteRenderer item in trashBin)
                {
                    Destroy(item.gameObject);
                }
            }

            return;
        }

        List<SpriteRenderer> FillBG_OBJ_Root()
        {
            NodeScriptPerLevel currLayer = eventObjectList[0];
            NodeScriptPerLevel nextLayer = eventObjectList[1];

            List<NodeController> events = currLayer.nodeList;
            List<SpriteRenderer> rtn = new();

            List<SpriteRenderer> instantBG = createMapBackgroundValues.setBackgroundSprites(events[0].transform.position,0,0);
            List<SpriteRenderer> _trashbin = new();

            events[0].ActiveDetailed(); //Root
            DisableIntersectingSprites(instantBG, _trashbin, events[0].EventNode, createMapEventValues.ereaseScale);

            //길 적용, next - super
            for (int j = 0; j < nextLayer.roadSetList.Count; j++)
            {
                DisableIntersectingSprites(instantBG, _trashbin, nextLayer.roadSetList[j].roadSet, createRoadValues.ereaseScale);
            }

            foreach (SpriteRenderer item in _trashbin)
            {
                Destroy(item.gameObject);
            }

            return rtn;
        }
    }
    public void FinishUpEnv()
    {
        NodeScriptPerLevel currLayer = eventObjectList[eventObjectList.Count - 2];
        NodeScriptPerLevel nextLayer = eventObjectList[eventObjectList.Count - 1];

        List<NodeController> events = nextLayer.nodeList;

        Vector2 range_Curr = eventObjectList.getChildRangeByGridPos_FocusStd(focusingNode);

        for (int i = (int)range_Curr.x; i < range_Curr.y + 1; i++)
        {
            List<SpriteRenderer> instantBG = createMapBackgroundValues.setBackgroundSprites_BOSS(
                events[i].transform.position,
                CreateMapTools.CreateMapTools.terrainDataToIndex(nextLayer.nodeTerrainData[i]),
                2
                );
            List<SpriteRenderer> trashBin = new();

            events[i].ActiveDetailed(); // LastDance

            //길 적용, focus - next
            DisableIntersectingSprites(instantBG, trashBin, currLayer.FindToDes(i), createRoadValues.ereaseScale);

            for (int j = 0; j < nextLayer.roadSetList.Count; j++)
            {
                DisableIntersectingSprites(instantBG, trashBin, nextLayer.roadSetList[j].roadSet, createRoadValues.ereaseScale);
            }

            foreach (SpriteRenderer item in trashBin)
            {
                Destroy(item.gameObject);
            }
        }

        return;
    }
    /*
            1. 배경 오브젝트 생성

            2. 전방의 길 제거

            3. 후방의 길 제거

            4. 캡쳐 및 배치

            5. 오브젝트 제거
         */

    //Road
    private void DisableIntersectingSprites(List<SpriteRenderer> backgroundSprites, List<SpriteRenderer> trashbin, List<SpriteRenderer> emptyZones, float radio)
    {
        if (emptyZones == null)
            return;

        for (int j = 0; j < backgroundSprites.Count; j++)
        {
            float min = 0;
            for (int i = 0; i < emptyZones.Count; i++)
            {
                float temp = Vector2.Distance(emptyZones[i].transform.position, backgroundSprites[j].transform.position);
                if(min != 0)
                    min = Mathf.Min(temp, min);
                else
                    min = temp;
            }

            backgroundSprites[j].transform.name =  "checked - " + min;

            if (min < radio)
            {
                backgroundSprites[j].transform.name = "sad";
                backgroundSprites[j].gameObject.SetActive(false);
                trashbin.Add(backgroundSprites[j]);
                backgroundSprites.RemoveAt(j);
                j--;
                continue;
            }

        }

        return;
    }

    private void DisableIntersectingSprites(List<SpriteRenderer> backgroundSprites, List<SpriteRenderer> trashbin, SpriteRenderer emptyZones, float radio)
    {
        if (emptyZones == null)
            return;

        for (int j = 0; j < backgroundSprites.Count; j++)
        {
            float min = 0;
            float temp = Vector2.Distance(emptyZones.transform.position, backgroundSprites[j].transform.position);
            if (min != 0)
                min = Mathf.Min(temp, min);
            else
                min = temp;

            backgroundSprites[j].transform.name = "checked - " + min;

            if (min < radio)
            {
                backgroundSprites[j].transform.name = "ToRemoveObj";
                backgroundSprites[j].gameObject.SetActive(false);
                trashbin.Add(backgroundSprites[j]);
                backgroundSprites.RemoveAt(j);
                j--;
                continue;
            }

        }

        return;
    }

}

[Serializable]
public struct CreateMapBackgroundValues
{
    [SerializeField] internal RefData_howNodeSet BgObjDataSet;
    public Transform parentTransform;
    
    public int countX;
    public int countY;

    public float coefX;
    public float coefY;

    public float coef_CircleLength;
    public float coef_Outline;

    public BoxPointer backgroundArea;
    
}

[Serializable]
public struct CreateMapEventValues
{
    public GameObject lastBOSS;
    public GameObject prefab;
    public List<EventDataSet> eventSpriteDB;

    public Transform parentTransform;

    public int gapPerLevel;
    public int gapPerEvent;
    public Vector2 radious;

    public float ereaseScale;
    public BoxPointer eventArea;
    public Vector2 stdPos;
}

[Serializable]
public struct CreateRoadValues
{
    public GameObject roadObject;

    public Transform parentTransform;
    
    public float gap;
    public float coef;

    public float ignoreRange;
    public float ereaseScale;
    public bool isBool_RoadVis;
}

[Serializable]
public class NodeScriptPerLevel
{
    public int level;
    public int childStd = 0;
    public List<Transform> transformList = new();
    public List<Vector2> connectingData = new();
    public List<Vector3> nodeTerrainData = new();
    public List<NodeController> nodeList = new();
    public List<RoadSet> roadSetList = new();
    public List<int> eventIndex = new();
    public List<float> terrainScale = new();

    public NodeScriptPerLevel(int _level , int childStd = 0)
    {
        level = _level;

        if (transformList == null)
            transformList = new();

        if (nodeList == null)
            nodeList = new();

        for (int i = 0; i < 6; i++)
        {
            terrainScale.Add(0);
        }
    }

    public List<SpriteRenderer> FindToDes(int des)
    {
        List<SpriteRenderer> rtnList = new();
        for (int i = 0; i < roadSetList.Count; i++)
        {
            if (roadSetList[i].nameV3.z == des)
                rtnList.AddRange(roadSetList[i].roadSet);
        }

        return rtnList;
    }
}

[Serializable]
public class RoadSet
{
    public Vector3Int nameV3;
    public Vector3 srcTerrain;
    public Vector3 desTerrain;
    public Transform _transform;

    public List<SpriteRenderer> roadSet = new();
}

[Serializable]
public class EventDataSet
{
    public string name;
    public Sprite sil_Sprite;
    public List<Sprite> real_Sprite;
    public List<Sprite> prefab;
}

[Serializable]
public class BoxPointer
{
    public float length_X, length_Y; 
    public float GetLength_X()
    {
        return length_X;
    }

    public float GetLength_Y()
    {
        return length_Y;
    }

    public float getRandomHeight(float coex = 0f)
    {
        float temp = 1 - coex * coex;
        return GetLength_Y() * (temp - 0.7f) - GetLength_Y()* 0.2f; 
    }
}

