using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.ENGINE.Components
{
    class ComponentRotation : IComponent
    {
        public float Yaw { get; set; }

        public ComponentRotation(float yaw = 0f)
        {
            Yaw = yaw;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_ROTATION; }
        }

        public void Close() { }
    }
}
