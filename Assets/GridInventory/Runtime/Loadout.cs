using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridInventory
{
    public class Loadout
    {
        public List<LoadoutSlotContainer> Slots { get; }

        public Loadout(LoadoutLayout layout)
        {
            if (layout == null)
            {
                throw new ArgumentNullException(nameof(layout));
            }

            var duplicates = layout.FindDuplicateSlotNames();
            if (duplicates.Count > 0)
            {
                throw new ArgumentException($"{string.Join(", ", duplicates)}", nameof(layout));
            }

            Slots = new List<LoadoutSlotContainer>();
            foreach (var definition in layout.SlotDefinitions)
            {
                Slots.Add(new LoadoutSlotContainer(definition));
            }

        }

        public LoadoutSlotContainer SlotNamed(string slotName)
        {
            return Slots.Find(slot => slot.Definition.SlotName == slotName);
        }
    }

}
