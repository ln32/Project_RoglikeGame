using System;
using System.Collections.Generic;
using UnityEngine;

public class _ValueSet_EventCoef : MonoBehaviour
{
    // CurrState
    internal Dictionary<int, int> currIncountState = new Dictionary<int, int>();
    [SerializeField] internal List<Vector2Int> currIncountState_ToShow = new List<Vector2Int>();

    // Original data
    internal Dictionary<int, _EventCoef> EventDataSet = new Dictionary<int, _EventCoef>(); 

    // _CoefSet_DEBUG Ctrl
    [SerializeField] internal List<Vector2> AgingCoefV2_ToSet = new List<Vector2>();
    [SerializeField] internal List<Vector2Int> Gap_onFocused_ToSet = new List<Vector2Int>();
    [SerializeField] internal List<Vector2Int> Gap_onIgnore_ToSet = new List<Vector2Int>();

    internal void Awake()
    {
        SetDict_Refresh();
    }

    public int getCoef_Gap_onFocused(int _eventIndex)
    {
        _EventCoef _value;
        EventDataSet.TryGetValue(_eventIndex, out _value);

        return _value._Gap_onFocused;
    }


    protected void SetCoefs_byInputData()
    {
        AgingCoefV2_ToSet.Clear();
        Gap_onFocused_ToSet.Clear();
        Gap_onIgnore_ToSet.Clear();

        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                AgingCoefV2_ToSet.Add(new Vector2(x * 100 + y, x * y + 0.5f));
            }
        }

        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                Gap_onFocused_ToSet.Add(new Vector2Int(x * 100 + y, 1));
            }
        }

        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                Gap_onIgnore_ToSet.Add(new Vector2Int(x * 100 + y, 1));
            }
        }

        SetDict();
    }

    protected void SetDict_Refresh()
    {
        SetDict();
        SetCoef_byAging();
        SetCoef_Gap_onFocused_ToSet();
        SetCoef_Gap_onIgnore_ToSet();
    }

    protected void SetDict()
    {
        if (currIncountState != null)
            currIncountState.Clear();
        currIncountState = new();
    }

    internal void SetCoef_byAging()
    {
        for (int i = 0; i < AgingCoefV2_ToSet.Count; i++)
        {
            int _eventIndex = (int)AgingCoefV2_ToSet[i].x;

            _EventCoef _value;
            EventDataSet.TryGetValue(_eventIndex, out _value);
            _value._Aging = AgingCoefV2_ToSet[i].y;

        }
    }

    internal void SetCoef_Gap_onFocused_ToSet()
    {
        for (int i = 0; i < AgingCoefV2_ToSet.Count; i++)
        {
            int _eventIndex = (int)Gap_onFocused_ToSet[i].x;

            _EventCoef _value;
            EventDataSet.TryGetValue(_eventIndex, out _value);
            _value._Gap_onFocused = Gap_onFocused_ToSet[i].y;
        }
    }

    internal void ApplyCurrState(List<int> target, Vector2Int childRangeV2, Vector2Int focusingPosV2)
    {
        // 전체적 이벤트 데이터 결정
        List<int> eventData;
        if (true)
        {
            eventData = SetDetailEventList(target, childRangeV2, focusingPosV2);
        }

        for (int index = childRangeV2.x; index < childRangeV2.y + 1; index++)
        {
            // 전체적 이벤트 -> 세부로 할당
            if (true)
            {
                target[index] = eventData[index];
            }
        }

    }

    protected void SetCoef_Gap_onIgnore_ToSet()
    {
        for (int i = 0; i < AgingCoefV2_ToSet.Count; i++)
        {
            int _eventIndex = (int)Gap_onIgnore_ToSet[i].x;

            _EventCoef _value;
            EventDataSet.TryGetValue(_eventIndex, out _value);
            _value._Gap_onIgnore = Gap_onIgnore_ToSet[i].y;
        }
    }


    protected List<int> SetDetailEventList(List<int> _eventData, Vector2Int _childRange,Vector2Int focusingPosV2)
    {
        int count = _childRange.y - _childRange.x + 1;
        List<int> rtnList = new List<int>();

        // fill up until empty to 0
        for (int i = 0; i < _eventData.Count; i++)
        {
            rtnList.Add(-1);
        }

        // {(100,1), (200,2)}
        List<Vector2Int> renewalData = new();
        // -1 1 1 2 1
        List<int> indexHistory_Renewal = new();
        // data foundation
        if (true)
        {
            if (true)
            {
                for (int i = 0; i < _childRange.x; i++)
                {
                    indexHistory_Renewal.Add(-1);
                }

                for (int i = _childRange.x; i < _childRange.y + 1; i++)
                {
                    indexHistory_Renewal.Add(setIndex_byFloor(_eventData[i]));
                }
                for (int i = _childRange.y+1; i < _eventData.Count; i++)
                {
                    indexHistory_Renewal.Add(-1);
                }

                // data to 100 -> 2 / 200 -> 1
                int setIndex_byFloor(int _index)
                {
                    for (int i = 0; i < renewalData.Count; i++)
                    {
                        if (_index == renewalData[i].x)
                        {
                            renewalData[i] += new Vector2Int(0, 1);
                            return renewalData[i].y;
                        }
                    }

                    renewalData.Add(new Vector2Int(_index, 1));
                    return 1;
                }
            }

        }

        // data deatailed
        if (true)
        {
            // fill up until to random
            for (int i = 0; i < renewalData.Count; i++)
            {
                List<int> currList = getStateByCurr(renewalData[i].x, renewalData[i].y);
                suffleList(currList);
                applyList_toEventList(currList, renewalData[i].x);
            }

            // return value by currState    V2(200,2) -> List(203, 201)
            List<int> getStateByCurr(int _floor,int _count)
            {
                List<int> rtn = new();

                List<Vector2Int> _list = new();
                int sum = 0;

                // TODO
                for (int i = 0; i < _count; i++)
                {
                    int targetIndex = getData_byCurrData(_floor);
                    rtn.Add(targetIndex);
                    applyData(targetIndex);
                }

                return rtn;

      
                //TODO!!! <<<<<<<<<<<<<<<
                int getData_byCurrData(int floorIndex)
                {
                    List<_EventCoef> _rtn = getAvailableeEvent();
                    List<_EventCoef> newSet = new();

                    for (int i = 0; i < _rtn.Count; i++)
                    {
                        int _targetEventIndex = _rtn[i].EventIndex; int _o;
                        if (currIncountState.TryGetValue(_targetEventIndex,out _o) == false)
                        {
                            newSet.Add(_rtn[i]);
                        }
                    }
                    _EventCoef target = getEvent_byRandom(newSet);
                    if (target == null)
                    {
                        Debug.Log("so sad");
                        return _floor;
                    }

                    return getEvent_byRandom(newSet).EventIndex;

                    List<_EventCoef> getAvailableeEvent()
                    {
                        List<_EventCoef> _rtn = new();
                        foreach (KeyValuePair<int, _EventCoef> item in EventDataSet)
                        {
                            if (item.Key - (item.Key % 100) == floorIndex)
                                _rtn.Add(item.Value);
                        }

                        return _rtn;
                    }

                    _EventCoef getEvent_byRandom(List<_EventCoef> target)
                    {
                        if (target.Count == 0)
                        {
                            return null;
                        }

                        int randomInt = UnityEngine.Random.Range(0, target.Count);
                        return target[randomInt];
                    }
                }

                void applyData(int eventIndex)
                {
                    // 세부 이벤트 결정
                    if (true)
                    {
                        int temp2 = eventIndex; int temp3;

                        // Add Event, 각각의 자식 노드들의 이벤트와 상호작용, 

                        //interact with dict
                        if (true)
                        {
                            if (currIncountState.TryGetValue(temp2, out temp3) == false)
                            {
                                // case : data is null
                                currIncountState.Add(temp2, 0); temp3 = 0;
                                currIncountState_ToShow.Add(new Vector2Int(temp2, 0));
                            }

                            currIncountState[temp2] = focusingPosV2.x + getCoef_Gap_onIgnore(temp2);
                        }

                        //interact with list
                        if (true)
                        {
                            int value_Dict = currIncountState[temp2];
                            for (int i = 0; i < currIncountState_ToShow.Count; i++)
                            {
                                if (currIncountState_ToShow[i].x == temp2)
                                {
                                    currIncountState_ToShow[i] = new Vector2Int(temp2, value_Dict);
                                    break;
                                }
                            }
                        }
                    }
                } //applyData
            }

            // suffle List each together    List(203, 201) -> List(201, 203)
            void suffleList(List<int> _list)
            {
                if (_list.Count == 1)
                    return;

                for (int i = 0; i < _list.Count; i++)
                {
                    //each swap
                    if (true)
                    {
                        int target = UnityEngine.Random.Range(0, _list.Count);

                        int cash = _list[i];
                        _list[i] = _list[target];
                        _list[target] = cash;
                    }
                }
                return;
            }

            // apply value by currState    List(203, 201)  rnw ( 0 1 0 0 2 ) -> ( -1 ,203 ,-1 ,-1 ,201 )
            void applyList_toEventList(List<int> _oddsList, int _floor)
            {
                for (int i = 0; i < _eventData.Count; i++)
                {
                    if (indexHistory_Renewal[i] > 0 && _eventData[i] == _floor)
                    {
                        int _index = indexHistory_Renewal[i] - 1;
                        rtnList[i] = _oddsList[_index];
                    } 
                }
                return;
            }
        }

        return rtnList;
    }

    protected float getCoef_Aging(int _eventIndex)
    {
        _EventCoef _value;
        if(EventDataSet.TryGetValue(_eventIndex, out _value) == false)
        {
            Debug.Log(_eventIndex + " << error");
            return 0;
        }

        return _value._Aging;
    }


    protected int getCoef_Gap_onIgnore(int _eventIndex)
    {
        _EventCoef _value;
        EventDataSet.TryGetValue(_eventIndex, out _value);
        return _value._Gap_onIgnore;
    }
}

[Serializable]
public class _EventCoef
{
    public int EventIndex;
    public float _Aging;
    public int _Gap_onFocused;
    public int _Gap_onIgnore;
    public int coef_3;

    public _EventCoef(int _EventIndex)
    {
        EventIndex = _EventIndex;
    }
}