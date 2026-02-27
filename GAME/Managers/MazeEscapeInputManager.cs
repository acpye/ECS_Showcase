using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Managers;
using OpenGL.ENGINE.Objects;
using OpenGL.GAME.Components;
using OpenGL.GAME.Objects;
using OpenGL.GAME.Scenes;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGL.GAME.Managers
{
    class MazeEscapeInputManager : InputManager
    {
        private readonly Camera camera;
        private readonly EntityManager entityManager;
        private bool spaceKeyPressed = false;
        private bool cKeyPressed = false;
        private bool mKeyPressed = false;

        public MazeEscapeInputManager(Camera camera, EntityManager entityManager) : base()
        {
            this.camera = camera;
            this.entityManager = entityManager;
        }

        protected override void ProcessInput()
        {
            if (IsKeyPressed(Keys.W))
            {
                camera.MoveForward(GameScene.gameInstance.PlayerSpeed);  // 0.1f
            }
            if (IsKeyPressed(Keys.S))
            {
                camera.MoveForward(-GameScene.gameInstance.PlayerSpeed); // -0.1f
            }
            if (IsKeyPressed(Keys.A))
            {
                camera.RotateY(-0.025f);
            }
            if (IsKeyPressed(Keys.D))
            {
                camera.RotateY(0.025f);
            }
            if (IsKeyPressed(Keys.Space)) // Fire
            {
                if (!spaceKeyPressed)
                {
                    FireWeapon();
                    spaceKeyPressed = true;
                }
            }
            else
            {
                spaceKeyPressed = false;
            }
            if (IsKeyPressed(Keys.M)) // Debug drone movement
            {
                if (!mKeyPressed)
                {
                    MazeEscapeBehaviourManager.DebugDroneMovement = !MazeEscapeBehaviourManager.DebugDroneMovement;
                    Console.WriteLine($"Drone movement: {(MazeEscapeBehaviourManager.DebugDroneMovement ? "DISABLED" : "ENABLED")}");
                    mKeyPressed = true;
                }
            }
            else
            {
                mKeyPressed = false;
            }
            if (IsKeyPressed(Keys.C)) // Debug noclip
            {
                if (!cKeyPressed)
                {
                    MazeEscapeCollisionManager.DebugNoClip = !MazeEscapeCollisionManager.DebugNoClip;
                    Console.WriteLine($"NoClip mode: {(MazeEscapeCollisionManager.DebugNoClip ? "ENABLED" : "DISABLED")}");
                    cKeyPressed = true;
                }
            }
            else
            {
                cKeyPressed = false;
            }
        }

        private void FireWeapon()
        {
            MazeEscapeAudioManager.PlayPlayerShoot(camera.cameraPosition);
            
            Vector3 rayStart = camera.cameraPosition;
            Vector3 rayDirection = camera.cameraDirection;
            float maxDistance = 100.0f;
            float closestDistance = maxDistance;
            Entity hitEntity = null;

            foreach (Entity entity in entityManager.Entities())
            {
                List<ComponentCollisionAABB> aabbComponents = entity.Components.Where(c => c.ComponentType == ComponentTypes.COMPONENT_COLLISION_AABB).Cast<ComponentCollisionAABB>().ToList();

                if (aabbComponents.Count == 0)
                {
                    continue;
                }

                ComponentPosition positionComponent = (ComponentPosition)entity.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_POSITION);
                Vector3 position = positionComponent?.Position ?? Vector3.Zero;

                foreach (ComponentCollisionAABB aabb in aabbComponents)
                {
                    Vector3 min = position + aabb.Min;
                    Vector3 max = position + aabb.Max;

                    float? distance = GetRayIntersection(rayStart, rayDirection, maxDistance, min, max);

                    if (distance.HasValue && distance.Value < closestDistance)
                    {
                        closestDistance = distance.Value;
                        hitEntity = entity;
                    }
                }
            }

            if (hitEntity != null)
            {
                if (hitEntity.Name == "Drone")
                {
                    if (hitEntity is Drone drone)
                    {
                        ComponentHealth healthComponent = (ComponentHealth)drone.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_HEALTH);
                        if (healthComponent != null)
                        {
                            byte damage = GameScene.gameInstance.GetPlayerDamage();
                            healthComponent.TakeDamage(damage);
                            Console.WriteLine($"{hitEntity.Name} hit - damage {damage} - current health: {healthComponent.Health}");
                            if (healthComponent.Health <= 0)
                            {
                                Console.WriteLine("Drone destroyed");
                                ComponentPosition posComponent = (ComponentPosition)drone.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_POSITION);
                                Vector3 dronePosition = posComponent?.Position ?? Vector3.Zero;
                                GameScene.gameInstance.OnDroneDestroyed(drone, dronePosition);
                                entityManager.RemoveEntity(drone);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Hit: {hitEntity.Name} at distance {closestDistance:F2}");
                }
            }
            else
            {
                Console.WriteLine("Shot missed");
            }
        }

        private float? GetRayIntersection(Vector3 rayOrigin, Vector3 rayDirection, float maxDistance, Vector3 min, Vector3 max)
        {
            float tMin = 0.0f;
            float tMax = maxDistance;

            if (Math.Abs(rayDirection.X) < 1e-6f)
            {
                if (rayOrigin.X < min.X || rayOrigin.X > max.X)
                {
                    return null;
                }
            }
            else
            {
                float t1 = (min.X - rayOrigin.X) / rayDirection.X;
                float t2 = (max.X - rayOrigin.X) / rayDirection.X;
                tMin = Math.Max(tMin, Math.Min(t1, t2));
                tMax = Math.Min(tMax, Math.Max(t1, t2));
            }

            if (Math.Abs(rayDirection.Y) < 1e-6f)
            {
                if (rayOrigin.Y < min.Y || rayOrigin.Y > max.Y)
                {
                    return null;
                }
            }
            else
            {
                float t1 = (min.Y - rayOrigin.Y) / rayDirection.Y;
                float t2 = (max.Y - rayOrigin.Y) / rayDirection.Y;
                tMin = Math.Max(tMin, Math.Min(t1, t2));
                tMax = Math.Min(tMax, Math.Max(t1, t2));
            }

            if (Math.Abs(rayDirection.Z) < 1e-6f)
            {
                if (rayOrigin.Z < min.Z || rayOrigin.Z > max.Z)
                {
                    return null;
                }
            }
            else
            {
                float t1 = (min.Z - rayOrigin.Z) / rayDirection.Z;
                float t2 = (max.Z - rayOrigin.Z) / rayDirection.Z;
                tMin = Math.Max(tMin, Math.Min(t1, t2));
                tMax = Math.Min(tMax, Math.Max(t1, t2));
            }

            if (tMax >= tMin && tMin < maxDistance && tMax > 0)
            {
                return tMin;
            }

            return null;
        }
    }
}