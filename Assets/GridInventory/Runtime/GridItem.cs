namespace GridInventory
{
    public class GridItem<TPayload> : IGridItem
    {
        public GridItemDef Def { get; }
        public TPayload Payload { get; }

        public GridItem(GridItemDef def, TPayload payload)
        {
            Def = def;
            Payload = payload;
        }
    }

}
