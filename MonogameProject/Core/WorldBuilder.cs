using MonogameProject.Components;
using MonogameProject.Enums;
using MonogameProject.Core;
using MonogameProject.Entities;

namespace MonogameProject.Core
{
    public static class WorldBuilder
    {
        /*
        public static void Build(World world)
        {
            // 🟫 Empty block (0, 0)
            var emptyBlock = world.CreateEntity(1);
            world.AddComponent(emptyBlock, new PositionComponent(0, 0));
            world.AddComponent(emptyBlock, new TileTypeComponent(TileType.Farm));

            // 🥕 Carrot (1, 0)
            var carrotBlock = world.CreateEntity(2);
            world.AddComponent(carrotBlock, new PositionComponent(1, 0));
            world.AddComponent(carrotBlock, new TileTypeComponent(TileType.Farm));
            world.AddComponent(carrotBlock, new PlantComponent(
                PlantType.Crops,
                HarvestBehavior.Remove,
                stagesCount: 3,
                growthTime: 45f
            ));
        }
        */
    }
}
