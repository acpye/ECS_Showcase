using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;
using System.Collections.Generic;
using OpenGL.ENGINE.Managers;
using OpenGL.ENGINE.Scenes;
using OpenGL.GAME.Objects;

namespace OpenGL.GAME.Scenes
{
    class LeaderboardScene : Scene
    {
        private readonly List<HighScoreEntry> highScores;

        public LeaderboardScene(SceneManager sceneManager) : base(sceneManager)
        {
            highScores = HighScoreClient.GetHighScores();

            sceneManager.Title = "Leaderboard";
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            sceneManager.mouseDelegate += OnMousePressed;
            sceneManager.keyboardDownDelegate += OnKeyDown;

            GL.ClearColor(0.05f, 0.05f, 0.15f, 1.0f);
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

            SKPaint titlePaint = new()
            {
                TextSize = 70,
                StrokeWidth = 2,
                TextAlign = SKTextAlign.Center,
                IsAntialias = true,
                Color = SKColors.Gold,
                Style = SKPaintStyle.Fill
            };
            GUI.DrawText("LEADERBOARD", centerX, 100, titlePaint);

            SKPaint headerPaint = new()
            {
                TextSize = 24,
                TextAlign = SKTextAlign.Center,
                IsAntialias = true,
                Color = SKColors.LightGray,
                Style = SKPaintStyle.Fill
            };
            GUI.DrawText("RANK", centerX - 200, 170, headerPaint);
            GUI.DrawText("INITIALS", centerX, 170, headerPaint);
            GUI.DrawText("SCORE", centerX + 200, 170, headerPaint);

            int y = 220;
            int rank = 1;
            foreach (HighScoreEntry entry in highScores)
            {
                SKColor rowColor = rank switch
                {
                    1 => SKColors.Gold,
                    2 => SKColors.Silver,
                    3 => new SKColor(205, 127, 50), // Bronze
                    _ => SKColors.White
                };

                SKPaint rowPaint = new()
                {
                    TextSize = 28,
                    TextAlign = SKTextAlign.Center,
                    IsAntialias = true,
                    Color = rowColor,
                    Style = SKPaintStyle.Fill
                };

                GUI.DrawText(rank.ToString(), centerX - 200, y, rowPaint);
                GUI.DrawText(entry.Initials, centerX, y, rowPaint);
                GUI.DrawText(entry.Score.ToString(), centerX + 200, y, rowPaint);

                y += 40;
                rank++;
            }

            SKPaint instructionPaint = new()
            {
                TextSize = 20,
                TextAlign = SKTextAlign.Center,
                IsAntialias = true,
                Color = SKColors.Gray,
                Style = SKPaintStyle.Fill
            };
            GUI.DrawText("Press ESC or Click to return to menu", centerX, sceneManager.Size.Y - 50, instructionPaint);

            GUI.Render();
        }

        private void OnMousePressed(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                sceneManager.StartMenu();
            }
        }

        private void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Escape)
            {
                sceneManager.StartMenu();
            }
        }

        public override void Close()
        {
            sceneManager.mouseDelegate -= OnMousePressed;
            sceneManager.keyboardDownDelegate -= OnKeyDown;
        }
    }
}