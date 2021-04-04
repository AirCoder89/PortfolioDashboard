

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ListToPopupAttribute: PropertyAttribute
{
    public Type targetType;
    public string propertyName;
    
    public ListToPopupAttribute(Type inType, string inName)
    {
        targetType = inType;
        propertyName = inName;
    }
}

[CustomPropertyDrawer(typeof(ListToPopupAttribute))]
public class ListToPopupDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var atb = attribute as ListToPopupAttribute;
        List<string> list = null;

        if (atb.targetType.GetField(atb.propertyName) != null)
        {
            list = atb.targetType.GetField(atb.propertyName).GetValue(atb.targetType) as List<string>;
        }

        if (list != null && list.Count != 0)
        {
            var index = Mathf.Max(list.IndexOf(property.stringValue), 0); //save
            index = EditorGUI.Popup(position, property.name, index, list.ToArray());
            property.stringValue = list[index];
        }
        else EditorGUI.PropertyField(position, property, label);
    }
}