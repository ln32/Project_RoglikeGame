using EnumCollection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterCollection
{
    public struct TalentFormStruct
    {
        public Dictionary<Language, string> name;
        public Dictionary<Language, string> explain;
        public int level;
        public int order;
        public List<TalentEffectForm> effects;
        public TalentFormStruct(Dictionary<Language, string> _name, int _level, Dictionary<Language, string> _explain, List<TalentEffectForm> _effects, int _order)
        {
            effects = _effects;
            name = _name;
            explain = _explain;
            level = _level;
            order = _order;
        }
    }
    public struct TalentEffectForm
    {
        public string value;
        public EffectType type;
        public TalentEffectForm(string _value, EffectType _type)
        {
            value = _value;
            type = _type;
        }
    }
    public struct TalentStruct
    {
        public int level;
        public Dictionary<Language, string> name;
        public Dictionary<Language, string> explain;
        public List<TalentEffect> effects;
        public TalentStruct(Dictionary<Language, string> _name, int _level, Dictionary<Language, string> _explain, List<TalentEffect> _effects)
        {
            effects = _effects;
            name = _name;
            level = _level;
            explain = _explain;
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
    public class BodyPartClass
    {
        public Sprite head;
        public Sprite armL;
        public Sprite armR;
    }
    public class EyeClass
    {
        public Sprite front;
        public Sprite back;
    }
}