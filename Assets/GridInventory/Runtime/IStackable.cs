using UnityEngine;

namespace GridInventory
{
    public interface IStackable
    {
        public int MaxStackSize { get; }
        public int StackCount { get; set; }
        public bool CanStackWith(IGridItem otherItem);
    }

}

