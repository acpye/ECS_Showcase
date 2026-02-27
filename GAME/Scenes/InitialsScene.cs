using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;
using System;
using OpenGL.ENGINE.Managers;
using OpenGL.ENGINE.Scenes;
using OpenGL.GAME.Objects;

namespace OpenGL.GAME.Scenes
{
    class InitialsScene : Scene
    {
        private readonly int finalScore;
        private char[] initials = ['A', 'A', 'A'];
        private int currentIndex = 0;
        private bool keyProcessed = false;

        public InitialsScene(SceneManager sceneManager, int score) : base(sceneManager)
        {
            finalScore = score;

            sceneManager.Title = "Enter Initials";
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            sceneManager.keyboardDownDelegate += OnKeyDown;

            GL.ClearColor(0.1f, 0.1f, 0.2f, 1.0f);
        }

        public override void Update(FrameEventArgs e)
        {
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Size.X, sceneManager.Size.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Size.X, 0, sceneManager.Size.Y, -1, 1);

            float centerX = sceneManager.Size.X * 0.5f;

            // Title
            SKPaint titlePaint = new()
            {
                TextSize = 60,
                TextAlign = SKTextAlign.Center,
                IsAntialias = true,
                Color = SKColors.Gold,
                Style = SKPaintStyle.Fill
            };
            GUI.DrawText("GAME OVER", centerX, 100, titlePaint);

            // Score
            SKPaint scorePaint = new()
            {
                TextSize = 40,
                TextAlign = SKTextAlign.Center,
                IsAntialias = true,
                Color = SKColors.White,
                Style = SKPaintStyle.Fill
            };
            GUI.DrawText($"Final Score: {finalScore:D5}", centerX, 180, scorePaint);

            // Instructions
            SKPaint instructionPaint = new()
            {
                TextSize = 24,
                TextAlign = SKTextAlign.Center,
                IsAntialias = true,
                Color = SKColors.LightGray,
                Style = SKPaintStyle.Fill
            };
            GUI.DrawText("Enter your initials", centerX, 260, instructionPaint);
            GUI.DrawText("W/S to change letter, A/D to move to next letter, ENTER to confirm", centerX, 300, instructionPaint);

            float spacing = 80;
            float startX = centerX - spacing;

            for (int i = 0; i < 3; i++)
            {
                SKPaint letterPaint = new()
                {
                    TextSize = 80,
                    TextAlign = SKTextAlign.Center,
                    IsAntialias = true,
                    Color = i == currentIndex ? SKColors.Yellow : SKColors.White,
                    Style = SKPaintStyle.Fill
                };
                GUI.DrawText(initials[i].ToString(), startX + i * spacing, 400, letterPaint);

                if (i == currentIndex)
                {
                    SKPaint underlinePaint = new()
                    {
                        TextSize = 80,
                        TextAlign = SKTextAlign.Center,
                        IsAntialias = true,
                        Color = SKColors.Yellow,
                        Style = SKPaintStyle.Fill
                    };
                    GUI.DrawText("_", startX + i * spacing, 420, underlinePaint);
                }
            }

            GUI.Render();
        }

        private void OnKeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.W:
                    initials[currentIndex]++;
                    if (initials[currentIndex] > 'Z') 
                        initials[currentIndex] = 'A';
                    break;

                case Keys.S:
                    initials[currentIndex]--;
                    if (initials[currentIndex] < 'A') initials[currentIndex] = 'Z';
                    break;

                case Keys.A:
                    currentIndex = Math.Max(0, currentIndex - 1);
                    break;

                case Keys.D:
                    currentIndex = Math.Min(2, currentIndex + 1);
                    break;

                case Keys.Enter:
                    string playerInitials = new string(initials);
                    HighScoreClient.AddHighScore(playerInitials, finalScore);
                    Console.WriteLine($"Score saved: {playerInitials} - {finalScore}");
                    sceneManager.StartLeaderboard();
                    break;
            }
        }

        public override void Close()
        {
            sceneManager.keyboardDownDelegate -= OnKeyDown;
        }
    }
}