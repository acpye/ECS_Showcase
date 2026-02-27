using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Managers;
using OpenGL.ENGINE.Objects;
using OpenGL.ENGINE.Scenes;
using OpenGL.ENGINE.Systems;
using OpenGL.GAME.Components;
using OpenGL.GAME.Managers;
using OpenGL.GAME.Objects;
using OpenGL.GAME.Systems;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;

namespace OpenGL.GAME.Scenes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class GameScene : Scene
    {
        public static float dt = 0;
        private EntityManager entityManager;
        private SystemManager systemManager;
        private CollisionManager collisionManager;
        private InputManager inputManager;
        public Camera camera;
        public Skybox skybox;
        public static GameScene gameInstance;
        private byte playerLives = 3;
        private byte playerDamage = 1;
        public float PlayerSpeed = 0.25f;
        private Vector3 playerStartPosition = new Vector3(-30.0f, 1.5f, 30.0f);
        private Vector3 playerTargetPosition = new Vector3(-30.0f, 1.5f, 0.0f);
        private PathNodes pathNodes = new PathNodes();
        private List<Drone> drones = new List<Drone>();
        private readonly Dictionary<PowerUpType, int> collectedPowerUps = new();
        private BehaviourManager behaviourManager;

        private int score = 0;
        private float gameTime = 0f;
        private int dronesDestroyed = 0;
        private const int BaseScorePerDrone = 1000;
        private const int TimeBonus = 50;
        private const float ParTimePerDrone = 60f;
        private const int LivesBonus = 500;
        private const int PowerUpBonus = 250;

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();

            camera = new Camera(playerStartPosition, playerTargetPosition, sceneManager.Size.X / (float)sceneManager.Size.Y, 0.1f, 100f);
            skybox = new Skybox(camera.cameraPosition);

            behaviourManager = new MazeEscapeBehaviourManager(entityManager, camera, pathNodes.Nodes);
            inputManager = new MazeEscapeInputManager(camera, entityManager);
            collisionManager = new MazeEscapeCollisionManager(camera, entityManager);

            foreach (PowerUpType t in Enum.GetValues(typeof(PowerUpType)))
            {
                collectedPowerUps[t] = 0;
            }

            sceneManager.Title = "Game";
            sceneManager.renderer = Render;
            sceneManager.updater = Update;

            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            CreateEntities();
            CreateSystems();
            CreateAudios();
        }

        public void PlayerHit()
        {
            MazeEscapeAudioManager.PlayPlayerHit(camera.cameraPosition);
            playerLives--;
            if (playerLives > 0)
            {
                ResetPositions();
            }
            else
            {
                EndGame(false);
            }
        }

        private void EndGame(bool victory)
        {
            if (victory)
            {
                int livesBonus = playerLives * LivesBonus;
                score += livesBonus;
                Console.WriteLine($"Victory, final score: {score}");
            }
            else
            {
                Console.WriteLine($"Game Over, final score: {score}");
            }

            sceneManager.StartInitials(score);
        }

        public void CollectPowerUp(PowerUpType type, byte value, Vector3 position)
        {
            MazeEscapeAudioManager.PlayPowerUpCollected(position);

            if (collectedPowerUps.ContainsKey(type))
            {
                collectedPowerUps[type] += value;
            }
            else
            {
                collectedPowerUps[type] = value;
            }
            score += PowerUpBonus * value;

            switch (type)
            {
                case PowerUpType.Health:
                    playerLives = (byte)Math.Min(playerLives + value, 255);
                    break;

                case PowerUpType.Damage:
                    playerDamage = (byte)Math.Min(playerDamage + value, 255);
                    break;
            }
        }

        public byte GetPlayerDamage() => playerDamage;

        public void OnDroneDestroyed(Drone drone, Vector3 position)
        {
            MazeEscapeAudioManager.PlayDroneDestroyed(position);
            dronesDestroyed++;

            float averageTimePerDrone = gameTime / dronesDestroyed;
            int timeBonus = 0;
            if (averageTimePerDrone < ParTimePerDrone)
            {
                timeBonus = (int)((ParTimePerDrone - averageTimePerDrone) * TimeBonus);
            }

            score += BaseScorePerDrone + timeBonus;

            Console.WriteLine($"Drone destroyed +{BaseScorePerDrone + timeBonus} points (Time bonus: {timeBonus})");

            CheckWinCondition();
        }

        private void CheckWinCondition()
        {
            int aliveDrones = 0;
            foreach (Drone drone in drones)
            {
                ComponentHealth health = (ComponentHealth)drone.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_HEALTH);
                if (health != null && health.Health > 0)
                {
                    aliveDrones++;
                }
            }

            if (aliveDrones == 0)
            {
                EndGame(true);
            }
        }

        private void ResetPositions()
        {
            camera.cameraPosition = playerStartPosition;

            foreach (Drone drone in drones)
            {
                drone.ResetToStart();
            }
        }

        private void CreateEntities()
        {
            entityManager.AddEntity(skybox);

            entityManager.AddEntity(new Maze());
            entityManager.AddEntity(new MazePlane());

            Drone drone1 = new Drone(new Vector3(-5.0f, 1.5f, 7.0f), pathNodes.Nodes);
            entityManager.AddEntity(drone1);
            drones.Add(drone1);

            Drone drone2 = new Drone(new Vector3(-5.0f, 1.5f, 55.0f), pathNodes.Nodes);
            entityManager.AddEntity(drone2);
            drones.Add(drone2);

            entityManager.AddEntity(new PowerUp(new Vector3(-5.0f, 1.5f, 25.0f), PowerUpType.Damage, 1));
            entityManager.AddEntity(new PowerUp(new Vector3(-55.0f, 1.5f, 25.0f), PowerUpType.Health, 1));

            foreach (Entity entity in entityManager.Entities())
            {
                Console.WriteLine($"Spawned entity: {entity.Name}");
            }
        }

        private void CreateSystems()
        {
            ENGINE.Systems.System newSystem;

            newSystem = new SystemRender(this);
            systemManager.AddSystem(newSystem);
            newSystem = new SystemPathfinding((MazeEscapeBehaviourManager)behaviourManager, entityManager, camera);
            systemManager.AddSystem(newSystem);
            newSystem = new SystemPhysics();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemAudio(camera);
            systemManager.AddSystem(newSystem);
            newSystem = new SystemCollisionAABB_AABB(collisionManager, entityManager);
            systemManager.AddSystem(newSystem);
            newSystem = new SystemCollisionCamera_AABB(collisionManager, camera);
            systemManager.AddSystem(newSystem);
        }


        private void CreateAudios()
        {
            foreach (Entity entity in entityManager.Entities())
            {
                if (entity.Name == "PowerUp")
                {
                    ComponentAudio powerUpAudio = (ComponentAudio)entity.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_AUDIO);
                    powerUpAudio?.Play();
                }
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Update(FrameEventArgs e)
        {
            dt = (float)e.Time;
            gameTime += dt;

            collisionManager.ProcessCollisions();

            inputManager.ProcessInput(sceneManager.KeyboardState, sceneManager.MouseState);

            camera.UpdateView();
            skybox.FollowCamera(camera);

            systemManager.ActionSystems(entityManager);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Size.X, sceneManager.Size.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            systemManager.ActionSystems(entityManager);

            RenderGUI();
        }

        private void RenderGUI()
        {
            GUI.DrawText($"Score: {score:D5}", 30, 70, 30, 255, 255, 255);
            GUI.DrawText($"Lives: {playerLives}", 30, 110, 30, 255, 255, 255);
            GUI.DrawText($"Time: {gameTime:F1}s", 30, 150, 24, 255, 255, 255);

            List<string> enabledDebugs = new List<string>();
            if (MazeEscapeBehaviourManager.DebugDroneMovement)
            {
                enabledDebugs.Add("DroneMovement");
            }
            if (MazeEscapeCollisionManager.DebugNoClip)
            {
                enabledDebugs.Add("NoClip");
            }
            string debugText = "Enabled debugs: " + (enabledDebugs.Count > 0 ? string.Join(", ", enabledDebugs) : "None");
            GUI.DrawText(debugText, 30, 190, 20, 200, 200, 0);

            int y = 220;
            GUI.DrawText($"Drones destroyed: {dronesDestroyed}/{drones.Count}", 30, y, 20, 255, 255, 255);
            y += 28;
            int idx = 1;
            foreach (Drone drone in drones)
            {
                ComponentHealth healthComponent = (ComponentHealth)drone.Components.Find(c => c.ComponentType == ComponentTypes.COMPONENT_HEALTH);
                byte hp = healthComponent?.Health ?? 0;
                if (hp <= 0)
                {
                    GUI.DrawText($"Drone {idx}: DESTROYED", 30, y, 18, 255, 100, 100);
                    y += 22;
                }
                idx++;
            }

            List<string> collectedLabels = new List<string>();
            foreach (PowerUpType type in Enum.GetValues(typeof(PowerUpType)))
            {
                int count = collectedPowerUps.TryGetValue(type, out int c) ? c : 0;
                if (count > 0)
                {
                    string label = type switch
                    {
                        PowerUpType.Health => "Bonus Health",
                        PowerUpType.Damage => "Bonus Damage",
                        _ => type.ToString()
                    };
                    collectedLabels.Add($"{label} x{count}");
                }
            }
            string collectedText = "Collected power-ups: " + (collectedLabels.Count > 0 ? string.Join(", ", collectedLabels) : "None");
            GUI.DrawText(collectedText, 30, y + 8, 18, 200, 255, 200);

            GUI.Render();
        }

        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        public override void Close()
        {
            entityManager.CloseAll();

            MazeEscapeAudioManager.CloseAllSources();
        }
    }
}