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

    public struct ActionMenuComponent : IComponent
    {
        public int TargetEntityId;
        public int CurrentIndex;
        public MenuMode Mode;

        public ActionMenuComponent(int targetEntityId, MenuMode mode)
        {
            TargetEntityId = targetEntityId;
            CurrentIndex = 1;
            Mode = mode;
        }
    }
}
