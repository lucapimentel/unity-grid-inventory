using GridInventory;
using NUnit.Framework;
using UnityEngine;

public class InventoryMoveTest
{
    private sealed class FakeItem : IGridItem
    {
        public GridItemDef Def { get; }
        public FakeItem(GridItemDef def) => Def = def;
    }

    private static IGridItem[,] ItemGridSnapshot(ItemGrid grid)
    {
        var currentGrid = new IGridItem[grid.Width, grid.Height];

        for (int dx = 0; dx < grid.Width; dx++)
        {
            for (int dy = 0; dy < grid.Height; dy++)
            {
                currentGrid[dx, dy] = grid.ItemAt(new Vector2Int(dx, dy));
            }
        }

        return currentGrid;
    }

    [Test]
    public void Move_ToEmptyGrid_TransfersItemAndEmptiesSource()
    {
        var gridA = new ItemGrid(2, 2);
        var gridB = new ItemGrid(2, 2);
        IGridItem testItem = new FakeItem(new GridItemDef(new Vector2Int(2, 2), null, "helmet"));

        Assert.That(gridA.TryPlaceItem(testItem, new Vector2Int(0, 0)), Is.True);
        Assert.That(InventoryMove.Move(gridA, testItem, gridB, new Vector2Int(0, 0)), Is.True);
        Assert.That(gridA.OriginOf(testItem), Is.Null);
        Assert.That(gridB.OriginOf(testItem), Is.EqualTo(new Vector2Int(0, 0)));
    }

    [Test]

    public void Move_DestinationFull_RestoresToOriginalOrigin()
    {
        var gridA = new ItemGrid(10, 10);
        var gridB = new ItemGrid(2, 2);
        IGridItem testItem1 = new FakeItem(new GridItemDef(new Vector2Int(2, 2), null, "helmet"));
        IGridItem testItem2 = new FakeItem(new GridItemDef(new Vector2Int(2, 2), null, "helmet"));

        Assert.That(gridA.TryPlaceItem(testItem1, new Vector2Int(3, 4)), Is.True);
        Assert.That(gridB.TryPlaceItem(testItem2, new Vector2Int(0, 0)), Is.True);

        Assert.That(InventoryMove.Move(gridA, testItem1, gridB, new Vector2Int(0, 0)), Is.False);
        Assert.That(gridA.OriginOf(testItem1), Is.EqualTo(new Vector2Int(3, 4)));
        Assert.That(gridB.OriginOf(testItem2), Is.EqualTo(new Vector2Int(0, 0)));
    }

    [Test]
    public void Move_DestinationFull_OrigingUntouched()
    {
        var gridA = new ItemGrid(10, 10);
        var gridB = new ItemGrid(2, 2);
        IGridItem testItem1 = new FakeItem(new GridItemDef(new Vector2Int(2, 2), null, "helmet"));
        IGridItem testItem2 = new FakeItem(new GridItemDef(new Vector2Int(2, 2), null, "helmet"));

        Assert.That(gridA.TryPlaceItem(testItem1, new Vector2Int(3, 4)), Is.True);
        Assert.That(gridB.TryPlaceItem(testItem2, new Vector2Int(0, 0)), Is.True);

        var beforeMoveSnapshotGridB = ItemGridSnapshot(gridB);
        Assert.That(InventoryMove.Move(gridA, testItem1, gridB, new Vector2Int(0, 0)), Is.False);
        var afterMoveSnapshotGridB = ItemGridSnapshot(gridB);

        Assert.That(beforeMoveSnapshotGridB, Is.EqualTo(afterMoveSnapshotGridB));
    }

}
