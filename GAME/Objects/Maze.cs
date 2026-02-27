using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Objects;
using OpenTK.Mathematics;

namespace OpenGL.GAME.Objects
{
    class Maze : Entity
    {
        private const string DefaultGeometryPath = "GAME/Geometry/Maze2/maze3.obj";

        public Maze() : this(Vector3.Zero, DefaultGeometryPath)
        {
        }

        public Maze(Vector3 position) : this(position, DefaultGeometryPath)
        {
        }

        public Maze(Vector3 position, string geometryPath) : base("Maze")
        {
            AddComponent(new ComponentPosition(position));
            AddComponent(new ComponentGeometry(geometryPath));
            AddComponent(new ComponentShaderDefault());

            // Outer walls
            AddComponent(new ComponentCollisionAABB(new Vector3(-60.0f, 0.0f, 0.0f), new Vector3(0.0f, 10.0f, 1.5f)));
            AddComponent(new ComponentCollisionAABB(new Vector3(-1.5f, 0.0f, 0.0f), new Vector3(0.0f, 10.0f, 60.0f)));
            AddComponent(new ComponentCollisionAABB(new Vector3(-60.0f, 0.0f, 58.5f), new Vector3(0.0f, 10.0f, 60.0f)));
            AddComponent(new ComponentCollisionAABB(new Vector3(-60.0f, 0.0f, 0.0f), new Vector3(-58.5f, 10.0f, 60.0f)));

            // Inner walls
            AddComponent(new ComponentCollisionAABB(new Vector3(-40.0f, 0.0f, 9.5f), new Vector3(-20.0f, 10.0f, 11.5f)));
            AddComponent(new ComponentCollisionAABB(new Vector3(-11.5f, 0.0f, 20.0f), new Vector3(-9.5f, 10.0f, 40.0f)));
            AddComponent(new ComponentCollisionAABB(new Vector3(-40.0f, 0.0f, 48.5f), new Vector3(-20.0f, 10.0f, 50.5f)));
            AddComponent(new ComponentCollisionAABB(new Vector3(-50.5f, 0.0f, 20.0f), new Vector3(-48.5f, 10.0f, 40.0f)));

            // Central walls
            AddComponent(new ComponentCollisionAABB(new Vector3(-21.5f, 0.0f, 20.0f), new Vector3(-19.5f, 10.0f, 40.0f)));
            AddComponent(new ComponentCollisionAABB(new Vector3(-40.0f, 0.0f, 38.5f), new Vector3(-19.5f, 10.0f, 40.5f)));
            AddComponent(new ComponentCollisionAABB(new Vector3(-40.5f, 0.0f, 20.0f), new Vector3(-38.5f, 10.0f, 40.0f)));
        }
    }
}
