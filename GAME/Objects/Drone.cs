using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Objects;
using OpenGL.GAME.Components;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenGL.GAME.Objects
{
    class Drone : Entity
    {
        private const string DefaultGeometryPath = "GAME/Geometry/Intergalactic_Spaceship/Intergalactic_Spaceship.obj";
        private const float DefaultSpeed = 2.5f;

        public Vector3 StartPosition { get; }

        public Drone(Vector3 position, List<Vector3> pathNodes)
            : this(position, pathNodes, DefaultGeometryPath, DefaultSpeed, "Drone")
        {
        }

        public Drone(Vector3 position, List<Vector3> pathNodes, float speed)
            : this(position, pathNodes, DefaultGeometryPath, speed, "Drone")
        {
        }

        public Drone(Vector3 position, List<Vector3> pathNodes, string geometryPath, float speed)
            : this(position, pathNodes, geometryPath, speed, "Drone")
        {
        }

        public Drone(Vector3 position, List<Vector3> pathNodes, string geometryPath, float speed, string name)
            : base(name)
        {
            StartPosition = position;

            AddComponent(new ComponentPosition(position));
            AddComponent(new ComponentVelocity(speed));
            AddComponent(new ComponentRotation());
            AddComponent(new ComponentPath(pathNodes, position));
            AddComponent(new ComponentGeometry(geometryPath));
            AddComponent(new ComponentShaderDefault());
            AddComponent(new ComponentCollisionAABB(geometryPath));
            AddComponent(new ComponentHealth(3));
        }

        public void ResetToStart()
        {
            ComponentPosition positionComponent = (ComponentPosition)Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_POSITION);
            if (positionComponent != null)
            {
                positionComponent.Position = StartPosition;
            }
        }
    }
}