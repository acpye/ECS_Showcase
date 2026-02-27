using OpenTK.Mathematics;

namespace OpenGL.ENGINE.Components
{
    class ComponentPosition : IComponent
    {
        Vector3 position;

        public ComponentPosition(float x, float y, float z)
        {
            position = new Vector3(x, y, z);
        }

        public ComponentPosition(Vector3 position)
        {
            this.position = position;
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_POSITION; }
        }

        public void Close() { }
    }
}
