using System;
using System.Numerics;
using GridInventory;
using NUnit.Framework;
using UnityEngine;

public class StashTests
{
    private sealed class FakeItem : IGridItem
    {
        public GridItemDef Def { get; }
        public FakeItem(GridItemDef def) => Def = def;
    }

    [Test]
    public void RemoveTab_TabHoldingItems_RefusesAndKeepsItems()
    {
        var mainStash = new Stash();
        var gearStashTab = mainStash.AddTab("Gear", 6, 6);

        IGridItem helmet = new FakeItem(new GridItemDef(new Vector2Int(2, 2), null, "helmet"));
        Assert.That(gearStashTab.Grid.TryPlaceItem(helmet, new Vector2Int(1, 1)), Is.True);

        var tabRemoved = mainStash.RemoveTab("Gear");

        Assert.That(tabRemoved, Is.False);
        Assert.That(mainStash.GetStashTabByTabName("Gear"), Is.Not.Null);
        Assert.That(mainStash.GetStashTabByTabName("Gear").Grid.OriginOf(helmet), Is.EqualTo(new Vector2Int(1, 1)));
    }

    [Test]
    public void RenameTab_TabHoldingItems_PreservesGridContents()
    {
        var mainStash = new Stash();
        var gearStashTab = mainStash.AddTab("Gear", 6, 6);
        IGridItem sword = new FakeItem(new GridItemDef(new Vector2Int(1, 3), null, "sword"));
        Assert.That(gearStashTab.Grid.TryPlaceItem(sword, new Vector2Int(4, 2)), Is.True);

        var renamed = mainStash.RenameTab("Gear", "Weapons");

        Assert.That(renamed, Is.True);
        Assert.That(mainStash.GetStashTabByTabName("Gear"), Is.Null);
        Assert.That(mainStash.GetStashTabByTabName("Weapons").Grid.OriginOf(sword), Is.EqualTo(new Vector2Int(4, 2)));
    }

    [Test]
    public void AddTab_DuplicateName_Throws()
    {
        var mainStash = new Stash();
        mainStash.AddTab("Gear", 6, 6);
        Assert.That(() => mainStash.AddTab("Gear", 6, 6), Throws.InstanceOf<ArgumentException>());
    }
}
