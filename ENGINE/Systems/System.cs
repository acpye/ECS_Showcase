using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Objects;
using System.Collections.Generic;

namespace OpenGL.ENGINE.Systems
{
    abstract class System
    {
        public IComponent GetComponent(Entity entity, ComponentTypes componentType)
        {
            List<IComponent> components = entity.Components;

            IComponent iComponent = components.Find(delegate (IComponent component)
            {
                return component.ComponentType == componentType;
            });

            return iComponent;
        }

        public List<IComponent> GetComponentList(Entity entity, ComponentTypes componentType)
        {
            List<IComponent> components = entity.Components;

            for (int i  = components.Count - 1; i >= 0; i--)
            {
                IComponent component = components[i];
                if (components[i].ComponentType != componentType)
                {
                    components.RemoveAt(i);
                }
            }
            return components;
        }


        public abstract void OnAction(Entity entity);

        // Property signatures: 
        public string Name
        {
            get;
        }
    }
}
