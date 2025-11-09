using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using MonogameProject.Components;
using MonogameProject.Config;
using MonogameProject.Core;
using MonogameProject.Entities;
using MonogameProject.Enums;

namespace MonogameProject.Systems
{
    public static class TilePurchaseSystem
    {
        public static void HandlePurchase(World world)
        {
            var mouse = Mouse.GetState();

            if (mouse.LeftButton != ButtonState.Pressed)
                return;

            int tileX = mouse.X / GameSettings.TileSize;
            int tileY = mouse.Y / GameSettings.TileSize;

            foreach (var entityId in world.GetAllEntityIds())
            {
                var entity = new Entity(entityId);

                // Get components
                var position = world.TryGetComponent<PositionComponent>(entity);
                var tile = world.TryGetComponent<TileTypeComponent>(entity);
                var owned = world.TryGetComponent<OwnedComponent>(entity);

                // Validate components
                if (!position.HasValue || !tile.HasValue)
                    continue;

                // Check if this is the tile being clicked
                if (position.Value.X == tileX && position.Value.Y == tileY)
                {
                    // Condition: must be water tile
                    if (tile.Value.Type != TileType.Water)
                        return;

                    // Condition: must not be already owned
                    if (owned.HasValue && owned.Value.isOwned)
                        return;

                    // You can only purchase if there's an owned neighbor
                    bool hasNeighbor = false;

                    // Check all entities for owned neighbors
                    foreach (var otherId in world.GetAllEntityIds())
                    {
                        var other = new Entity(otherId);
                        var otherPos = world.TryGetComponent<PositionComponent>(other);
                        var otherOwned = world.TryGetComponent<OwnedComponent>(other);

                        // Validate other tile components
                        if (!otherPos.HasValue || !otherOwned.HasValue || !otherOwned.Value.isOwned)
                            continue;

                        // Check adjacency
                        int dx = Math.Abs(otherPos.Value.X - tileX);
                        int dy = Math.Abs(otherPos.Value.Y - tileY);
                        // Adjacent if within 1 tile in any direction (including diagonals)
                        if (dx <= 1 && dy <= 1 && (dx + dy) > 0)
                        {
                            hasNeighbor = true;
                            break;
                        }
                    }

                    // Neighbor check
                    if (!hasNeighbor)
                        return;

                    // Coints existence check
                    if (GameSettings.PlayerCoins < GameSettings.TileCost)
                        return;

                    // Purchase the tile
                    GameSettings.PlayerCoins -= GameSettings.TileCost;
                    world.AddComponent(entity, new OwnedComponent(true));
                    world.AddComponent(entity, new TileTypeComponent(TileType.Grass));
                }
            }
        }
    }
}
