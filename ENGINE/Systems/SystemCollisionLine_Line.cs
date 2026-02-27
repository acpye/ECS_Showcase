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
    class SystemCollisionLine_Line : System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_LINE;

        CollisionManager collisionManager;
        EntityManager entityManager;

        public SystemCollisionLine_Line(CollisionManager collisionManager, EntityManager entityManager)
        {
            this.collisionManager = collisionManager;
            this.entityManager = entityManager;
        }

        public string Name
        {
            get { return "SystemCollisionLine_Line"; }
        }

        public override void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                IComponent collisionComponent = GetComponent(entity, ComponentTypes.COMPONENT_COLLISION_LINE);
                ComponentCollisionLine collision = (ComponentCollisionLine)collisionComponent;

                IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                ComponentPosition position = (ComponentPosition)positionComponent;

                Collision(entity, position, collision);
            }
        }

        public void Collision(Entity entity, ComponentPosition position, ComponentCollisionLine collision)
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
                IComponent collisionComponentB = GetComponent(other, ComponentTypes.COMPONENT_COLLISION_LINE);
                ComponentCollisionLine collisionB = (ComponentCollisionLine)collisionComponentB;

                IComponent positionComponentB = GetComponent(other, ComponentTypes.COMPONENT_POSITION);
                ComponentPosition positionB = (ComponentPosition)positionComponentB;

                Vector3 segment1Start = position.Position + collision.Start;
                Vector3 segment1End = position.Position + collision.End;
                Vector3 segment2Start = positionB.Position + collisionB.Start;
                Vector3 segment2End = positionB.Position + collisionB.End;

                float distanceSquared = SegmentSegmentDistanceSquared(segment1Start, segment1End, segment2Start, segment2End);
                float combinedRadius = collision.Radius + collisionB.Radius;
                if (distanceSquared <= combinedRadius * combinedRadius)
                {
                    collisionManager.CollisionBetweenEntities(entity, other, COLLISIONTYPE.LINE_LINE);
                }
            }
        }

        static float SegmentSegmentDistanceSquared(Vector3 segment1Start, Vector3 segment1End, Vector3 segment2Start, Vector3 segment2End)
        {
            Vector3 direction1 = segment1End - segment1Start;
            Vector3 direction2 = segment2End - segment2Start;
            Vector3 startOffset = segment1Start - segment2Start;
            float direction1LengthSquared = Vector3.Dot(direction1, direction1);
            float direction2LengthSquared = Vector3.Dot(direction2, direction2);
            float dotDirection2Offset = Vector3.Dot(direction2, startOffset);

            const float SMALL_NUMBER = 1e-8f;

            if (direction1LengthSquared <= SMALL_NUMBER && direction2LengthSquared <= SMALL_NUMBER)
            {
                return Vector3.DistanceSquared(segment1Start, segment2Start);
            }
            if (direction1LengthSquared <= SMALL_NUMBER)
            {
                float t2 = MathHelper.Clamp(dotDirection2Offset / direction2LengthSquared, 0f, 1f);
                Vector3 projectedPointOnSegment2 = segment2Start + direction2 * t2;
                return Vector3.DistanceSquared(segment1Start, projectedPointOnSegment2);
            }

            if (direction2LengthSquared <= SMALL_NUMBER)
            {
                float s2 = MathHelper.Clamp(-Vector3.Dot(direction1, startOffset) / direction1LengthSquared, 0f, 1f);
                Vector3 projectedPointOnSegment1 = segment1Start + direction1 * s2;
                return Vector3.DistanceSquared(projectedPointOnSegment1, segment2Start);
            }

            float dotDirection1Direction2 = Vector3.Dot(direction1, direction2);
            float dotDirection1Offset = Vector3.Dot(direction1, startOffset);
            float denominator = direction1LengthSquared * direction2LengthSquared - dotDirection1Direction2 * dotDirection1Direction2;
            float parametricOnSegment1 = 0f;
            float parametricOnSegment2 = 0f;

            if (denominator != 0f)
            {
                parametricOnSegment1 = MathHelper.Clamp((dotDirection1Direction2 * dotDirection2Offset - dotDirection1Offset * direction2LengthSquared) / denominator, 0f, 1f);
            }
            else
            {
                parametricOnSegment1 = 0f;
            }

            float parametricOnSegment2Numerator = dotDirection1Direction2 * parametricOnSegment1 + dotDirection2Offset;
            if (parametricOnSegment2Numerator <= 0f)
            {
                parametricOnSegment2 = 0f;
                parametricOnSegment1 = MathHelper.Clamp(-dotDirection1Offset / direction1LengthSquared, 0f, 1f);
            }
            else if (parametricOnSegment2Numerator >= direction2LengthSquared)
            {
                parametricOnSegment2 = 1f;
                parametricOnSegment1 = MathHelper.Clamp((dotDirection1Direction2 - dotDirection1Offset) / direction1LengthSquared, 0f, 1f);
            }
            else
            {
                parametricOnSegment2 = parametricOnSegment2Numerator / direction2LengthSquared;
            }

            if (denominator != 0f)
            {
                float parametricOnSegment1Numerator = dotDirection1Direction2 * dotDirection2Offset - dotDirection1Offset * direction2LengthSquared;
                parametricOnSegment1 = MathHelper.Clamp(parametricOnSegment1Numerator / denominator, 0f, 1f);
            }

            Vector3 closestPointOnSegment1 = segment1Start + direction1 * parametricOnSegment1;
            Vector3 closestPointOnSegment2 = segment2Start + direction2 * parametricOnSegment2;
            return Vector3.DistanceSquared(closestPointOnSegment1, closestPointOnSegment2);
        }
    }
}