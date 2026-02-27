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
    class SystemCollisionAABB_AABB : System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_AABB;

        CollisionManager collisionManager;
        EntityManager entityManager;

        public SystemCollisionAABB_AABB(CollisionManager collisionManager, EntityManager entityManager)
        {
            this.collisionManager = collisionManager;
            this.entityManager = entityManager;
        }

        public string Name
        {
            get { return "SystemCollisionAABB_AABB"; }
        }

        public override void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> collisionComponents = entity.Components.Where(c => c.ComponentType == ComponentTypes.COMPONENT_COLLISION_AABB).ToList();
                IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                ComponentPosition position = (ComponentPosition)positionComponent;

                foreach (ComponentCollisionAABB collision in collisionComponents.Cast<ComponentCollisionAABB>())
                {
                    Collision(entity, position, collision);
                }
            }
        }
        public void Collision(Entity entity, ComponentPosition position, ComponentCollisionAABB collision)
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
                List<IComponent> otherCollisionComponents = other.Components.Where(c => c.ComponentType == ComponentTypes.COMPONENT_COLLISION_AABB).ToList();
                IComponent positionComponentB = GetComponent(other, ComponentTypes.COMPONENT_POSITION);
                ComponentPosition positionB = (ComponentPosition)positionComponentB;

                foreach (ComponentCollisionAABB collisionB in otherCollisionComponents.Cast<ComponentCollisionAABB>())
                {
                    Vector3 minimumA = position.Position + collision.Min;
                    Vector3 maximumA = position.Position + collision.Max;
                    Vector3 minimumB = positionB.Position + collisionB.Min;
                    Vector3 maximumB = positionB.Position + collisionB.Max;

                    bool overlap =
                        minimumA.X <= maximumB.X && maximumA.X >= minimumB.X &&
                        minimumA.Y <= maximumB.Y && maximumA.Y >= minimumB.Y &&
                        minimumA.Z <= maximumB.Z && maximumA.Z >= minimumB.Z;

                    if (overlap)
                    {
                        collisionManager.CollisionBetweenEntities(entity, other, COLLISIONTYPE.AABB_AABB);
                    }
                }
            }

        }
    }
}