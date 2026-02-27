using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenGL.ENGINE.Managers;
using OpenGL.ENGINE.OBJLoader;

namespace OpenGL.ENGINE.Components
{
    class ComponentCollisionAABB : IComponent
    {
        Vector3 min;
        Vector3 max;

        public ComponentCollisionAABB(string geometryName)
        {
            Geometry geometry = ResourceManager.LoadGeometry(geometryName);
            (min, max) = geometry.CalculateAABB();
        }

        public ComponentCollisionAABB(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }

        public Vector3 Min
        {
            get { return min; }
            set { min = value; }
        }

        public Vector3 Max
        {
            get { return max; }
            set { max = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_AABB; }
        }

        public void Close() { }
    }
}