using GridInventory;
using NUnit.Framework;
using UnityEngine;
public class GridItemWrapperTests
{

    private sealed class GenericSword { }
    private struct GenericCoinStack
    {
        public int Count;
    }

    [Test]
    public void GridItem_TwoWrappersSamePayload_AreDistinctItems()
    {
        var gridA = new ItemGrid(4, 4);

        var def = new GridItemDef(new Vector2Int(1, 1), null, "weapon");
        var sword = new GenericSword();

        var wrapperA = new GridItem<GenericSword>(def, sword);
        var wrapperB = new GridItem<GenericSword>(def, sword);

        Assert.That(gridA.TryPlaceItem(wrapperA, new Vector2Int(0, 0)), Is.True);
        Assert.That(gridA.TryPlaceItem(wrapperB, new Vector2Int(1, 0)), Is.True);

        Assert.That(gridA.OriginOf(wrapperA), Is.EqualTo(new Vector2Int(0, 0)));
        Assert.That(gridA.OriginOf(wrapperB), Is.EqualTo(new Vector2Int(1, 0)));
    }

    [Test]
    public void Move_WrappedItem_IdentitySurvivesFullCycle()
    {
        var gridA = new ItemGrid(4, 4);
        var gridB = new ItemGrid(4, 4);

        var def = new GridItemDef(new Vector2Int(2, 2), null, "weapon");
        var sword = new GenericSword();

        var wrapperA = new GridItem<GenericSword>(def, sword);
        Assert.That(gridA.TryPlaceItem(wrapperA, new Vector2Int(0, 0)), Is.True);
        Assert.That(InventoryMove.Move(gridA, wrapperA, gridB, new Vector2Int(2, 2)), Is.True);
        Assert.That(InventoryMove.Move(gridB, wrapperA, gridA, new Vector2Int(1, 1)), Is.True);

        Assert.That(gridA.OriginOf(wrapperA), Is.EqualTo(new Vector2Int(1, 1)));
        Assert.That(gridB.OriginOf(wrapperA), Is.Null);
        Assert.That(wrapperA.Payload, Is.SameAs(sword));
    }

    [Test]
    public void GridItem_StructPayload_ReadsBackUnchanged()
    {
        var def = new GridItemDef(new Vector2Int(2, 2), null, "bag");
        var coinStack = new GenericCoinStack { Count = 12 };

        var coinsWrapper = new GridItem<GenericCoinStack>(def, coinStack);
        Assert.That(coinsWrapper.Payload.Count, Is.EqualTo(12));
    }
}
