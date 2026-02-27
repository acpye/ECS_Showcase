using System;

namespace OpenGL.ENGINE.Components
{
    [Flags]
    public enum ComponentTypes
    {
        COMPONENT_NONE              = 0,
	    COMPONENT_POSITION          = 1 << 0,
        COMPONENT_GEOMETRY          = 1 << 1,
        COMPONENT_VELOCITY          = 1 << 2,
        COMPONENT_SHADER            = 1 << 3,
        COMPONENT_AUDIO             = 1 << 4,
        COMPONENT_COLLISION_SPHERE  = 1 << 5,
        COMPONENT_COLLISION_LINE    = 1 << 6,
        COMPONENT_COLLISION_AABB    = 1 << 7,
        COMPONENT_PATH              = 1 << 8,
        COMPONENT_ROTATION          = 1 << 9,
        COMPONENT_HEALTH            = 1 << 10,
        COMPONENT_POWERUP           = 1 << 11
    }

    interface IComponent
    {
        ComponentTypes ComponentType { get; }
        void Close() { }
    }
}
