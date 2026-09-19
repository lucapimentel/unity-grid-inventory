using System;
using NUnit.Framework;
using UnityEngine;
using GridInventory;
using Unity.Mathematics;
public class GridItemDefTests
{
    private sealed class FakeItem : IGridItem
    {
        public GridItemDef Def { get; }
        public FakeItem(GridItemDef def) => Def = def;
    }

    [Test]
    public void Constructor_values()
    {
        var def = new GridItemDef(new Vector2Int(2, 3), null, "weapon");
        Assert.That(def.Footprint, Is.EqualTo(new Vector2Int(2, 3)));
        Assert.That(def.Icon, Is.Null);
        Assert.That(def.SlotTag, Is.EqualTo("weapon"));
    }

    [TestCase(0, 1)]
    [TestCase(1, 0)]
    [TestCase(-1, 2)]
    public void Constructor_Negative_Footprints(int x, int y)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GridItemDef(new Vector2Int(x, y), null, "weapon"));
    }

    [Test]
    public void SlotTag_BuyerTag()
    {
        var def = new GridItemDef(Vector2Int.one, null, "Charm");
        Assert.That(def.SlotTag, Is.EqualTo("Charm"));
    }

    [Test]
    public void IGridItem_IsImplementableWithOneMember()
    {
        IGridItem testItem = new FakeItem(new GridItemDef(new Vector2Int(1, 3), null, "staff"));
        Assert.That(testItem.Def.Footprint, Is.EqualTo(new Vector2Int(1, 3)));
    }
}
