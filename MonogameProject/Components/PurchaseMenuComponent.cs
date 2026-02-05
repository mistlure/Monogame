using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameProject.Components
{
    public enum MenuMode
    {
        Buy,
        Dig
    }

    public struct PurchaseMenuComponent : IComponent
    {
        public int TargetEntityId;
        public int CurrentIndex;
        public MenuMode Mode;

        public PurchaseMenuComponent(int targetEntityId, MenuMode mode)
        {
            TargetEntityId = targetEntityId;
            CurrentIndex = 1;
            Mode = mode;
        }
    }
}
