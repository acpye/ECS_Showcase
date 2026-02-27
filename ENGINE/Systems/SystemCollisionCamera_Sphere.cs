using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenGL.ENGINE.Objects;
using OpenGL.ENGINE.Managers;
using OpenGL.GAME.Objects;
using OpenGL.ENGINE.Components;

namespace OpenGL.ENGINE.Systems
{
    class SystemCollisionCamera_Sphere : System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SPHERE;

        CollisionManager collisionManager;
        Camera camera;

        public SystemCollisionCamera_Sphere(CollisionManager collisionManager, Camera camera)
        {
            this.collisionManager = collisionManager;
            this.camera = camera;
        }

        public string Name
        {
            get { return "SystemCollisionCamera_Sphere"; }
        }

        public override void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                IComponent collisionComponent = GetComponent(entity, ComponentTypes.COMPONENT_COLLISION_SPHERE);
                ComponentCollisionSphere collision = (ComponentCollisionSphere)collisionComponent;

                IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                ComponentPosition position = (ComponentPosition)positionComponent;

                Collision(entity, position, collision);
            }
        }

        public void Collision(Entity entity, ComponentPosition position, ComponentCollisionSphere collision)
        {
            if (entity == null || position == null || collision == null || camera == null)
            {
                return;
            }

            float distanceSquared = (position.Position - camera.cameraPosition).LengthSquared;
            float radius = collision.Radius;
            if (distanceSquared <= radius * radius)
            {
                collisionManager.CollisionBetweenCamera(entity, collision, COLLISIONTYPE.CAMERA_SPHERE);
            }
        }
    }
}
