using System.Collections.Generic;
using UnityEngine;
using Views.Blocks;

namespace Views
{
    public class View : UIPanel
    {
        private Dictionary<string, Block> _blocks;

        public void Initialize()
        {
            ClosePanelImmediately();
        }
        
        public void AddBlock(Block inBlock, bool replaceIfExists = false)
        {
            if(_blocks == null) _blocks = new Dictionary<string, Block>();
            if (HasBlock(inBlock.blockName) && replaceIfExists)
                _blocks[inBlock.blockName] = inBlock;
            else if(!HasBlock(inBlock.blockName))
                _blocks.Add(inBlock.blockName, inBlock);
        }

        public void RemoveBlock(Block inBlock)
        {
            if(inBlock == null || !HasBlock(inBlock.blockName)) return;
            _blocks.Remove(inBlock.blockName);
        }

        public bool HasBlock(string inBlockName)
        {
            return _blocks != null && _blocks.ContainsKey(inBlockName);
        }

        public void Search(string inPrefix)
        {
            
        }
    }
}