using OpenGL.ENGINE.Managers;
using OpenGL.ENGINE.OBJLoader;

namespace OpenGL.ENGINE.Components
{
    class ComponentGeometry : IComponent
    {
        Geometry geometry;

        public ComponentGeometry(string geometryName)
        {
            geometry = ResourceManager.LoadGeometry(geometryName);
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_GEOMETRY; }
        }

        public Geometry Geometry()
        {
            return geometry;
        }

        public void Close() { }
    }
}
