using System;
using System.Collections.Generic;
using UnityEngine;

public class GUI_SelectedNode : MonoBehaviour
{
    [SerializeField] internal CreateMapVisual adress;
    [SerializeField] internal List<NodeScriptPerLevel> _eventObjectList = new();
    [SerializeField] internal GUI_MapScenario values;
    [SerializeField] internal GUI_MapNodeInfo NodeInfoGUI;
    [SerializeField] internal List<SpriteRenderer> myList;
    [SerializeField] internal State_NodeBtn state = new();

    [SerializeField] internal Color HighLightMaterial, subHighLightMaterial, DefaultMaterial, DefaultMaterial_Node;
    [SerializeField] internal Action<int> _decideSelected_Func; // Progress 
    [SerializeField] internal float power; public float timer;
    
    public void _InitSelectedFunc(Action<int> target)
    {
        _decideSelected_Func = target;
    }

    public void OnLoadedInit()
    {
        this.SaveHighlight();
        Clear();
        state.StartGetInput();
    }

    public void _SelectEvent(int value)
    {
        if (!state.isTimeToInput)
            return;

        // progress?
        if (value == state.selectedNode)
        {
            decideSelected();
            return;
        }
        else if(-1 != state.selectedNode)
        {
            Clear();
            return;
        }

        Select(value);
    }

    public void CancelSelected()
    {
        if (!state.isTimeToInput)
        {
            return;
        }
        Clear();
    }

    void decideSelected()
    {
        state.StopGetInput();

        if (state.selectedNode < 0)
        {
            return;
        }

        _decideSelected_Func(state.selectedNode);
    }

    public void Clear()
    {
        state.selectedNode = -1;
        for (int i = 0; i < myList.Count; i++)
        {
            myList[i].color = DefaultMaterial;
        }

        if(myList.Count > 0)
            myList[myList.Count-1].color = DefaultMaterial_Node;


        myList.Clear();
        if (NodeInfoGUI == null)
        {
            NodeInfoGUI = MapDataSGT.GlobalInit().CurrMS._SC.mapNodeInfo;
        }
        NodeInfoGUI.SetGUI_toDefault();
        return;
    }

    public void Select(int input)
    {
        List<SpriteRenderer> materials =  this.getDataToGUI_ByIndex(input);
     
        for (int i = 0; i < myList.Count; i++)
        {
            myList[i].color = DefaultMaterial;
        }

        if (myList.Count != 0)
        {
            myList[myList.Count-1].color = Color.white;
        }

        myList.Clear();

        setHighLight();

        if (true)
        {
            this.getNodeData_ByIndex(input);
        }



        return;

        void setHighLight()
        {
            for (int i = 0; i < materials.Count; i++)
            {
                myList.Add(materials[i]);
                materials[i].color = HighLightMaterial;
            }

            return;
        }
    }
}

[Serializable]
internal class State_NodeBtn
{
    [SerializeField] internal bool isTimeToInput = true;
    [SerializeField] internal int selectedNode;

    internal void StopGetInput()
    {
        isTimeToInput = false;
    }
    internal void StartGetInput()
    {
        isTimeToInput = true;
    }
}