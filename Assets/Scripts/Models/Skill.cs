using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class Skill: Model,IHaveTextField, IHaveColorField, IHaveNumericField, IHaveEnabledField
    {
        public bool enabled;
        public string title;
        public string description;
        public string iconLink;
        public float rating;
        public DBColor color;
        
        public bool IsHave(string inString)
        {
            if (title.Contains(inString)) return true;
            if (description.Contains(inString)) return true;
            if (iconLink.Contains(inString)) return true;
            return false;
        }

        public bool IsHave(Color inColor)
        {
            return color?.ToColor() == inColor;
        }

        public bool IsHave(float inNumeric)
        {
            return rating.Equals(inNumeric);
        }

        public bool IsEnabled => enabled;
    }
    
    [System.Serializable]
    public class RootSkill : RootModel
    {
        public List<Skill> skills;

        public override List<T> GetModels<T>()
        {
            return skills as List<T>;
        }
    }
}