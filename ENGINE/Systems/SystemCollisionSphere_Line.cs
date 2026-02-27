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
    class SystemCollisionSphere_Line : System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SPHERE;

        CollisionManager collisionManager;
        EntityManager entityManager;

        public SystemCollisionSphere_Line(CollisionManager collisionManager, EntityManager entityManager)
        {
            this.collisionManager = collisionManager;
            this.entityManager = entityManager;
        }

        public string Name
        {
            get { return "SystemCollisionSphere_Line"; }
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
            if (entity == null || position == null || collision == null || entityManager == null || collisionManager == null)
            {
                return;
            }

            Vector3 sphereCenter = position.Position;
            float sphereRadius = collision.Radius;

            List<Entity> entities = entityManager.Entities();
            if (entities == null || entities.Count == 0)
            {
                return;
            }

            foreach (Entity other in entities)
            {
                if (other == null || other == entity)
                {
                    continue;
                }

                if ((other.Mask & (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_LINE))
                    != (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_LINE))
                {
                    continue;
                }

                IComponent collisionComponentB = GetComponent(other, ComponentTypes.COMPONENT_COLLISION_LINE);
                ComponentCollisionLine line = (ComponentCollisionLine)collisionComponentB;

                IComponent positionComponentB = GetComponent(other, ComponentTypes.COMPONENT_POSITION);
                ComponentPosition positionB = (ComponentPosition)positionComponentB;

                Vector3 segmentStart = positionB.Position + line.Start;
                Vector3 segmentEnd = positionB.Position + line.End;

                float distanceSquared = PointSegmentDistanceSquared(sphereCenter, segmentStart, segmentEnd);
                float combined = sphereRadius + line.Radius;
                if (distanceSquared <= combined * combined)
                {
                    collisionManager.CollisionBetweenEntities(entity, other, COLLISIONTYPE.SPHERE_LINE);
                }
            }
        }

        static float PointSegmentDistanceSquared(Vector3 point, Vector3 segmentStart, Vector3 segmentEnd)
        {
            Vector3 segmentVector = segmentEnd - segmentStart;
            Vector3 pointToStart = point - segmentStart;
            float segmentLengthSquared = Vector3.Dot(segmentVector, segmentVector);

            const float SMALL_NUMBER = 1e-8f;
            if (segmentLengthSquared <= SMALL_NUMBER)
            {
                return (point - segmentStart).LengthSquared;
            }

            float projection = Vector3.Dot(pointToStart, segmentVector) / segmentLengthSquared;
            projection = MathHelper.Clamp(projection, 0f, 1f);
            Vector3 closest = segmentStart + segmentVector * projection;
            return (point - closest).LengthSquared;
        }
    }
}
