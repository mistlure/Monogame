using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonogameProject.Enums;
namespace MonogameProject.Components
{
    public struct PlantComponent : IComponent
    {
        public PlantType Type;
        public HarvestBehavior HarvestBehavior;

        public int StagesCount;
        public int CurrentStage;

        public float GrowthTime;
        public float TimeSinceLastGrowth;

        public PlantComponent(PlantType type, HarvestBehavior harvestBehavior, int stagesCount, float growthTime)
        {
            Type = type;
            HarvestBehavior = harvestBehavior;
            StagesCount = stagesCount;
            CurrentStage = 1;
            GrowthTime = growthTime;
            TimeSinceLastGrowth = 0f;
        }
    }
}
