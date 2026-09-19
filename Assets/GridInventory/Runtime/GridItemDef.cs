using UnityEngine;
using System;

namespace GridInventory
{
    public readonly struct GridItemDef
    {
        public readonly Vector2Int Footprint;
        public readonly Sprite Icon;
        public readonly string SlotTag;

        public GridItemDef(Vector2Int footprint, Sprite icon, string slotTag)
        {
            if (footprint.x < 1 || footprint.y < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(footprint), "footprint missing");
            }

            Footprint = footprint;
            Icon = icon;
            SlotTag = slotTag;
        }
    }

    public interface IGridItem
    {
        GridItemDef Def { get; }
    }
}
