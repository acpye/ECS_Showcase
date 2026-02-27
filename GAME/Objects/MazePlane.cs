using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Objects;
using OpenTK.Mathematics;

namespace OpenGL.GAME.Objects
{
    class MazePlane : Entity
    {
        private const string DefaultGeometryPath = "GAME/Geometry/Maze2/plane.obj";

        public MazePlane() : this(Vector3.Zero, DefaultGeometryPath)
        {
        }

        public MazePlane(Vector3 position) : this(position, DefaultGeometryPath)
        {
        }

        public MazePlane(Vector3 position, string geometryPath) : base("MazePlane")
        {
            AddComponent(new ComponentPosition(position));
            AddComponent(new ComponentGeometry(geometryPath));
            AddComponent(new ComponentShaderDefault());
        }
    }
}