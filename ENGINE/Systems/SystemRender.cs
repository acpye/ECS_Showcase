using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Mathematics;
using OpenGL.ENGINE.Objects;
using OpenGL.ENGINE.Scenes;
using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.OBJLoader;

namespace OpenGL.ENGINE.Systems
{
    class SystemRender : System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_SHADER;
        Scene scene;

        public SystemRender(Scene scene)
        {
            this.scene = scene;
        }

        public string Name
        {
            get { return "SystemRender"; }
        }

        public override void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                IComponent geometryComponent = GetComponent(entity, ComponentTypes.COMPONENT_GEOMETRY);
                Geometry geometry = ((ComponentGeometry)geometryComponent).Geometry();

                IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                Vector3 position = ((ComponentPosition)positionComponent).Position;

                float yaw = 0f;
                IComponent rotationComponent = GetComponent(entity, ComponentTypes.COMPONENT_ROTATION);
                if (rotationComponent is ComponentRotation rot)
                {
                    yaw = rot.Yaw;
                }

                Matrix4 model = Matrix4.CreateRotationY(yaw) * Matrix4.CreateTranslation(position);

                IComponent shaderComponent = GetComponent(entity, ComponentTypes.COMPONENT_SHADER);
                ComponentShader shader = (ComponentShader)shaderComponent;

                Draw(model, geometry, shader);
            }
        }

        public void Draw(Matrix4 model, Geometry geometry, ComponentShader shaderComponent)
        {
            shaderComponent.ApplyShader(scene, model, geometry);
        }
    }
}
