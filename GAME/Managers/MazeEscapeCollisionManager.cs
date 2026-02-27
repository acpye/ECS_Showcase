using System;
using OpenTK.Mathematics;
using OpenGL.ENGINE.Objects;
using OpenGL.ENGINE.Managers;
using OpenGL.GAME.Scenes;
using OpenGL.GAME.Objects;
using OpenGL.ENGINE.Components;

namespace OpenGL.GAME.Managers
{
    class MazeEscapeCollisionManager : CollisionManager
    {
        private readonly Camera camera;
        private readonly EntityManager entityManager;

        public static bool DebugNoClip { get; set; } = false;

        public MazeEscapeCollisionManager(Camera camera, EntityManager entityManager) : base()
        {
            this.camera = camera;
            this.entityManager = entityManager;
        }

        public override void ProcessCollisions()
        {
            foreach (Collision collision in collisionManifold)
            {
                switch (collision.collisionType)
                {
                    case COLLISIONTYPE.CAMERA_AABB:
                        // Check if it's a power-up
                        if (collision.entityA.Name == "PowerUp")
                        {
                            HandlePowerUpCollision(collision);
                            continue;
                        }

                        // Skip wall collisions when debug noclip is enabled
                        if (DebugNoClip && collision.entityA.Name == "Maze")
                        {
                            continue;
                        }

                        HandleCamera_AABBCollision(collision);

                        if (collision.entityA.Name == "Drone")
                        {
                            GameScene.gameInstance.PlayerHit();
                        }
                        break;

                    case COLLISIONTYPE.AABB_AABB:
                        HandleAABB_AABBCollision(collision);
                        break;

                    default:
                        break;
                }
            }
            ClearManifold();
        }

        private void HandlePowerUpCollision(Collision collision)
        {
            Entity entity = collision.entityA;

            ComponentPowerUp powerUp = (ComponentPowerUp)entity.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_POWERUP);
            if (powerUp != null)
            {
                ComponentPosition positionComponent = (ComponentPosition)entity.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_POSITION);
                Vector3 position = positionComponent?.Position ?? Vector3.Zero;

                ComponentAudio audioComponent = (ComponentAudio)entity.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_AUDIO);
                audioComponent?.Close();


                GameScene.gameInstance.CollectPowerUp(powerUp.Type, powerUp.Value, position);
                entityManager.RemoveEntity(entity);
                Console.WriteLine($"Collected {powerUp.Type} power-up (+{powerUp.Value})");
            }
        }

        private void HandleCamera_AABBCollision(Collision collision)
        {
            ComponentCollisionAABB aabb = (ComponentCollisionAABB)collision.componentA;
            ComponentPosition aabbPosition = (ComponentPosition)collision.entityA.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_POSITION);

            if (aabb == null || aabbPosition == null)
            {
                return;
            }

            Vector3 aabbMin = aabbPosition.Position + aabb.Min;
            Vector3 aabbMax = aabbPosition.Position + aabb.Max;
            Vector3 cameraPosition = camera.cameraPosition;

            float dx1 = aabbMax.X - cameraPosition.X;
            float dx2 = cameraPosition.X - aabbMin.X;
            float dy1 = aabbMax.Y - cameraPosition.Y;
            float dy2 = cameraPosition.Y - aabbMin.Y;
            float dz1 = aabbMax.Z - cameraPosition.Z;
            float dz2 = cameraPosition.Z - aabbMin.Z;

            float minPenetration = float.MaxValue;
            Vector3 pushDirection = Vector3.Zero;

            if (dx1 < minPenetration) { minPenetration = dx1; pushDirection = new Vector3(dx1, 0, 0); }
            if (dx2 < minPenetration) { minPenetration = dx2; pushDirection = new Vector3(-dx2, 0, 0); }
            if (dy1 < minPenetration) { minPenetration = dy1; pushDirection = new Vector3(0, dy1, 0); }
            if (dy2 < minPenetration) { minPenetration = dy2; pushDirection = new Vector3(0, -dy2, 0); }
            if (dz1 < minPenetration) { minPenetration = dz1; pushDirection = new Vector3(0, 0, dz1); }
            if (dz2 < minPenetration) { minPenetration = dz2; pushDirection = new Vector3(0, 0, -dz2); }

            camera.cameraPosition += pushDirection;
            camera.UpdateView();
        }

        private void HandleAABB_AABBCollision(Collision collision)
        {
            ComponentPosition positionA = (ComponentPosition)collision.entityA.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_POSITION);
            ComponentVelocity velocityA = (ComponentVelocity)collision.entityA.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_VELOCITY);

            ComponentPosition positionB = (ComponentPosition)collision.entityB.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_POSITION);
            ComponentVelocity velocityB = (ComponentVelocity)collision.entityB.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_VELOCITY);

            if (velocityA != null)
            {
                positionA.Position -= velocityA.Velocity * GameScene.dt;
                velocityA.Velocity = -velocityA.Velocity;
            }

            if (velocityB != null)
            {
                positionB.Position -= velocityB.Velocity * GameScene.dt;
                velocityB.Velocity = -velocityB.Velocity;
            }
        }
    }
}