using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Objects;
using OpenGL.GAME.Scenes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.ENGINE.Systems
{
    class SystemPhysics : System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_VELOCITY;

        public SystemPhysics() { }

        public string Name
        {
            get { return "SystemPhysics"; }
        }

        public override void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                IComponent velocityComponent = GetComponent(entity, ComponentTypes.COMPONENT_VELOCITY);
                Vector3 velocity = ((ComponentVelocity)velocityComponent).Velocity;

                IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                //Vector3 position = ((ComponentPosition)positionComponent).Position;

                Motion((ComponentPosition)positionComponent, velocity);
            }
        }

        public void Motion(ComponentPosition position, Vector3 velocity)
        {
            position.Position = position.Position + velocity*GameScene.dt;
        }
    }
}
