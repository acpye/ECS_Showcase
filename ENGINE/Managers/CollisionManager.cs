using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Objects;

namespace OpenGL.ENGINE.Managers
{
    enum COLLISIONTYPE
    {
        AABB_AABB,
        CAMERA_AABB,
        CAMERA_LINE,
        CAMERA_SPHERE,
        LINE_LINE,
        POINT_AABB,
        SPHERE_LINE,
        SPHERE_SPHERE
    }

    struct Collision
    {
        public Entity entityA;
        public Entity entityB;
        public IComponent componentA;
        public COLLISIONTYPE collisionType;
    }

    abstract class CollisionManager
    {
        protected List<Collision> collisionManifold = new List<Collision>();
        public CollisionManager() { }
        public void ClearManifold() { collisionManifold.Clear(); }
        public void CollisionBetweenCamera(Entity entity, IComponent component, COLLISIONTYPE collisionType)
        {
            foreach (Collision collisions in collisionManifold)
            {
                if (collisions.entityA == entity && collisions.componentA == component)
                {
                    return;
                }
            }
            Collision collision;
            collision.entityA = entity;
            collision.entityB = null;
            collision.componentA = component;
            collision.collisionType = collisionType;
            collisionManifold.Add(collision);
        }

        public void CollisionBetweenEntities(Entity entityA, Entity entityB, COLLISIONTYPE collisionType)
        {
            foreach (Collision collisions in collisionManifold)
            {
                if (collisions.entityA == entityA && collisions.entityB == entityB ||
                    collisions.entityA == entityB && collisions.entityB == entityA)
                {
                    return;
                }
            }
            Collision collision;
            collision.entityA = entityA;
            collision.entityB = entityB;
            collision.componentA = null;
            collision.collisionType = collisionType;
            collisionManifold.Add(collision);
        }

        public abstract void ProcessCollisions();
    }
}