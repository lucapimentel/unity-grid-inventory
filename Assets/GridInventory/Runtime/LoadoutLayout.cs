using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GridInventory
{
    [Serializable]
    public class LoadoutLayoutDefinition
    {
        public string SlotName; //head, mainhand, offhand etc.
        public string RequiredSlotTag; // check the slottag
        public Vector2Int Footprint; // how many slots it takes
        public Vector2Int PanelPosition; // UI uses to draw it
    }

    [CreateAssetMenu]
    public class LoadoutLayout : ScriptableObject
    {
        public List<LoadoutLayoutDefinition> SlotDefinitions;
        public List<string> FindDuplicateSlotNames()
        {
            var duplicatedNames = SlotDefinitions.GroupBy(x => x.SlotName).Where(group => group.Count() > 1).Select(group => group.Key);

            return duplicatedNames.ToList();
        }

    }

}
