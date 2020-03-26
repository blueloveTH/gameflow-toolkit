using System.Collections.Generic;
using UnityEngine;

namespace GameFlow
{
    internal class MetaSignal
    {
        public string name { get; private set; }
        public InteractionUnit src { get; private set; }
        public List<MetaSlot> mSlots { get; private set; }

        public MetaSignal(string name, InteractionUnit src)
        {
            this.name = name;
            this.src = src;
            mSlots = new List<MetaSlot>();
        }
    }

}
