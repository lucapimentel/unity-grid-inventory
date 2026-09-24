using System;
using System.Collections.Generic;
using System.Linq;

namespace GridInventory
{
    public class StashTab
    {
        public string TabName { get; internal set; }
        public readonly ItemGrid Grid;

        public StashTab(string tabName, ItemGrid grid)
        {
            TabName = tabName;
            Grid = grid;
        }
    }

    public class Stash
    {
        private readonly List<StashTab> tabs = new List<StashTab>();
        public IReadOnlyList<StashTab> Tabs => tabs;
        public IEnumerable<string> TabNames => Tabs.Select(tab => tab.TabName);
        public StashTab GetStashTabByTabName(string tabName)
        {
            return tabs.Find(tab => tab.TabName == tabName);
        }

        public Stash(List<StashTab> initialTabs = null)
        {
            if (initialTabs == null) return;
            foreach (var tab in initialTabs)
            {
                AddTab(tab.TabName, tab.Grid);
            }
        }

        public StashTab AddTab(string tabName, ItemGrid grid)
        {
            ValidateNewTabName(tabName);

            if (grid == null)
            {
                throw new ArgumentNullException(nameof(grid));
            }

            var newTabToAdd = new StashTab(tabName, grid);
            tabs.Add(newTabToAdd);

            return newTabToAdd;
        }
        public StashTab AddTab(string tabName, int width, int height)
        {
            return AddTab(tabName, new ItemGrid(width, height));
        }

        public bool RemoveTab(string tabName)
        {
            var tabToRemove = GetStashTabByTabName(tabName);

            if (tabToRemove != null || !tabToRemove.Grid.IsEmpty) return false;

            return tabs.Remove(tabToRemove);
        }

        private void ValidateNewTabName(string tabName)
        {
            if (string.IsNullOrWhiteSpace(tabName))
            {
                throw new ArgumentException("tab name must have a value", nameof(tabName));
            }

            if (GetStashTabByTabName(tabName) != null)
            {
                throw new ArgumentException("tab name is already on the list", nameof(tabName));
            }

        }

        public bool RenameTab(string currentTabName, string newTabName)
        {
            var tabToRename = GetStashTabByTabName(currentTabName);

            if (tabToRename == null) return false;
            if (currentTabName == newTabName) return true;

            ValidateNewTabName(newTabName);
            tabToRename.TabName = newTabName;
            return true;
        }
    }

}
