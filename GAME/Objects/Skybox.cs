using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Objects;
using OpenGL.GAME.Components;
using OpenTK.Mathematics;

namespace OpenGL.GAME.Objects
{
    class Skybox : Entity
    {
        private const string DefaultGeometryPath = "GAME/Geometry/Skybox/Skybox.obj";
        private ComponentPosition positionComponent;

        public Skybox() : this(Vector3.Zero, DefaultGeometryPath)
        {
        }

        public Skybox(Vector3 position) : this(position, DefaultGeometryPath)
        {
        }

        public Skybox(Vector3 position, string geometryPath) : base("Skybox")
        {
            positionComponent = new ComponentPosition(position);
            AddComponent(positionComponent);
            AddComponent(new ComponentGeometry(geometryPath));
            AddComponent(new ComponentShaderSkybox());
        }

        public void FollowCamera(Camera camera)
        {
            if (camera != null)
            {
                positionComponent.Position = camera.cameraPosition;
            }
        }
    }
}