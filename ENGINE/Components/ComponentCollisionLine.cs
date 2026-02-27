using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace OpenGL.ENGINE.Components
{
    class ComponentCollisionLine : IComponent
    {
        Vector3 start;
        Vector3 end;
        float radius;

        public ComponentCollisionLine(Vector3 start, Vector3 end, float radius)
        {
            this.start = start;
            this.end = end;
            this.radius = radius;
        }

        public Vector3 Start
        {
            get { return start; }
            set { start = value; }
        }

        public Vector3 End
        {
            get { return end; }
            set { end = value; }
        }

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_LINE; }
        }

        public void Close() { }
    }
}