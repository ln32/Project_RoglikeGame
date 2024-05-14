using EnumCollection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LobbyCollection
{
    public class UpgradeClass
    {
        public Dictionary<Language, string> name;
        public int index;
        public List<UpgradeContent> content;
        public Dictionary<Language, string> explain;
        public Dictionary<Language, string> info;
        public UpgradeEffectType type;
        public string lobbyCase;
        public UpgradeClass(Dictionary<Language, string> _name, int _index, List<UpgradeContent> _content, Dictionary<Language, string> _explain, Dictionary<Language, string> _info, UpgradeEffectType _type, string _lobbyCase)
        {
            name = _name;
            index = _index;
            content = _content;
            explain = _explain;
            info = _info;
            type = _type;
            lobbyCase = _lobbyCase;
        }
    }
    public struct TalentEffect
    {
        public float value;
        public EffectType type;
        public TalentEffect(float _value, EffectType _type)
        {
            value = _value;
            type = _type;
        }
    }
    public struct UpgradeContent
    {
        public float value;
        public int price;
        public UpgradeContent(float _value, int _price)
        {
            value = _value;
            price = _price;
        }
    }
}