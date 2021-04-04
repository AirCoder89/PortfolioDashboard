using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Theme
{
    [CreateAssetMenu(menuName = "Dashboard/new theme")]
    public class ThemeInfo: ScriptableObject
    {
        [SerializeField][TableList] private List<ColorInfo> colors;

        private Dictionary<ThemeColor, Color> _map;
        private Dictionary<ThemeColor, Color> _colorMap
        {
            get
            {
                if (_map == null)
                {
                    if(colors == null) throw new NullReferenceException($"Theme colors list must be not null !");
                    _map = new Dictionary<ThemeColor, Color>();
                    foreach (var colorInfo in colors)
                        _map.Add(colorInfo.colorLabel, colorInfo.color);
                }
                return _map;
            }
        }

        public Color GetColor(ThemeColor inColor)
        {
            if(!_colorMap.ContainsKey(inColor)) throw new Exception($"Theme color not found {inColor}");
            return _colorMap[inColor];
        }

        public void UpdateColors()
        {
            _map = null;
        }
    }
}