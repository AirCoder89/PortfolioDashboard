using System.Collections.Generic;
using System.Linq;
using AirCoder.ItemsMaker.Items;
using Models;
using PathologicalGames;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace AirCoder.ItemsMaker
{
    [RequireComponent(typeof(SpawnPool))]
    public class ItemsMaker : MonoBehaviour
    {
        [SerializeField] private string emptyItemName;
        [SerializeField] private string prefix = "\"skills\": [";
        [SerializeField] private string suffix = "]";
        [SerializeField] private Inspector.Inspector inspector;
        [SerializeField] private Transform holder;
        [SerializeField][TextArea(25, 5000)] private string jsonTxt;
        
        private HashSet<Item> _items;
        private Item _selectedItem;
        private ContentSizeFitter _sizeFitter;
        private SpawnPool _pool;

        [BoxGroup("Testing")] public List<Skill> testSkills;
        [BoxGroup("Testing")] public List<Reward> testRewards;
        

        public SpawnPool Pool
        {
            get
            {
                if (_pool == null) _pool = GetComponent<SpawnPool>();
                return _pool;
            }
        }
        
        private void Start()
        {
            Initialize();
        }

        

        public void Initialize()
        {
            _sizeFitter = holder.GetComponent<ContentSizeFitter>();
            inspector.Initialize(this);
            Item.OnInspect += InspectItem;
            
            /*_items = new HashSet<Item>();
            foreach (Transform trItem in holder)
            {
                var item = trItem.gameObject.GetComponent<Item>();
                if(item == null) continue;
                _items.Add(item);
                item.Initialize(this);
            }

            _sizeFitter.enabled = _items.Count > 2;*/
        }

        
        private void InspectItem(Item inItem)
        {
            if (_selectedItem != null)  _selectedItem.Unselect();
            _selectedItem = inItem;
            _selectedItem.Select();
            inspector.Inspect(_selectedItem);
        }

        /*{
           "skills": [
           {
               "title": "Skill Title",
               "description": "Skill description here",
               "iconLink": "www.iconlinkhere.com"
           },
           {
               "title": "Skill 2 Title",
               "description": "Skill 2 description here",
               "iconLink": "www.icon22linkhere.com"
           }
           ]
       }*/
        
        [Button("Skills", ButtonSizes.Large)]
        private void TestingSkills()
        {
           Generate(testSkills);
        }
        
         [Button("Rewards", ButtonSizes.Large)]
        private void TestingRewards()
        {
           Generate(testRewards);
        }
        
        public void Generate<T>(List<T> inSkills)
        {
            ClearItems();
            foreach (var skill in inSkills)
            {
                var fields = typeof(T).GetFields();
                var item = GetEmptyItem();
                item.Initialize(this, typeof(T).Name);
                
                fields.ForEach(field =>
                {
                    var fieldType = field.FieldType;
                    var fieldLabel = field.Name;
                    var fieldValue = field.GetValue(skill);
                    item.AddField(fieldLabel, fieldValue, fieldType);
                });
                
                item.UpdateSize();
                _items.Add(item);
            }
            _sizeFitter.enabled = true;
        }

        private void ClearItems()
        {
            if (_items == null)
            {
                _items = new HashSet<Item>();
                return;
            }

            _sizeFitter.enabled = false;
            foreach (var item in _items)
            {
                item.ResetItem();
                Pool.Despawn(item.transform, Pool.transform);
            }
            _items.Clear();
        }

        private Item GetEmptyItem()
        {
            return Pool.Spawn(this.emptyItemName, holder).gameObject
                .GetComponent<Item>();
        }

        [Button("ToJsonTest", ButtonSizes.Large)]
        private void ToJsonTest()
        {
            jsonTxt = ToJson();
            Debug.Log($"{ToJson()}");
        }
        public string ToJson()
        {
            var json = "{\n" + prefix + "\n";
            for (var i = 0; i < _items.Count; i++)
            {
                json += _items.ElementAt(i).ToJson();
                if (i < (_items.Count - 1)) json += ",\n";
            }
            json += "\n" + suffix + "\n}";
            return json;
        }
    }
}