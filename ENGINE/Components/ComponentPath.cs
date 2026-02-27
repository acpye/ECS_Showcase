using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenGL.ENGINE.Components
{
    class ComponentPath : IComponent
    {
        public List<Vector3> PathNodes { get; }
        public int CurrentNodeIndex { get; set; }

        public ComponentTypes ComponentType => ComponentTypes.COMPONENT_PATH;

        public ComponentPath(List<Vector3> pathNodes)
        {
            PathNodes = pathNodes;
            CurrentNodeIndex = 0;
        }

        public ComponentPath(List<Vector3> pathNodes, Vector3 currentPosition)
        {
            PathNodes = pathNodes;
            CurrentNodeIndex = 0;

            float shortestDistance = float.MaxValue;
            for (int i = 0; i < pathNodes.Count; i++)
            {
                float distance = Vector3.Distance(currentPosition, pathNodes[i]);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    CurrentNodeIndex = i;
                }
            }
        }

        public void Close()
        {
        }
    }
}