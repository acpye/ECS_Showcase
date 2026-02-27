using OpenGL.ENGINE.Components;
using System;

namespace OpenGL.GAME.Components
{
    class ComponentHealth : IComponent
    {
        public byte Health { get; set; }

        public ComponentHealth(byte health)
        {
            Health = health;
        }

        public void TakeDamage(byte damage)
        {
            if (damage >= Health)
            {
                Health = 0;
            }
            else
            {
                Health -= damage;
            }
        }

        public ComponentTypes ComponentType => ComponentTypes.COMPONENT_HEALTH;

        public void Close() { }
    }
}