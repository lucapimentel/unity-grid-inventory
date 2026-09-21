using System;
using System.Linq;
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

            var blockers = destination.BlockersAt(item, targetOrigin);
            if (blockers.Count > 1)
            {
                return false;
            }
            // merge stack
            var blocker = blockers.FirstOrDefault();
            if (item is IStackable stackableItem && blocker is IStackable stackableBlocker && stackableItem.CanStackWith(blocker))
            {
                var stackDiff = stackableBlocker.MaxStackSize - stackableBlocker.StackCount;
                var valueToTransfer = Math.Min(stackDiff, stackableItem.StackCount);

                if (valueToTransfer <= 0) return false;

                stackableBlocker.StackCount += valueToTransfer;
                stackableItem.StackCount -= valueToTransfer;

                if (stackableItem.StackCount == 0)
                {
                    source.RemoveItem(item);
                }
                return true;
            }

            if (blocker != null)
            {
                //swap
                var blockerOrigin = destination.OriginOf(blocker).Value;

                // item not removed
                if (!source.RemoveItem(item)) return false;

                if (!destination.RemoveItem(blocker))
                {
                    source.TryPlaceItem(item, itemOriginalOrigin.Value); // undo first step
                    return false;
                }

                if (!destination.TryPlaceItem(item, targetOrigin))
                {
                    destination.TryPlaceItem(blocker, blockerOrigin);
                    source.TryPlaceItem(item, itemOriginalOrigin.Value);
                    return false;
                }

                if (!source.TryPlaceItem(blocker, itemOriginalOrigin.Value))
                {
                    destination.RemoveItem(item);
                    destination.TryPlaceItem(blocker, blockerOrigin);
                    source.TryPlaceItem(item, itemOriginalOrigin.Value);
                    return false;
                }

                return true;
            }

            if (!source.RemoveItem(item)) return false;

            if (destination.TryPlaceItem(item, targetOrigin)) return true;

            var rollbackSucceeded = source.TryPlaceItem(item, itemOriginalOrigin.Value);

            if (!rollbackSucceeded)
            {
                throw new InvalidOperationException($"move failed item tag: {item.Def.SlotTag}, footprint {item.Def.Footprint} could not be restored into {itemOriginalOrigin.Value} where it was placed in the source container");
            }

            return false;

        }
    }
}
