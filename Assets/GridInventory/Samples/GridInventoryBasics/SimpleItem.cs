using UnityEngine;

namespace GridInventory.Samples
{
    [CreateAssetMenu(menuName = "Grid Inventory/Sample/Simple Item")]
    public class SimpleItem : ScriptableObject, IGridItem
    {
        public Vector2Int Footprint = Vector2Int.one;
        public Sprite Icon;
        public string SlotTag;
        public string DisplayName;
        public GridItemDef Def => new GridItemDef(Footprint, Icon, SlotTag);
    }
}

