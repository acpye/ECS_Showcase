using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Managers;
using OpenTK.Mathematics;

namespace OpenGL.GAME.Managers
{
    class MazeEscapeAudioManager : AudioManager
    {
        private static readonly string PowerUpAmbientSound = "GAME/Audio/buzz.wav";
        private static readonly string PowerUpCollectedSound = "GAME/Audio/pop.wav";
        private static readonly string PlayerShootSound = "GAME/Audio/gun.wav";
        private static readonly string DroneDestroyedSound = "GAME/Audio/cannon.wav";
        private static readonly string PlayerHitSound = "GAME/Audio/bell.wav";

        public static void PlayPowerUpCollected(Vector3 position)
        {
            PlayOneShot(PowerUpCollectedSound, position);
        }

        public static void PlayPlayerShoot(Vector3 position)
        {
            PlayOneShot(PlayerShootSound, position);
        }

        public static void PlayDroneDestroyed(Vector3 position)
        {
            PlayOneShot(DroneDestroyedSound, position);
        }

        public static void PlayPlayerHit(Vector3 position)
        {
            PlayOneShot(PlayerHitSound, position);
        }

        public static ComponentAudio CreatePowerUpAmbientAudio()
        {
            return CreateAmbientAudio(PowerUpAmbientSound);
        }
    }
}