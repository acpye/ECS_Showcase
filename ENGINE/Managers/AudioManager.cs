using OpenGL.ENGINE.Components;
using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenGL.ENGINE.Managers
{
    abstract class AudioManager
    {
        private static readonly List<ComponentAudio> activeSources = new();

        protected static void PlayOneShot(string audioFile, Vector3 position)
        {
            CleanupFinishedSources();

            ComponentAudio audio = new ComponentAudio(audioFile, looping: false);
            audio.SetPosition(position);
            audio.Play();
            activeSources.Add(audio);
        }

        protected static ComponentAudio CreateAmbientAudio(string audioFile)
        {
            return new ComponentAudio(audioFile, looping: true);
        }

        private static void CleanupFinishedSources()
        {
            for (int i = activeSources.Count - 1; i >= 0; i--)
            {
                AL.GetSource(activeSources[i].AudioSource, ALGetSourcei.SourceState, out int state);
                if (state == (int)ALSourceState.Stopped)
                {
                    activeSources[i].Close();
                    activeSources.RemoveAt(i);
                }
            }
        }

        public static void CloseAllSources()
        {
            foreach (ComponentAudio audio in activeSources)
            {
                audio.Close();
            }
            activeSources.Clear();
        }
    }
}