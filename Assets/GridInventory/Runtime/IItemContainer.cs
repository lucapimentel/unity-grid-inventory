using UnityEngine;

namespace GridInventory
{
    public interface IItemContainer
    {
        Vector2Int? OriginOf(IGridItem item);
        bool RemoveItem(IGridItem item);
        bool TryPlaceItem(IGridItem item, Vector2Int origin);
    }

}
