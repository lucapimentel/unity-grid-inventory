using System;
using NUnit.Framework;
using UnityEngine;
using GridInventory;
using Unity.Mathematics;
public class ItemGridTests
{
    private sealed class FakeItem : IGridItem
    {
        public GridItemDef Def { get; }
        public FakeItem(GridItemDef def) => Def = def;
    }

    [Test]
    public void PlaceTwice_SecondFails()
    {
        var grid = new ItemGrid(10, 10);
        IGridItem testItem = new FakeItem(new GridItemDef(new Vector2Int(1, 3), null, "staff"));
        Assert.That(grid.TryPlaceItem(testItem, new Vector2Int(0, 0)), Is.True);
        Assert.That(grid.TryPlaceItem(testItem, new Vector2Int(0, 0)), Is.False);
    }

    [Test]
    public void Really_Occupies_Space()
    {
        var grid = new ItemGrid(10, 10);
        IGridItem testItem = new FakeItem(new GridItemDef(new Vector2Int(2, 3), null, "shield"));
        IGridItem testItem2 = new FakeItem(new GridItemDef(new Vector2Int(2, 3), null, "shield"));
        Assert.That(grid.TryPlaceItem(testItem, new Vector2Int(0, 0)), Is.True);
        Assert.That(grid.TryPlaceItem(testItem2, new Vector2Int(1, 1)), Is.False);
        Assert.That(grid.TryPlaceItem(testItem2, new Vector2Int(1, 2)), Is.False);
        Assert.That(grid.TryPlaceItem(testItem2, new Vector2Int(2, 0)), Is.True);
    }

    [Test]
    public void Out_of_Bounds()
    {
        var grid = new ItemGrid(10, 10);
        IGridItem testItem = new FakeItem(new GridItemDef(new Vector2Int(2, 2), null, "helmet"));
        Assert.That(grid.TryPlaceItem(testItem, new Vector2Int(9, 0)), Is.False);
        Assert.That(grid.TryPlaceItem(testItem, new Vector2Int(0, 9)), Is.False);
        Assert.That(grid.TryPlaceItem(testItem, new Vector2Int(-1, 0)), Is.False);
        Assert.That(grid.TryPlaceItem(testItem, new Vector2Int(0, -1)), Is.False);
    }

    [Test]
    public void Overlap()
    {
        var grid = new ItemGrid(10, 10);
        IGridItem testItem = new FakeItem(new GridItemDef(new Vector2Int(2, 2), null, "helmet"));
        IGridItem testItem2 = new FakeItem(new GridItemDef(new Vector2Int(2, 2), null, "gloves"));
        Assert.That(grid.TryPlaceItem(testItem, new Vector2Int(0, 0)), Is.True);
        Assert.That(grid.TryPlaceItem(testItem2, new Vector2Int(1, 1)), Is.False);
        Assert.That(grid.TryPlaceItem(testItem2, new Vector2Int(2, 2)), Is.True);
    }

    [Test]
    public void Remove_Restore_Space()
    {
        var grid = new ItemGrid(10, 10);
        IGridItem testItem = new FakeItem(new GridItemDef(new Vector2Int(2, 2), null, "helmet"));
        IGridItem testItem2 = new FakeItem(new GridItemDef(new Vector2Int(2, 2), null, "gloves"));
        Assert.That(grid.TryPlaceItem(testItem, new Vector2Int(0, 0)), Is.True);
        grid.RemoveItem(testItem);
        Assert.That(grid.TryPlaceItem(testItem2, new Vector2Int(0, 0)), Is.True);

    }
}
