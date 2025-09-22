using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonogameProject.Enums;

namespace MonogameProject.Components
{
    public struct TileTypeComponent : IComponent
    {
        public TileType Type;

        public TileTypeComponent(TileType type)
        {
            Type = type;
        }
    }
}
