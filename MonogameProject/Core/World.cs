using System;
using System.Collections.Generic;
using System.Linq;
using MonogameProject.Components;
using MonogameProject.Entities;

namespace MonogameProject.Core
{
    public class World
    {
        // Dictionary to hold all entities and their components
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

                var existing = components.FirstOrDefault(c => c.GetType() == type);
                if (existing != null)
                {
                    components.Remove(existing);
                }

                components.Add(component);
            }
        }

        public IEnumerable<IComponent> GetComponents(Entity entity)
        {
            return _entities.TryGetValue(entity.Id, out var components)
                ? components
                : Enumerable.Empty<IComponent>();
        }

        public T? TryGetComponent<T>(Entity entity) where T : struct, IComponent
        {
            if (_entities.TryGetValue(entity.Id, out var components))
            {
                foreach (var c in components)
                {
                    if (c is T typed)
                        return typed;
                }
            }

            return null;
        }

        public IEnumerable<int> GetAllEntityIds()
        {
            return _entities.Keys;
        }
    }
}
