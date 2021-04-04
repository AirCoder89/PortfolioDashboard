using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class DBColor
    {
        public float r;
        public float g;
        public float b;
        
        public Color ToColor()
            => new Color(r,g,b,1f);

        public DBColor()
        {
        }

        public DBColor(Color inColor)
        {
            r = inColor.r;
            g = inColor.g;
            b = inColor.b;
        }

        public string ToJson()
        {
            return "{" + 
                   
                   $"\"r\":{r}, " +
                   $"\"g\":{g}, " +
                   $"\"b\":{b}" 
                   
                   + "}";
        }
        
    }
}