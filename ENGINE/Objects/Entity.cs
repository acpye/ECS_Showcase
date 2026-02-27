using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenGL.ENGINE.Components;

namespace OpenGL.ENGINE.Objects
{
    class Entity
    {
        string name;
        List<IComponent> componentList = new List<IComponent>();
        ComponentTypes mask;
 
        public Entity(string name)
        {
            this.name = name;
        }

        /// <summary>Adds a single component</summary>
        public void AddComponent(IComponent component)
        {
            Debug.Assert(component != null, "Component cannot be null");

            componentList.Add(component);
            mask |= component.ComponentType;
        }

        public void RemoveComponent(IComponent component)
        {
                component.Close();
                componentList.Remove(component);
                mask &= ~component.ComponentType;
        }

        public string Name
        {
            get { return name; }
        }

        public ComponentTypes Mask
        {
            get { return mask; }
        }

        public List<IComponent> Components
        {
            get { return componentList; }
        }

        public void Close()
        {
            foreach (IComponent component in Components)
            {
                component.Close();
            }
        }
    }
}
