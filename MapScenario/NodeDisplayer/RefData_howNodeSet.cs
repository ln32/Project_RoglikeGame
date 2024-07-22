using System;
using System.Collections.Generic;
using UnityEngine;

public class RefData_howNodeSet : MonoBehaviour
{
    [SerializeField] internal List<SettingDataUnit> mapSettingDataSet= new List<SettingDataUnit>();
    [SerializeField] internal BgSet_byStage[] bgPrefabList;
    [SerializeField] internal Vector2Int myStd;
        
   internal GameObject GetDataSet_byStage(int _value)
    {
        return GetMapData_byStage(_value).bgPrefabList.GetGameObj_byRandom();
    }

    internal SettingDataUnit GetMapData_byStage(int _value)
    {
        if (_value == -1)
            return mapSettingDataSet[mapSettingDataSet.Count-1];
        else if (_value < myStd.x)
        {
            return mapSettingDataSet[0];
        }
        else if (_value < myStd.y)
        {
            return mapSettingDataSet[1];
        }
        else
        {
            return mapSettingDataSet[2];
        }
    } 
}

[Serializable]
internal class SettingDataUnit
{
    public string name;
    public NodeCase _nodeCase;
    public int index;
    [SerializeField] internal BgSet_byStage bgPrefabList;
}

enum NodeCase
{
    Plain,       Forest,    Ruin,
    Beach ,      Swamp,     IceField,
    Cave,        Desert,    Lava
}

[Serializable]
internal class BgSet_byStage
{
    public int typeIndex;
    public GameObject[] bgPrefabList;
    public int[] ratioList;

    public GameObject GetGameObj_byRandom()
    {
        int _rtn = 0;
        int _sum = 0;
        for (int i = 0; i < ratioList.Length; i++)
        {
            _sum += ratioList[i];
        }

        int _rnd = UnityEngine.Random.Range(0, _sum);

        for (int i = 0; i < ratioList.Length; i++)
        {
            _rnd -= ratioList[i];

            if (_rnd <= 0)
            {
                _rtn = i;
                break;
            }
        }

        return bgPrefabList[_rtn];
    }
}
