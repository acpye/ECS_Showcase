using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Objects;
using OpenGL.GAME.Objects;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.ENGINE.Systems
{
    class SystemAudio : System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_AUDIO;
        private readonly Camera camera;

        public SystemAudio(Camera camera)
        {
            this.camera = camera;
        }

        public string Name
        {
            get { return "SystemAudio"; }
        }

        public override void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                ComponentPosition positionComponent = (ComponentPosition)GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                ComponentAudio audioComponent = (ComponentAudio)GetComponent(entity, ComponentTypes.COMPONENT_AUDIO);

                if (positionComponent != null && audioComponent != null)
                {
                    audioComponent.SetPosition(positionComponent.Position);
                }
            }

            AL.Listener(ALListener3f.Position, ref camera.cameraPosition);
            AL.Listener(ALListenerfv.Orientation, ref camera.cameraDirection, ref camera.cameraUp);
        }
    }
}