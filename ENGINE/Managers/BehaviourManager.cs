using OpenGL.ENGINE.Objects;
using OpenGL.GAME.Objects;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenGL.ENGINE.Managers
{
    abstract class BehaviourManager
    {
        protected EntityManager entityManager;
        protected Camera camera;
        protected List<Vector3> pathNodes;

        protected BehaviourManager(EntityManager entityManager, Camera camera, List<Vector3> pathNodes)
        {
            this.entityManager = entityManager;
            this.camera = camera;
            this.pathNodes = pathNodes;
        }

        public abstract void Update(Entity entity, float deltaTime);

        protected abstract List<Vector3> FindPath(Vector3 start, Vector3 goal);
    }
}