using System.Collections.Generic;
using OpenTK.Mathematics;

namespace OpenGL.GAME.Objects
{
    class PathNodes
    {
        public List<Vector3> Nodes { get; }

        public PathNodes()
        {
            Nodes = new List<Vector3>
            {
                new Vector3(-55.0f, 1.5f, 55.0f),
                new Vector3(-45.0f, 1.5f, 55.0f),
                new Vector3(-35.0f, 1.5f, 55.0f),
                new Vector3(-25.0f, 1.5f, 55.0f),
                new Vector3(-15.0f, 1.5f, 55.0f),
                new Vector3(-5.0f, 1.5f, 55.0f),

                new Vector3(-55.0f, 1.5f, 45.0f),
                new Vector3(-45.0f, 1.5f, 45.0f),
                new Vector3(-35.0f, 1.5f, 45.0f),
                new Vector3(-25.0f, 1.5f, 45.0f),
                new Vector3(-15.0f, 1.5f, 45.0f),
                new Vector3(-5.0f, 1.5f, 45.0f),

                new Vector3(-55.0f, 1.5f, 35.0f),
                new Vector3(-45.0f, 1.5f, 35.0f),
                new Vector3(-35.0f, 1.5f, 35.0f),
                // new Vector3(-30.0f, 1.5f, 35.0f),
                new Vector3(-25.0f, 1.5f, 35.0f),
                new Vector3(-15.0f, 1.5f, 35.0f),
                new Vector3(-5.0f, 1.5f, 35.0f),

                new Vector3(-55.0f, 1.5f, 25.0f),
                new Vector3(-45.0f, 1.5f, 25.0f),
                new Vector3(-35.0f, 1.5f, 25.0f),
                // new Vector3(-30.0f, 1.5f, 25.0f),
                new Vector3(-25.0f, 1.5f, 25.0f),
                new Vector3(-15.0f, 1.5f, 25.0f),
                new Vector3(-5.0f, 1.5f, 25.0f),

                new Vector3(-55.0f, 1.5f, 15.0f),
                new Vector3(-45.0f, 1.5f, 15.0f),
                new Vector3(-35.0f, 1.5f, 15.0f),
                new Vector3(-25.0f, 1.5f, 15.0f),
                new Vector3(-15.0f, 1.5f, 15.0f),
                new Vector3(-5.0f, 1.5f, 15.0f),

                new Vector3(-55.0f, 1.5f, 5.0f),
                new Vector3(-45.0f, 1.5f, 5.0f),
                new Vector3(-35.0f, 1.5f, 5.0f),
                new Vector3(-25.0f, 1.5f, 5.0f),
                new Vector3(-15.0f, 1.5f, 5.0f),
                new Vector3(-5.0f, 1.5f, 5.0f)
            };
        }

        public PathNodes(IEnumerable<Vector3> nodes)
        {
            Nodes = new List<Vector3>(nodes);
        }
    }
}