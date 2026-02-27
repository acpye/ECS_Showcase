using System.Collections.Generic;
using OpenGL.ENGINE.Objects;

//using System.Diagnostics;

namespace OpenGL.ENGINE.Managers
{
    class EntityManager
    {
        List<Entity> entityList;

        public EntityManager()
        {
            entityList = new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            entityList.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            entityList.Remove(entity);
        }

        public Entity FindEntity(string name)
        {
            return entityList.Find(e => e.Name == name);
        }

        public List<Entity> Entities()
        {
            return entityList;
        }

        public void CloseAll()
        {
            foreach (Entity entity in entityList)
            {
                entity.Close();
            }
            entityList.Clear();
        }
    }
}
