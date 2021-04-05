using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
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
using Views.SearchDashboard;

[RequireComponent(typeof(SpawnPool))]
public class Dashboard : MonoBehaviour
{
    [SerializeField][Required] private DashboardSettings settings;
    [SerializeField][Required] private Menu.Menu menu;
    [SerializeField][Required] private Searching seraching;
    [SerializeField] private string emptyBlockName;
    [SerializeField] private string prefix = "\"skills\": [";
    [SerializeField] private string suffix = "]";
    [SerializeField] private ThemeInfo targetTheme;
    [SerializeField] private Transform holder;
    //[SerializeField][TextArea(25, 5000)] private string jsonTxt;
    
    
    private FilterType _filters;
    private bool _assignFilter;
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
    private void Start()
    {
        Searching.OnSearch += OnSearch;
        themeManager = new ThemeManager(targetTheme);
        _sizeFitter = holder.GetComponent<ContentSizeFitter>();
    }

    private void OnSearch(FilterType inFilters, string inString)
    {
        
    }

    [Button("Start", ButtonSizes.Large)]
    private void StartDashboard()
    {
        seraching.Initialize(this);
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
        ClearFilters();
        ClearBlocks();
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

                AssignFilters(fieldType);
            }
            
            block.UpdateSize();
            _blocks.Add(block);
        }
        seraching.BindData(_filters);
        _sizeFitter.enabled = true;
    }

    private void ClearFilters()
    {
        seraching.Clear();
        _filters = FilterType.None;
        _assignFilter = true;
    }
    
    private void AssignFilters(Type inType)
    {
        if(!_assignFilter) return;
        _assignFilter = false;
        
        var interfaces = inType.GetInterfaces();
        foreach (var filterInterface in interfaces)
        {
            if (filterInterface == typeof(IHaveColorField)) _filters |= FilterType.Color;
            if (filterInterface == typeof(IHaveNumericField)) _filters |= FilterType.Numeric;
            if (filterInterface == typeof(IHaveTextField)) _filters |= FilterType.String;
            if (filterInterface == typeof(IHaveEnabledField)) _filters |= FilterType.Enabled;
        }
    }

    private void ClearBlocks()
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
