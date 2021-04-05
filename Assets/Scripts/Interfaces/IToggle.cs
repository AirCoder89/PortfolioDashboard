using System;
using UnityEngine;

namespace Interfaces
{
    public interface IToggle
    {
        event Action<bool> onValueChanged; 
        bool IsSelected { get; }
        void SetColors(Color selectedColor, Color unselectedColor);
    }
}