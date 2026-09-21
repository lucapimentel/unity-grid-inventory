using System;
using UnityEngine;

namespace GridInventory
{
    public class InventoryMove
    {
        public static bool Move(ItemGrid source, IGridItem item, ItemGrid destination, Vector2Int targetOrigin)
        {
            if (item == null || source == null || destination == null)
            {
                return false;
            }

            var itemOriginalOrigin = source.OriginOf(item);

            if (itemOriginalOrigin == null) // there is no item on the source
            {
                return false;
            }

            if (!source.RemoveItem(item)) // item not removed
            {
                return false;
            }

            if (destination.TryPlaceItem(item, targetOrigin)) // item placed
            {
                return true;
            }

            var rollbackSucceeded = source.TryPlaceItem(item, itemOriginalOrigin.Value);

            if (!rollbackSucceeded)
            {
                throw new InvalidOperationException($"move failed item tag: {item.Def.SlotTag}, footprint {item.Def.Footprint} could not be restored into {itemOriginalOrigin.Value} where it was placed in the source container");
            }

            return false;

        }
    }
}

