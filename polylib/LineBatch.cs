using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PolyLib
{
    public class LineBatch
    {
        bool _endMissed;
        bool _started;
        GraphicsDevice GraphicsDevice;
        List<VertexPositionColor> verticies = new List<VertexPositionColor>();
        BasicEffect effect;
        public LineBatch(GraphicsDevice graphics)
        {
            GraphicsDevice = graphics;
            effect = new BasicEffect(GraphicsDevice);
            Matrix world = Matrix.Identity;
            Matrix view = Matrix.CreateTranslation(-GraphicsDevice.Viewport.Width / 2, -GraphicsDevice.Viewport.Height / 2, 0);
            Matrix projection = Matrix.CreateOrthographic(GraphicsDevice.Viewport.Width, -GraphicsDevice.Viewport.Height, -10, 10);
            effect.World = world;
            effect.View = view;
            effect.VertexColorEnabled = true;
            effect.Projection = projection;
            effect.DiffuseColor = Color.White.ToVector3();
            _endMissed = true;
        }
        public LineBatch(GraphicsDevice graphics, bool cares_about_begin_without_end)
        {
            _endMissed = cares_about_begin_without_end;
            GraphicsDevice = graphics;
            effect = new BasicEffect(GraphicsDevice);
            Matrix world = Matrix.Identity;
            Matrix view = Matrix.CreateTranslation(-GraphicsDevice.Viewport.Width / 2, -GraphicsDevice.Viewport.Height / 2, 0);
            Matrix projection = Matrix.CreateOrthographic(GraphicsDevice.Viewport.Width, -GraphicsDevice.Viewport.Height, -10, 10);
            effect.World = world;
            effect.View = view;
            effect.VertexColorEnabled = true;
            effect.Projection = projection;
            effect.DiffuseColor = Color.White.ToVector3();
        }
        public void DrawAngledLineWithRadians(Vector2 start, float length, float radians, Color color)
        {
            Vector2 offset = new Vector2(
                (float)Math.Sin(radians) * length, //x
                -(float)Math.Cos(radians) * length //y
                );
            Draw(start, start + offset, color);
        }
        public void DrawOutLineOfRectangle(Rectangle rectangle, Color color)
        {
            Draw(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y), color);
            Draw(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X, rectangle.Y + rectangle.Height), color);
            Draw(new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), color);
            Draw(new Vector2(rectangle.X, rectangle.Y + rectangle.Height), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), color);
        }
        public void DrawOutLineOfTriangle(Vector2 point_1, Vector2 point_2, Vector2 point_3, Color color)
        {
            Draw(point_1, point_2, color);
            Draw(point_1, point_3, color);
            Draw(point_2, point_3, color);
        }
        float GetRadians(float angleDegrees)
        {
            return angleDegrees * ((float)Math.PI) / 180.0f;
        }
        public void DrawAngledLine(Vector2 start, float length, float angleDegrees, Color color)
        {
            DrawAngledLineWithRadians(start, length, GetRadians(angleDegrees), color);
        }
        public void Draw(Vector2 start, Vector2 end, Color color)
        {
            verticies.Add(new VertexPositionColor(new Vector3(start, 0f), color));
            verticies.Add(new VertexPositionColor(new Vector3(end, 0f), color));
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
        public void Draw(Vector3 start, Vector3 end, Color color)
        {
            verticies.Add(new VertexPositionColor(start, color));
            verticies.Add(new VertexPositionColor(end, color));
        }
        public void End()
        {
            if (!_started)
                if (_endMissed)
                    throw new ArgumentException("Please add begin before end!");
                else
                    Begin();
            if (verticies.Count > 0)
            {
                VertexBuffer vb = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), verticies.Count, BufferUsage.WriteOnly);
                vb.SetData<VertexPositionColor>(verticies.ToArray());
                GraphicsDevice.SetVertexBuffer(vb);

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, verticies.Count / 2);
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
            verticies.Clear();
            _started = true;
        }
    }
}
