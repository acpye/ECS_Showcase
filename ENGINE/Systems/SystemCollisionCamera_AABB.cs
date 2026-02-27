using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenGL.ENGINE.Objects;
using OpenGL.ENGINE.Managers;
using OpenGL.GAME.Objects;
using OpenGL.ENGINE.Components;

namespace OpenGL.ENGINE.Systems
{
    class SystemCollisionCamera_AABB : System
    {
        private const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_AABB;

        private readonly CollisionManager collisionManager;
        private readonly Camera camera;

        public SystemCollisionCamera_AABB(CollisionManager collisionManager, Camera camera)
        {
            this.collisionManager = collisionManager;
            this.camera = camera;
        }

        public string Name
        {
            get { return "SystemCollisionCamera_AABB"; }
        }
        public override void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> collisionComponents = entity.Components.Where(c => c.ComponentType == ComponentTypes.COMPONENT_COLLISION_AABB).ToList();
                IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                ComponentPosition position = (ComponentPosition)positionComponent;

                foreach (ComponentCollisionAABB collision in collisionComponents)
                {
                    Collision(entity, position, collision);
                }
            }
        }

        private void Collision(Entity entity, ComponentPosition position, ComponentCollisionAABB collision)
        {
            if (entity == null || position == null || collision == null || camera == null || collisionManager == null)
            {
                return;
            }

            Vector3 point = camera.cameraPosition;
            Vector3 minimum = position.Position + collision.Min;
            Vector3 maximum = position.Position + collision.Max;

            bool inside = point.X >= minimum.X && point.X <= maximum.X &&
                          point.Y >= minimum.Y && point.Y <= maximum.Y &&
                          point.Z >= minimum.Z && point.Z <= maximum.Z;

            if (inside)
            {
                collisionManager.CollisionBetweenCamera(entity, collision, COLLISIONTYPE.CAMERA_AABB);
            }
        }
    }
}