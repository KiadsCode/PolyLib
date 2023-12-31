﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PolyLib
{
    public class LineBatch
    {
        private bool _endMissed;
        private bool _started;
        private GraphicsDevice _graphicsDevice;
        private List<VertexPositionColor> _verticies = new List<VertexPositionColor>();
        private BasicEffect _effect;

        public LineBatch(GraphicsDevice graphics)
        {
            _graphicsDevice = graphics;
            _effect = new BasicEffect(_graphicsDevice);
            Matrix world = Matrix.Identity;
            Matrix view = Matrix.CreateTranslation(-_graphicsDevice.Viewport.Width / 2, -_graphicsDevice.Viewport.Height / 2, 0);
            Matrix projection = Matrix.CreateOrthographic(_graphicsDevice.Viewport.Width, -_graphicsDevice.Viewport.Height, -10, 10);
            _effect.World = world;
            _effect.View = view;
            _effect.VertexColorEnabled = true;
            _effect.Projection = projection;
            _effect.DiffuseColor = Color.White.ToVector3();
            _endMissed = true;
        }

        public LineBatch(GraphicsDevice graphics, bool cares_about_begin_without_end)
        {
            _endMissed = cares_about_begin_without_end;
            _graphicsDevice = graphics;
            _effect = new BasicEffect(_graphicsDevice);
            Matrix world = Matrix.Identity;
            Matrix view = Matrix.CreateTranslation(-_graphicsDevice.Viewport.Width / 2, -_graphicsDevice.Viewport.Height / 2, 0);
            Matrix projection = Matrix.CreateOrthographic(_graphicsDevice.Viewport.Width, -_graphicsDevice.Viewport.Height, -10, 10);
            _effect.World = world;
            _effect.View = view;
            _effect.VertexColorEnabled = true;
            _effect.Projection = projection;
            _effect.DiffuseColor = Color.White.ToVector3();
        }

        public void Draw(Vector2 start, Vector2 end, Color color)
        {
            _verticies.Add(new VertexPositionColor(new Vector3(start, 0f), color));
            _verticies.Add(new VertexPositionColor(new Vector3(end, 0f), color));
        }

        public void DrawPolygon(Polygon2D polygon, Color color)
        {
            for (int i = 0; i < polygon.Points.Length; i++)
            {
                if (i + 1 < polygon.Points.Length)
                Draw(polygon.Points[i], polygon.Points[i + 1], color);
                else
                    Draw(polygon.Points[i], polygon.Points[0], color);
            }
        }

        public void End()
        {
            if (!_started)
                if (_endMissed)
                    throw new ArgumentException("Please add begin before end!");
                else
                    Begin();
            if (_verticies.Count > 0)
            {
                VertexBuffer vb = new VertexBuffer(_graphicsDevice, typeof(VertexPositionColor), _verticies.Count, BufferUsage.WriteOnly);
                vb.SetData<VertexPositionColor>(_verticies.ToArray());
                _graphicsDevice.SetVertexBuffer(vb);

                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    _graphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, _verticies.Count / 2);
                }
            }
            _started = false;
        }

        public void Begin()
        {
            if (_started)
                if (_endMissed)
                    throw new ArgumentException("You forgot end.");
                else
                    End();
            _verticies.Clear();
            _started = true;
        }
    }
}
