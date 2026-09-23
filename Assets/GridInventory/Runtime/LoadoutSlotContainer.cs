using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridInventory
{
    public class LoadoutSlotContainer : IItemContainer
    {
        public readonly LoadoutLayoutDefinition Definition;
        public IGridItem EquippedItem { get; private set; }
        private static readonly Vector2Int SlotOrigin = new Vector2Int(0, 0);

        public LoadoutSlotContainer(LoadoutLayoutDefinition definition)
        {
            Definition = definition;
        }
        public List<IGridItem> BlockersAt(IGridItem item, Vector2Int origin)
        {
            var blockers = new List<IGridItem>();
            if (EquippedItem != null) blockers.Add(EquippedItem);

            return blockers;
        }

        public Vector2Int? OriginOf(IGridItem item)
        {
            if (item == null || EquippedItem != item) return null;

            return SlotOrigin;
        }

        public bool RemoveItem(IGridItem item)
        {
            if (item == null || EquippedItem != item) return false;

            EquippedItem = null;
            return true;
        }

        public bool TryPlaceItem(IGridItem item, Vector2Int origin)
        {
            if (item == null) return false;
            if (EquippedItem != null) return false;

            if (item.Def.SlotTag != Definition.RequiredSlotTag) return false;

            EquippedItem = item;
            return true;
        }
    }
}

