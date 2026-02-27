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
    class SystemCollisionCamera_Line : System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_LINE;

        CollisionManager collisionManager;
        Camera camera;

        public SystemCollisionCamera_Line(CollisionManager collisionManager, Camera camera)
        {
            this.collisionManager = collisionManager;
            this.camera = camera;
        }

        public string Name
        {
            get { return "SystemCollisionCamera_Line"; }
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
            if (entity == null || position == null || collision == null || camera == null || collisionManager == null)
            {
                return;
            }

            Vector3 segmentStart = position.Position + collision.Start;
            Vector3 segmentEnd = position.Position + collision.End;

            float distanceSquared = PointSegmentDistanceSquared(camera.cameraPosition, segmentStart, segmentEnd);
            float radius = collision.Radius;
            if (distanceSquared <= radius * radius)
            {
                collisionManager.CollisionBetweenCamera(entity, collision, COLLISIONTYPE.CAMERA_LINE);
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
