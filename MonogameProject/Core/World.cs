using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonogameProject.Components;
using MonogameProject.Entities;

namespace MonogameProject.Core
{
    public class World
    {
        /*
        private readonly Dictionary<int, List<IComponent>> _entities = new();

        public Entity CreateEntity(int id)
        {
            var entity = new Entity(id);
            _entities[entity.Id] = new List<IComponent>();
            return entity;
        }

        public void AddComponent(Entity entity, IComponent component)
        {
            if (_entities.ContainsKey(entity.Id))
            {
                var components = _entities[entity.Id];
                var type = component.GetType();

                if (!components.Any(c => c.GetType() == type))
                {
                    components.Add(component);
                }
            }
        }

        public IEnumerable<IComponent> GetComponents(Entity entity)
        {
            return _entities.TryGetValue(entity.Id, out var components)
                ? components
                : new List<IComponent>();
        }
        */
    }
}
