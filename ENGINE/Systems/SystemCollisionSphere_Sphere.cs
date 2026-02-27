using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenGL.ENGINE.Objects;
using OpenGL.ENGINE.Managers;
using OpenGL.ENGINE.Components;

namespace OpenGL.ENGINE.Systems
{
    class SystemCollisionSphere_Sphere : System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SPHERE;

        CollisionManager collisionManager;
        EntityManager entityManager;

        public SystemCollisionSphere_Sphere(CollisionManager collisionManager, EntityManager entityManager)
        {
            this.collisionManager = collisionManager;
            this.entityManager = entityManager;
        }

        public string Name
        {
            get { return "SystemCollisionSphere_Sphere"; }
        }

        public override void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> collisionComponents = entity.Components.Where(c => c.ComponentType == ComponentTypes.COMPONENT_COLLISION_SPHERE).ToList();
                IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                ComponentPosition position = (ComponentPosition)positionComponent;

                foreach (ComponentCollisionSphere collision in collisionComponents)
                {
                    Collision(entity, position, collision);
                }
            }
        }

        public void Collision(Entity entity, ComponentPosition position, ComponentCollisionSphere collision)
        {
            if (entity == null || position == null || collision == null || entityManager == null)
            {
                return;
            }

            List<Entity> entities = entityManager.Entities();
            int index = entities.IndexOf(entity);
            if (index < 0)
            {
                return;
            }

            for (int i = index + 1; i < entities.Count; i++)
            {
                Entity other = entities[i];

                if ((other.Mask & MASK) != MASK)
                {
                    continue;
                }

                List<IComponent> otherCollisionComponents = other.Components.Where(c => c.ComponentType == ComponentTypes.COMPONENT_COLLISION_SPHERE).ToList();
                IComponent positionComponentB = GetComponent(other, ComponentTypes.COMPONENT_POSITION);
                ComponentPosition positionB = (ComponentPosition)positionComponentB;

                foreach (ComponentCollisionSphere collisionB in otherCollisionComponents)
                {
                    Vector3 delta = position.Position - positionB.Position;
                    float distanceSquared = delta.LengthSquared;
                    float combinedRadius = collision.Radius + collisionB.Radius;
                    if (distanceSquared <= combinedRadius * combinedRadius)
                    {
                        collisionManager.CollisionBetweenEntities(entity, other, COLLISIONTYPE.SPHERE_SPHERE);
                    }
                }
            }
        }
    }
}