using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GridInventory
{
    public class ItemGrid : IItemContainer
    {
        public readonly int Width, Height;
        private readonly IGridItem[,] Occupancy;
        private readonly Dictionary<IGridItem, Vector2Int> Placements;

        public ItemGrid(int width, int height)
        {
            if (width < 1 || height < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "out of range");
            }

            Width = width;
            Height = height;
            Occupancy = new IGridItem[width, height];
            Placements = new Dictionary<IGridItem, Vector2Int>();
        }

        public IEnumerable<Vector2Int> CellsCovered(Vector2Int footprint, Vector2Int origin)
        {
            for (int dx = 0; dx < footprint.x; dx++)
            {
                for (int dy = 0; dy < footprint.y; dy++)
                {
                    yield return new Vector2Int(origin.x + dx, origin.y + dy);
                }
            }
        }

        public bool DoesItemFit(IGridItem item, Vector2Int origin)
        {
            return DoesItemFit(item, origin, null);
        }

        public bool DoesItemFit(IGridItem item, Vector2Int origin, IGridItem ignore)
        {
            var currentItemFootPrint = item.Def.Footprint;

            if (currentItemFootPrint.x < 1 || currentItemFootPrint.y < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(currentItemFootPrint), "");
            }

            foreach (var cell in CellsCovered(currentItemFootPrint, origin))
            {
                // out of bounds
                if (cell.x < 0 || cell.y < 0)
                {
                    return false;
                }
                // out of bounds
                if (cell.x >= Width || cell.y >= Height)
                {
                    return false;
                }

                var occupant = Occupancy[cell.x, cell.y];

                if (occupant != null && occupant != ignore) // cell has something in it and the one beign treated as empty 
                {
                    // 1x3 staff at (0,0) covers (0,0) (0,1) (0,2). Move it to (0,1) -> new cells (0,1) (0,2) (0,3).
                    //  Two of those are occupied by the staff itself.
                    // ignore would ignore the staff cells.
                    return false;
                }
            }

            return true;
        }

        public Vector2Int? OriginOf(IGridItem item)
        {
            if (item == null)
            {
                return null;
            }

            if (Placements.TryGetValue(item, out var origin)) // check on dictionary for the item if exists return the origin vector
            {
                return origin;
            }

            return null;
        }

        public IGridItem ItemAt(Vector2Int origin)
        {
            // out of bounds
            if (origin.x < 0 || origin.y < 0)
            {
                return null;
            }
            // out of bounds
            if (origin.x >= Width || origin.y >= Height)
            {
                return null;
            }

            return Occupancy[origin.x, origin.y];
        }

        public List<IGridItem> BlockersAt(IGridItem item, Vector2Int origin)
        {
            List<IGridItem> blockers = new List<IGridItem>();
            foreach (var cell in CellsCovered(item.Def.Footprint, origin))
            {
                // out of bounds
                if (cell.x < 0 || cell.y < 0)
                {
                    continue;
                }
                // out of bounds
                if (cell.x >= Width || cell.y >= Height)
                {
                    continue;
                }

                var occupantItem = ItemAt(cell);

                if (occupantItem != null && occupantItem != item & !blockers.Contains(occupantItem))
                {
                    blockers.Add(occupantItem);
                }
            }
            return blockers;
        }

        public bool TryPlaceItem(IGridItem item, Vector2Int origin)
        {
            if (item == null)
            {
                return false;
            }

            if (Placements.ContainsKey(item)) // item is already on the inventory grid.
            {
                return false;
            }

            if (!DoesItemFit(item, origin))
            {
                return false;
            }

            foreach (var cell in CellsCovered(item.Def.Footprint, origin))
            {
                Occupancy[cell.x, cell.y] = item;
            }

            Placements.Add(item, origin);
            return true;
        }

        public bool RemoveItem(IGridItem item)
        {

            if (item == null || !Placements.TryGetValue(item, out var origin))
            {
                return false; // not on grid -> nothing to remove
            }

            foreach (var cell in CellsCovered(item.Def.Footprint, origin)) // clear the cells of that item
            {
                Occupancy[cell.x, cell.y] = null;
            }

            Placements.Remove(item); // remove from the dictionary
            return true;
        }

        public bool TryAutoPlace(IGridItem item) // item when picked up needs to be placed on the grid, needs to check first
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (TryPlaceItem(item, new Vector2Int(x, y)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}