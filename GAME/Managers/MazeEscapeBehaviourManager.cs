using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Managers;
using OpenGL.ENGINE.Objects;
using OpenGL.GAME.Objects;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenGL.GAME.Managers
{
    enum DroneState
    {
        Exploring,
        Chasing
    }

    class MazeEscapeBehaviourManager : BehaviourManager
    {
        private readonly Dictionary<Entity, DroneState> droneStates = new();
        private readonly Dictionary<Entity, Vector3> droneTargets = new();
        private readonly Random random = new();
        private readonly float lineOfSightRange = 100.0f;

        public static bool DebugDroneMovement { get; set; } = false;

        public MazeEscapeBehaviourManager(EntityManager entityManager, Camera camera, List<Vector3> pathNodes)
            : base(entityManager, camera, pathNodes)
        {
        }

        public override void Update(Entity entity, float deltaTime)
        {
        }
        protected override List<Vector3> FindPath(Vector3 start, Vector3 goal)
        {
            if (pathNodes == null || pathNodes.Count == 0)
            {
                return new List<Vector3> { start };
            }

            Vector3 nearestStart = pathNodes[0];
            Vector3 nearestGoal = pathNodes[0];
            float minDistStart = float.MaxValue;
            float minDistGoal = float.MaxValue;

            foreach (Vector3 node in pathNodes)
            {
                float ds = Vector3.Distance(start, node);
                if (ds < minDistStart)
                {
                    minDistStart = ds;
                    nearestStart = node;
                }

                float dg = Vector3.Distance(goal, node);
                if (dg < minDistGoal)
                {
                    minDistGoal = dg;
                    nearestGoal = node;
                }
            }

            if (Vector3.Distance(nearestStart, nearestGoal) < 0.1f)
            {
                return new List<Vector3> { nearestStart };
            }

            return new List<Vector3> { nearestStart, nearestGoal };
        }

        public Vector3? GetTargetPosition(Entity drone, Vector3 dronePosition, Vector3 playerPosition)
        {
            if (DebugDroneMovement)
            {
                return null;
            }

            if (!droneStates.ContainsKey(drone))
            {
                droneStates[drone] = DroneState.Exploring;
            }

            UpdateState(drone, dronePosition, playerPosition);

            return droneStates[drone] switch
            {
                DroneState.Chasing => playerPosition,
                DroneState.Exploring => GetExplorationTarget(drone, dronePosition),
                _ => null
            };
        }

        private void UpdateState(Entity drone, Vector3 dronePosition, Vector3 playerPosition)
        {
            bool canSeePlayer = CanSeePlayer(dronePosition, playerPosition);
            DroneState currentState = droneStates[drone];

            switch (currentState)
            {
                case DroneState.Exploring:
                    if (canSeePlayer)
                    {
                        droneStates[drone] = DroneState.Chasing;
                        Console.WriteLine($"{drone.Name} chasing");
                    }
                    break;

                case DroneState.Chasing:
                    if (!canSeePlayer)
                    {
                        droneStates[drone] = DroneState.Exploring;
                        droneTargets.Remove(drone);
                        Console.WriteLine($"{drone.Name} exploring");
                    }
                    break;
            }
        }

        private bool CanSeePlayer(Vector3 dronePosition, Vector3 playerPosition)
        {
            float distance = Vector3.Distance(dronePosition, playerPosition);
            if (distance > lineOfSightRange)
            {
                return false;
            }

            return HasLineOfSight(dronePosition, playerPosition);
        }

        private bool HasLineOfSight(Vector3 from, Vector3 to)
        {
            Entity mazeEntity = entityManager.FindEntity("Maze");
            if (mazeEntity == null)
            {
                return true;
            }

            List<ComponentCollisionAABB> aabbComponents = mazeEntity.Components.Where(c => c.ComponentType == ComponentTypes.COMPONENT_COLLISION_AABB).Cast<ComponentCollisionAABB>().ToList();

            ComponentPosition positionComponent = (ComponentPosition)mazeEntity.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_POSITION);

            Vector3 mazePosition = positionComponent?.Position ?? Vector3.Zero;

            foreach (ComponentCollisionAABB aabb in aabbComponents)
            {
                Vector3 minimum = mazePosition + aabb.Min;
                Vector3 maximum = mazePosition + aabb.Max;

                if (RayIntersectsAABB(from, to, minimum, maximum))
                {
                    return false;
                }
            }

            return true;
        }

        private Vector3 GetExplorationTarget(Entity drone, Vector3 currentPosition)
        {
            ComponentPath pathComponent = (ComponentPath)drone.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_PATH);

            if (pathComponent == null)
            {
                return currentPosition;
            }

            if (!droneTargets.ContainsKey(drone) ||
                Vector3.Distance(currentPosition, droneTargets[drone]) < 1.0f)
            {
                List<Vector3> nodes = pathComponent.PathNodes;
                Vector3 newTarget = nodes[random.Next(nodes.Count)];
                droneTargets[drone] = newTarget;
            }

            return droneTargets[drone];
        }

        private bool RayIntersectsAABB(Vector3 rayStart, Vector3 rayEnd, Vector3 min, Vector3 max)
        {
            Vector3 direction = rayEnd - rayStart;
            float tMin = 0.0f;
            float tMax = 1.0f;

            const float epsilon = 0.01f;

            Vector3 shrunkMin = min + new Vector3(epsilon, epsilon, epsilon);
            Vector3 shrunkMax = max - new Vector3(epsilon, epsilon, epsilon);

            for (int i = 0; i < 3; i++)
            {
                float start = i == 0 ? rayStart.X : i == 1 ? rayStart.Y : rayStart.Z;
                float directions = i == 0 ? direction.X : i == 1 ? direction.Y : direction.Z;
                float minimumValue = i == 0 ? shrunkMin.X : i == 1 ? shrunkMin.Y : shrunkMin.Z;
                float maximumValue = i == 0 ? shrunkMax.X : i == 1 ? shrunkMax.Y : shrunkMax.Z;

                if (Math.Abs(directions) < 1e-6f)
                {
                    if (start < minimumValue || start > maximumValue)
                    {
                        return false;
                    }
                }
                else
                {
                    float t1 = (minimumValue - start) / directions;
                    float t2 = (maximumValue - start) / directions;

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
            return tMin < tMax && tMin < 1.0f && tMax > 0.0f;
        }

        public DroneState GetDroneState(Entity drone)
        {
            return droneStates.TryGetValue(drone, out DroneState state) ? state : DroneState.Exploring;
        }

        public void ResetDroneState(Entity drone)
        {
            droneStates[drone] = DroneState.Exploring;
            droneTargets.Remove(drone);
        }
    }
}