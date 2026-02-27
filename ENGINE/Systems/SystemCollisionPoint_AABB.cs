using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Managers;
using OpenGL.ENGINE.Objects;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.ENGINE.Systems
{
    class SystemCollisionPoint_AABB : System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION;

        CollisionManager collisionManager;
        EntityManager entityManager;

        public SystemCollisionPoint_AABB(CollisionManager collisionManager, EntityManager entityManager)
        {
            this.collisionManager = collisionManager;
            this.entityManager = entityManager;
        }

        public string Name
        {
            get { return "SystemCollisionPoint_AABB"; }
        }

        public override void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                ComponentPosition position = (ComponentPosition)positionComponent;

                Collision(entity, position);
            }
        }

        public void Collision(Entity entity, ComponentPosition position)
        {
            if (entity == null || position == null || entityManager == null || collisionManager == null)
            {
                return;
            }

            if ((entity.Mask & ComponentTypes.COMPONENT_COLLISION_AABB) != 0)
            {
                return;
            }

            List<Entity> entities = entityManager.Entities();
            if (entities == null || entities.Count == 0)
            {
                return;
            }

            Vector3 point = position.Position;

            foreach (Entity other in entities)
            {
                if (other == null || other == entity)
                {
                    continue;
                }

                if ((other.Mask & (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_AABB))
                    != (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_AABB))
                {
                    continue;
                }

                IComponent collisionComponentB = GetComponent(other, ComponentTypes.COMPONENT_COLLISION_AABB);
                ComponentCollisionAABB collisionB = (ComponentCollisionAABB)collisionComponentB;

                IComponent positionComponentB = GetComponent(other, ComponentTypes.COMPONENT_POSITION);
                ComponentPosition positionB = (ComponentPosition)positionComponentB;

                Vector3 minimumB = positionB.Position + collisionB.Min;
                Vector3 maximumB = positionB.Position + collisionB.Max;

                bool inside =
                    point.X >= minimumB.X && point.X <= maximumB.X &&
                    point.Y >= minimumB.Y && point.Y <= maximumB.Y &&
                    point.Z >= minimumB.Z && point.Z <= maximumB.Z;

                if (inside)
                {
                    collisionManager.CollisionBetweenEntities(entity, other, COLLISIONTYPE.POINT_AABB);
                }
            }
        }
    }
}
