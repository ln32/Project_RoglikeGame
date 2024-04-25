using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.Rendering.DebugUI;

internal static class GUI_SelectedNodeTools
{
    internal static void getNodeData_ByIndex(this GUI_SelectedNode obj, int input)
    {
        obj.state.selectedNode = input;

        List<NodeScriptPerLevel> data = obj._eventObjectList;
        int maxLevel = data.Count;

        NodeScriptPerLevel last = data[maxLevel - 1];
        NodeScriptPerLevel select = data[maxLevel - 2];
        NodeScriptPerLevel focus = data[maxLevel - 3];

        if (maxLevel <= obj.adress.focusingNode.x + 2)
        {
            select = last;
            focus = select;
        }
        input += focus.childStd;

        if (true)
        {
            Vector2Int focus_Adrr = obj.adress.focusingNode;
            Vector2Int select_Adrr = new Vector2Int(focus_Adrr.x + 1, input);

            if(data[select_Adrr.x].eventIndex.Count == 0)
            {
                Debug.Log("BOSS is Selected");
                GUI_MapNodeInfo _nodeInfoGUI = obj.NodeInfoGUI;

                _nodeInfoGUI.SetGUI_toActive();
                _nodeInfoGUI.SetGUI_byImage(data[select_Adrr.x].nodeList[select_Adrr.y].EventNode.sprite);
                _nodeInfoGUI.SetGUI_byName("BOSS");// + data[select_Adrr.x].terrainScale[select_Adrr.y]);
                _nodeInfoGUI.SetGUI_byVector3(data[select_Adrr.x].nodeTerrainData[select_Adrr.y]);
            }
            else
            {
                Debug.Log(data[select_Adrr.x].eventIndex[select_Adrr.y] + " is Selected");

                GUI_MapNodeInfo _nodeInfoGUI = obj.NodeInfoGUI;

                _nodeInfoGUI.SetGUI_toActive();
                _nodeInfoGUI.SetGUI_byImage(data[select_Adrr.x].nodeList[select_Adrr.y].EventNode.sprite);
                _nodeInfoGUI.SetGUI_byName(_convEventIndex_toString(data[select_Adrr.x].eventIndex[select_Adrr.y]));// + data[select_Adrr.x].terrainScale[select_Adrr.y]);
                _nodeInfoGUI.SetGUI_byVector3(data[select_Adrr.x].nodeTerrainData[select_Adrr.y]);
            }
        }

        return;
    }

    static string _convEventIndex_toString(int index)
    {
        int index_0 = index / 100;
        int index_1 = index % 100;

        switch (index_0)
        {
            case 0: 
               return SubBranch_0();
            case 1:
                return SubBranch_1();
            case 2:
                return SubBranch_2();
            default:
                break;
        }

        return "";

        string SubBranch_0()
        {
            switch (index_1)
            {
                case 0:
                    return "house";
                case 1:
                    return "castle";
                case 2:
                    return "church";
                case 3:
                    return "temple";
                default:
                    break;
            }
            return "sad";
        }
        string SubBranch_1()
        {
            switch (index_1)
            {
                case 0:
                    return "camp";
                case 1:
                    return "ruin";
                case 2:
                    return "cave";
                case 3:
                    return "odd";
                default:
                    break;
            }
            return "sad";
        }
        string SubBranch_2()
        {
            switch (index_1)
            {
                case 0:
                    return "flag";
                case 1:
                    return "portal";
                case 2:
                    return "deep ruin";
                case 3:
                    return "totem";
                default:
                    break;
            }
            return "sad";
        }
    }

    internal static List<SpriteRenderer> getDataToGUI_ByIndex(this GUI_SelectedNode obj, int input)
    {
        obj.state.selectedNode = input;

        List<NodeScriptPerLevel> data = obj._eventObjectList;
        int maxLevel = data.Count;

        NodeScriptPerLevel last = data[maxLevel - 1];
        NodeScriptPerLevel select = data[maxLevel - 2];
        NodeScriptPerLevel focus = data[maxLevel - 3];

        if(maxLevel <= obj.adress.focusingNode.x + 2)
        {
            select = last;
            focus = select;
        }

        //input += focus.childStd;
        List<SpriteRenderer> materials = new List<SpriteRenderer>();

        // 1. dst로가는 길, dst 아이콘
        //Debug.Log(select.getSelectRoadMaterials(obj.adress.focusingNode.y, input + focus.childStd).Count + " <<");
        materials.AddRange(select.getSelectRoadMaterials(obj.adress.focusingNode.y, input + focus.childStd));
        materials.Add(select.getSelectMaterial(input + focus.childStd));

        // 2. dst에 연결된 노드 범위 rangeV2
        //Vector2Int rangeV2 = select._getChildsByData(input);
        
        //Debug.Log(rangeV2 + " <- saddd, " + last.childStd + " / " + select.childStd + " / " + focus.childStd);

        // dst -> rangeV2 각각에 해당하는 길과 노드
        //Vector2Int rangeV2 = select._getChildsByData(input);

        return materials;
    }

    internal static SpriteRenderer getSelectMaterial(this NodeScriptPerLevel select, int index)
    {
        return select.nodeList[index].myPoint;// + select.childStd
    }

    internal static List<SpriteRenderer> getSelectRoadMaterials(this NodeScriptPerLevel select, int src, int dst)
    {
        Vector2Int dirV2 = new Vector2Int(0, 0);
        for (int i = 0; i < select.roadSetList.Count; i++)
        {
            if (select.roadSetList[i].nameV3.y == src && select.roadSetList[i].nameV3.z == dst)
                return select.roadSetList[i].roadSet;
        }

        Debug.Log("Tlqkf!!! - " + src + " / " + dst);
        return null;
    }

    internal static Vector2Int _getChildsByData(this NodeScriptPerLevel data, int index)
    {
        int min = (int)data.connectingData[index].x;
        int max = (int)data.connectingData[index].y;

        return new Vector2Int(min,max);
    }
    internal static bool isLastNode(this NodeScriptPerLevel data, int index)
    {
        return data.connectingData.Count == 0;
    }

    internal static void SaveHighlight_Root(this GUI_SelectedNode obj)
    {
        obj._eventObjectList = obj.adress.eventObjectList;
        List<NodeScriptPerLevel> data = obj._eventObjectList;
        data[0].nodeList[0].myPoint.color = obj.subHighLightMaterial;

        return;
    }

    internal static void SaveHighlight(this GUI_SelectedNode obj)
    {
        obj.state.selectedNode = -1;

        for (int i = 0; i < obj.myList.Count; i++)
        {
            obj.myList[i].color = (obj.subHighLightMaterial);
        }
        
        obj.myList.Clear();
        return;
    }

    internal static void GetDataToGUI_ByIndex(this List<Material> target, Material material)
    {
        return;
    }

}