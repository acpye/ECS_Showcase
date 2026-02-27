using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Objects;
using OpenGL.GAME.Components;
using OpenGL.GAME.Managers;
using OpenTK.Mathematics;

namespace OpenGL.GAME.Objects
{
    class PowerUp : Entity
    {
        private const string DefaultGeometryPath = "GAME/Geometry/Moon/moon.obj";

        public PowerUp(Vector3 position, PowerUpType type, byte value = 1)
            : base("PowerUp")
        {
            AddComponent(new ComponentPosition(position));
            AddComponent(new ComponentGeometry(DefaultGeometryPath));
            AddComponent(new ComponentShaderDefault());
            AddComponent(new ComponentCollisionAABB(DefaultGeometryPath));
            AddComponent(new ComponentPowerUp(type, value));
            AddComponent(MazeEscapeAudioManager.CreatePowerUpAmbientAudio()); // 3D positional audio
        }
    }
}