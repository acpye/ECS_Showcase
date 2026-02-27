using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Managers;
using OpenGL.ENGINE.Objects;
using OpenGL.GAME.Managers;
using OpenGL.GAME.Objects;
using OpenGL.GAME.Scenes;
using OpenGL.ENGINE.Systems;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenGL.GAME.Systems
{
    class SystemPathfinding : OpenGL.ENGINE.Systems.System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_VELOCITY | ComponentTypes.COMPONENT_PATH;

        private readonly MazeEscapeBehaviourManager behaviourManager;
        private readonly EntityManager entityManager;
        private readonly Camera camera;

        public SystemPathfinding(MazeEscapeBehaviourManager behaviourManager, EntityManager entityManager, Camera camera)
        {
            this.behaviourManager = behaviourManager;
            this.entityManager = entityManager;
            this.camera = camera;
        }

        public string Name => "SystemPathfinding";

        public override void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) != MASK)
            {
                return;
            }

            ComponentPosition positionComponent = (ComponentPosition)GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
            ComponentVelocity velocityComponent = (ComponentVelocity)GetComponent(entity, ComponentTypes.COMPONENT_VELOCITY);
            ComponentPath pathComponent = (ComponentPath)GetComponent(entity, ComponentTypes.COMPONENT_PATH);
            ComponentRotation rotationComponent = (ComponentRotation)GetComponent(entity, ComponentTypes.COMPONENT_ROTATION);

            Vector3 currentPosition = positionComponent.Position;
            Vector3? targetPosition = behaviourManager.GetTargetPosition(entity, currentPosition, camera.cameraPosition);

            if (targetPosition == null)
            {
                velocityComponent.Velocity = Vector3.Zero;
                return;
            }

            List<Vector3> path = FindPath(currentPosition, targetPosition.Value, pathComponent.PathNodes);

            if (path.Count > 1)
            {
                Vector3 nextNode = path[1];

                if (Vector3.Distance(currentPosition, nextNode) < 0.5f && path.Count > 2)
                {
                    nextNode = path[2];
                }

                Vector3 direction = nextNode - currentPosition;
                float distance = direction.Length;

                if (distance > 0.01f)
                {
                    direction = Vector3.Normalize(direction);
                    velocityComponent.Velocity = direction * velocityComponent.Speed;

                    if (rotationComponent != null)
                    {
                        float targetYaw = MathF.Atan2(direction.X, direction.Z);
                        rotationComponent.Yaw = Interpolation(rotationComponent.Yaw, targetYaw, 5.0f * GameScene.dt);
                    }
                }
                else
                {
                    velocityComponent.Velocity = Vector3.Zero;
                }
            }
            else if (path.Count == 1)
            {
                Vector3 direction = targetPosition.Value - currentPosition;
                float distance = direction.Length;

                if (distance > 0.5f)
                {
                    direction = Vector3.Normalize(direction);
                    velocityComponent.Velocity = direction * velocityComponent.Speed;

                    if (rotationComponent != null)
                    {
                        float targetYaw = MathF.Atan2(direction.X, direction.Z);
                        rotationComponent.Yaw = Interpolation(rotationComponent.Yaw, targetYaw, 5.0f * GameScene.dt);
                    }
                }
                else
                {
                    velocityComponent.Velocity = Vector3.Zero;
                }
            }
            else
            {
                velocityComponent.Velocity = Vector3.Zero;
            }
        }

        private static float Interpolation(float current, float target, float t)
        {
            float diff = target - current;

            while (diff > MathF.PI)
            {
                diff -= MathF.PI * 2;
            }
            while (diff < -MathF.PI)
            {
                diff += MathF.PI * 2;
            }
            return current + diff * MathHelper.Clamp(t, 0f, 1f);
        }

        private List<Vector3> FindPath(Vector3 start, Vector3 goal, List<Vector3> nodes)
        {
            Vector3 startNode = GetNearestNode(start, nodes);
            Vector3 goalNode = GetNearestNode(goal, nodes);

            if (Vector3.Distance(startNode, goalNode) < 0.1f)
            {
                return new List<Vector3> { startNode };
            }

            List<PathNode> openSet = new List<PathNode>();
            HashSet<Vector3> closedSet = new HashSet<Vector3>();
            Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();

            Dictionary<Vector3, float> gScore = new Dictionary<Vector3, float>();
            Dictionary<Vector3, float> fScore = new Dictionary<Vector3, float>();

            foreach (Vector3 node in nodes)
            {
                gScore[node] = float.MaxValue;
                fScore[node] = float.MaxValue;
            }

            gScore[startNode] = 0;
            fScore[startNode] = Heuristic(startNode, goalNode);

            openSet.Add(new PathNode(startNode, fScore[startNode]));

            while (openSet.Count > 0)
            {
                openSet.Sort((a, b) => a.FScore.CompareTo(b.FScore));
                PathNode current = openSet[0];
                openSet.RemoveAt(0);

                if (Vector3.Distance(current.Position, goalNode) < 0.1f)
                {
                    return ReconstructPath(cameFrom, current.Position);
                }

                closedSet.Add(current.Position);

                foreach (Vector3 neighbour in GetNeighbours(current.Position, nodes))
                {
                    if (closedSet.Contains(neighbour))
                    { 
                        continue;
                    }
                    if (!HasLineOfSight(current.Position, neighbour))
                    {
                        continue;
                    }

                    float tentativeGScore = gScore[current.Position] + Vector3.Distance(current.Position, neighbour);

                    if (tentativeGScore < gScore[neighbour])
                    {
                        cameFrom[neighbour] = current.Position;
                        gScore[neighbour] = tentativeGScore;
                        fScore[neighbour] = gScore[neighbour] + Heuristic(neighbour, goalNode);

                        if (!openSet.Any(n => n.Position == neighbour))
                        {
                            openSet.Add(new PathNode(neighbour, fScore[neighbour]));
                        }
                    }
                }
            }
            return new List<Vector3> { startNode };
        }

        private float Heuristic(Vector3 a, Vector3 b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Z - b.Z);
        }

        private Vector3 GetNearestNode(Vector3 position, List<Vector3> nodes)
        {
            Vector3 nearest = nodes[0];
            float minDistance = float.MaxValue;

            foreach (Vector3 node in nodes)
            {
                float distance = Vector3.Distance(position, node);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = node;
                }
            }

            return nearest;
        }

        private List<Vector3> GetNeighbours(Vector3 node, List<Vector3> allNodes)
        {
            const float maxNeighbourDistance = 15.0f;
            return allNodes.Where(n => n != node && Vector3.Distance(node, n) <= maxNeighbourDistance).ToList();
        }

        private List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 current)
        {
            List<Vector3> path = new List<Vector3> { current };

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Insert(0, current);
            }

            return path;
        }

        public bool HasLineOfSight(Vector3 from, Vector3 to)
        {
            Entity mazeEntity = entityManager.FindEntity("Maze");
            if (mazeEntity == null)
            {
                return true;
            }

            List<ComponentCollisionAABB> aabbComponents = mazeEntity.Components
                .Where(c => c.ComponentType == ComponentTypes.COMPONENT_COLLISION_AABB)
                .Cast<ComponentCollisionAABB>()
                .ToList();

            ComponentPosition positionComponent = (ComponentPosition)mazeEntity.Components
                .Find(c => c.ComponentType == ComponentTypes.COMPONENT_POSITION);

            Vector3 mazePosition = positionComponent?.Position ?? Vector3.Zero;

            foreach (ComponentCollisionAABB aabb in aabbComponents)
            {
                Vector3 min = mazePosition + aabb.Min;
                Vector3 max = mazePosition + aabb.Max;

                if (RayIntersectsAABB(from, to, min, max))
                {
                    return false;
                }
            }

            return true;
        }

        private bool RayIntersectsAABB(Vector3 rayStart, Vector3 rayEnd, Vector3 min, Vector3 max)
        {
            Vector3 direction = rayEnd - rayStart;
            float tMin = 0.0f;
            float tMax = 1.0f;

            for (int i = 0; i < 3; i++)
            {
                float start = i == 0 ? rayStart.X : i == 1 ? rayStart.Y : rayStart.Z;
                float dir = i == 0 ? direction.X : i == 1 ? direction.Y : direction.Z;
                float minVal = i == 0 ? min.X : i == 1 ? min.Y : min.Z;
                float maxVal = i == 0 ? max.X : i == 1 ? max.Y : max.Z;

                if (Math.Abs(dir) < 1e-6f)
                {
                    if (start < minVal || start > maxVal)
                        return false;
                }
                else
                {
                    float t1 = (minVal - start) / dir;
                    float t2 = (maxVal - start) / dir;

                    if (t1 > t2)
                    {
                        (t1, t2) = (t2, t1);
                    }

                    tMin = Math.Max(tMin, t1);
                    tMax = Math.Min(tMax, t2);

                    if (tMin > tMax)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private struct PathNode
        {
            public Vector3 Position;
            public float FScore;

            public PathNode(Vector3 position, float fScore)
            {
                Position = position;
                FScore = fScore;
            }
        }
    }
}