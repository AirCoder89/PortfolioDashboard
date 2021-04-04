using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using PathologicalGames;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Theme;
using UnityEngine;
using UnityEngine.UI;
using Views.Blocks;
using Views.Blocks.Fields;
using Newtonsoft.Json;
[RequireComponent(typeof(SpawnPool))]
public class Dashboard : MonoBehaviour
{
    [SerializeField][Required] private DashboardSettings settings;
    [SerializeField][Required] private Menu.Menu menu;
    [SerializeField] private string emptyBlockName;
    [SerializeField] private string prefix = "\"skills\": [";
    [SerializeField] private string suffix = "]";
    [SerializeField] private ThemeInfo targetTheme;
    [SerializeField] private Transform holder;
    //[SerializeField][TextArea(25, 5000)] private string jsonTxt;
    
    private HashSet<Block> _blocks;
    private ContentSizeFitter _sizeFitter;
    private SpawnPool _p;
    public SpawnPool Pool
    {
        get
        {
            if (_p == null) _p = GetComponent<SpawnPool>();
            return _p;
        }
    }
    public DashboardSettings Settings => settings;

    public ThemeManager themeManager { get; private set; }
    void Start()
    {
        themeManager = new ThemeManager(targetTheme);
        _sizeFitter = holder.GetComponent<ContentSizeFitter>();
    }

    [Button("Start", ButtonSizes.Large)]
    private void StartDashboard()
    {
        menu.Initialize(this);
        menu.AddEntry<RootSkill, Skill>("Skills","Skills");
        menu.AddEntry<RootSkill, Skill>("Skills","Skills");
        menu.AddEntry<RootSkill, Skill>("Skills","Skills");
    }
    
    [Button("UpdateTheme", ButtonSizes.Large)]
    private void UpdateTheme()
    {
        targetTheme.UpdateColors();
        themeManager?.ChangeTheme(targetTheme);
    }
    
    public void EvaluateRoot<T>(RootModel inRoot) where T : Model
    {
        if(inRoot?.GetModels<T>() == null) throw new NullReferenceException();
        Generate(inRoot.GetModels<T>());
    }
    
    private void Generate<T>(IEnumerable<T> inEntries) where T : Model
    {
        ClearItems();
        foreach (var entry in inEntries)
        {
            var fields = typeof(T).GetFields();
            var block = GetEmptyBlock();
            block.Initialize(Pool, typeof(T).Name, entry);
            foreach (var field in fields)
            {
                var fieldType = field.FieldType;
                var fieldLabel = field.Name;
                var fieldValue = field.GetValue(entry);
                var isNullable = Nullable.GetUnderlyingType(fieldType) != null;
                Debug.Log($"Type : {fieldType.Name} - isNullable : {isNullable} ");
                if(isNullable && fieldValue == null){
                    Debug.Log($"Skip!");
                    continue;}
                block.AddField(fieldLabel, fieldValue, fieldType);
            }
            
            block.UpdateSize();
            _blocks.Add(block);
        }
        _sizeFitter.enabled = true;
    }
    
    private void ClearItems()
    {
        if (_blocks == null)
        {
            _blocks = new HashSet<Block>();
            return;
        }

        _sizeFitter.enabled = false;
        foreach (var block in _blocks)
        {
            block.ResetItem();
            Pool.Despawn(block.transform, Pool.transform);
        }
        _blocks.Clear();
    }

    private Block GetEmptyBlock()
    {
        return Pool.Spawn(this.emptyBlockName, holder).gameObject
            .GetComponent<Block>();
    }

}
