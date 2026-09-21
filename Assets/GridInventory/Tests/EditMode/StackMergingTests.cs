using GridInventory;
using NUnit.Framework;
using UnityEngine;

public class StackMergingTests
{
    private sealed class FakeItem : IGridItem, IStackable
    {
        public GridItemDef Def { get; }
        public int MaxStackSize { get; }
        public int StackCount { get; set; }
        public FakeItem(GridItemDef def, int maxStackSize, int stackCount)
        {
            Def = def;
            MaxStackSize = maxStackSize;
            StackCount = stackCount;
        }
        public bool CanStackWith(IGridItem otherItem)
        {
            return otherItem.Def.Equals(Def);
        }
    }

    [Test]
    public void Move_StackableIntoRoom_MergesFully()
    {
        var gridA = new ItemGrid(4, 4);
        var gridB = new ItemGrid(4, 4);

        var heldItem = new FakeItem(new GridItemDef(new Vector2Int(1, 1), null, "potion"), 10, 3);
        Assert.That(gridA.TryPlaceItem(heldItem, new Vector2Int(0, 0)), Is.True);

        var targetItem = new FakeItem(new GridItemDef(new Vector2Int(1, 1), null, "potion"), 10, 5);
        Assert.That(gridB.TryPlaceItem(targetItem, new Vector2Int(2, 2)), Is.True);

        Assert.That(InventoryMove.Move(gridA, heldItem, gridB, new Vector2Int(2, 2)), Is.True);

        Assert.That(targetItem.StackCount, Is.EqualTo(8));
        Assert.That(heldItem.StackCount, Is.EqualTo(0));

        Assert.That(gridA.OriginOf(heldItem), Is.Null);
        Assert.That(gridB.OriginOf(targetItem), Is.EqualTo(new Vector2Int(2, 2)));
    }

    [Test]
    public void Move_StackableIntoPartialRoom_LeavesRemainderAtOriginalOrigin()
    {
        var gridA = new ItemGrid(4, 4);
        var heldItem = new FakeItem(new GridItemDef(new Vector2Int(1, 1), null, "potion"), 20, 5);
        Assert.That(gridA.TryPlaceItem(heldItem, new Vector2Int(0, 0)), Is.True);

        var targetItem = new FakeItem(new GridItemDef(new Vector2Int(1, 1), null, "potion"), 20, 16);
        Assert.That(gridA.TryPlaceItem(targetItem, new Vector2Int(2, 2)), Is.True);

        Assert.That(InventoryMove.Move(gridA, heldItem, gridA, new Vector2Int(2, 2)), Is.True);
        Assert.That(heldItem.StackCount, Is.EqualTo(1));
        Assert.That(targetItem.StackCount, Is.EqualTo(20));
    }

    [Test]
    public void Move_StackableIntoFullStack_ReturnsFalse()
    {
        var gridA = new ItemGrid(4, 4);
        var heldItem = new FakeItem(new GridItemDef(new Vector2Int(1, 1), null, "potion"), 20, 1);
        Assert.That(gridA.TryPlaceItem(heldItem, new Vector2Int(0, 0)), Is.True);

        var targetItem = new FakeItem(new GridItemDef(new Vector2Int(1, 1), null, "potion"), 20, 20);
        Assert.That(gridA.TryPlaceItem(targetItem, new Vector2Int(2, 2)), Is.True);

        Assert.That(InventoryMove.Move(gridA, heldItem, gridA, new Vector2Int(2, 2)), Is.False);
        Assert.That(heldItem.StackCount, Is.EqualTo(1));
        Assert.That(targetItem.StackCount, Is.EqualTo(20));
    }

    [Test]
    public void Move_SingleBlocker_SwapsBothItems()
    {
        var gridA = new ItemGrid(4, 4);
        var gridB = new ItemGrid(4, 4);

        var heldItem = new FakeItem(new GridItemDef(new Vector2Int(2, 2), null, "helmet"), 1, 1);
        Assert.That(gridA.TryPlaceItem(heldItem, new Vector2Int(0, 0)), Is.True);

        var targetItem = new FakeItem(new GridItemDef(new Vector2Int(2, 2), null, "gloves"), 1, 1);
        Assert.That(gridB.TryPlaceItem(targetItem, new Vector2Int(1, 1)), Is.True);

        Assert.That(InventoryMove.Move(gridA, heldItem, gridB, new Vector2Int(1, 1)), Is.True);
        Assert.That(gridB.OriginOf(heldItem), Is.EqualTo(new Vector2Int(1, 1)));
        Assert.That(gridA.OriginOf(targetItem), Is.EqualTo(new Vector2Int(0, 0)));
    }
}
