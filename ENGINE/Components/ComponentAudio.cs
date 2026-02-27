using System;
using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;
using OpenGL.ENGINE.Managers;

namespace OpenGL.ENGINE.Components
{
    class ComponentAudio : IComponent
    {
        private int audioSource;
        private int audioBuffer;
        private bool looping;

        public ComponentTypes ComponentType => ComponentTypes.COMPONENT_AUDIO;

        public ComponentAudio(string audioFile, bool looping = true)
        {
            audioBuffer = ResourceManager.LoadAudio(audioFile);

            audioSource = AL.GenSource();
            AL.Source(audioSource, ALSourcei.Buffer, audioBuffer);
            AL.Source(audioSource, ALSourceb.Looping, looping);
            this.looping = looping;
        }

        public void SetPosition(Vector3 emitterPosition)
        {
            AL.Source(audioSource, ALSource3f.Position, ref emitterPosition);
        }

        public void SetVelocity(Vector3 velocity)
        {
            AL.Source(audioSource, ALSource3f.Velocity, ref velocity);
        }

        public void Play()
        {
            AL.SourcePlay(audioSource);
        }

        public void Stop()
        {
            AL.SourceStop(audioSource);
        }

        public void Close()
        {
            AL.SourceStop(audioSource);
            AL.DeleteSource(audioSource);
        }

        public int AudioSource => audioSource;
        public int AudioBuffer => audioBuffer;
        public bool Looping => looping;
    }
}