using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace OpenGL.ENGINE.Components
{
    public class ComponentVelocity : IComponent
    {
        Vector3 velocity;
        public float Speed { get; set; }

        public ComponentVelocity(float speed = 1.0f)
        {
            velocity = Vector3.Zero;
            Speed = speed;
        }

        public ComponentVelocity(float x, float y, float z, float speed = 1.0f)
        {
            velocity = new Vector3(x, y, z);
            Speed = speed;
        }

        public ComponentVelocity(Vector3 velocity, float speed = 1.0f)
        {
            this.velocity = velocity;
            Speed = speed;
        }

        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_VELOCITY; }
        }

        public void Close() { }
    }
}