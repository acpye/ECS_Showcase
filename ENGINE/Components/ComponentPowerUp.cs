using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.ENGINE.Components
{
    enum PowerUpType
    {
        Health,
        Damage
    }

    class ComponentPowerUp : IComponent
    {
        public PowerUpType Type { get; }
        public byte Value { get; }

        public ComponentPowerUp(PowerUpType type, byte value = 1)
        {
            Type = type;
            Value = value;
        }

        public ComponentTypes ComponentType => ComponentTypes.COMPONENT_POWERUP;

        public void Close() { }
    }
}