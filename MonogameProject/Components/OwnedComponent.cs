using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameProject.Components
{
    public struct OwnedComponent : IComponent
    {
        public bool isOwned;

        public OwnedComponent(bool isOwned)
        {
            this.isOwned = isOwned;
        }
    }
}
