using GridInventory;
using NUnit.Framework;
using UnityEngine;

public class LoadoutSlotContainerTests
{
    private sealed class FakeItem : IGridItem
    {
        public GridItemDef Def { get; }
        public FakeItem(GridItemDef def) => Def = def;
    }

    [Test]
    public void TryPlaceItem_WrongTag_Rejects()
    {
        var headSlotDefinition = new LoadoutLayoutDefinition
        {
            SlotName = "head",
            RequiredSlotTag = "helmet",
            Footprint = new Vector2Int(2, 2)
        };

        var container = new LoadoutSlotContainer(headSlotDefinition);

        IGridItem testItem = new FakeItem(new GridItemDef(new Vector2Int(2, 2), null, "boots"));
        Assert.That(container.TryPlaceItem(testItem, new Vector2Int(0, 0)), Is.False);
        Assert.That(container.OriginOf(testItem), Is.Null);
    }

    [Test]
    public void Move_BagOntoOccupiedSlot_SwapsDisplacedItemIntoBag()
    {
        var mainHandSlotDefinition = new LoadoutLayoutDefinition
        {
            SlotName = "MainHand",
            RequiredSlotTag = "weapon",
            Footprint = new Vector2Int(2, 4)
        };

        var gridA = new ItemGrid(4, 4);
        IGridItem swordA = new FakeItem(new GridItemDef(new Vector2Int(1, 3), null, "weapon"));
        Assert.That(gridA.TryPlaceItem(swordA, new Vector2Int(2, 0)), Is.True);

        IGridItem swordB = new FakeItem(new GridItemDef(new Vector2Int(1, 3), null, "weapon"));
        var mainHand = new LoadoutSlotContainer(mainHandSlotDefinition);
        Assert.That(mainHand.TryPlaceItem(swordB, new Vector2Int(0, 0)), Is.True);

        Assert.That(InventoryMove.Move(gridA, swordA, mainHand, new Vector2Int(0, 0)), Is.True);
        Assert.That(mainHand.OriginOf(swordA), Is.EqualTo(new Vector2Int(0, 0)));
        Assert.That(gridA.OriginOf(swordB), Is.EqualTo(new Vector2Int(2, 0)));
        Assert.That(gridA.OriginOf(swordA), Is.Null);
    }
}
