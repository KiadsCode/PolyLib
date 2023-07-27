using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace PolyLib
{
    public class Polygon2D
    {
        private List<Vector2> _points = new List<Vector2>();

        public Vector2[] Points
        {
            get
            {
                return _points.ToArray();
            }
        }

        public Polygon2D()
        {
            _points = new List<Vector2>();
        }

        public Polygon2D(Vector2[] points)
        {
            _points = new List<Vector2>(points);
        }

        public void Add(Vector2 point)
        {
            _points.Add(point);
        }

        public void TranslatePoint(int index, Vector2 dir)
        {
            _points[index] += dir;
        }

        public void Edit(int index, Vector2 position)
        {
            _points[index] = position;
        }

        public Vector2 GetPosition()
        {
            float x = float.MaxValue;
            float y = float.MaxValue;

            foreach (Vector2 item in _points)
                if (x > item.X)
                    x = item.X;
            foreach (Vector2 item in _points)
                if (y > item.Y)
                    y = item.Y;

            return new Vector2(x, y);
        }

        public Rectangle GetRectangle()
        {
            int x = int.MaxValue;
            int y = int.MaxValue;
            int width = 0;
            int height = 0;

            foreach (Vector2 item in _points)
                if (x > item.X)
                    x = Convert.ToInt32(item.X);
            foreach (Vector2 item in _points)
                if (y > item.Y)
                    y = Convert.ToInt32(item.Y);
            foreach (Vector2 item in _points)
                if (width < item.X)
                    width = Convert.ToInt32(item.X);
            foreach (Vector2 item in _points)
                if (height < item.Y)
                    height = Convert.ToInt32(item.Y);
            return new Rectangle(x, y, width, height);
        }

        public void Translate(Vector2 dir)
        {
            for (int i = 0; i < _points.Count; i++)
                _points[i] += dir;
        }

        public void Remove(int index)
        {
            _points.RemoveAt(index);
        }

        public bool Intersects(Polygon2D poly)
        {
            Gjk gjk = new Gjk(this, poly);
            return gjk.CheckCollision();
        }

        public bool Intersects(Rectangle rectangle)
        {
            Polygon2D poly = ToPolygon(rectangle);
            Gjk gjk = new Gjk(this, poly);
            return gjk.CheckCollision();
        }

        public static bool Intersects(Polygon2D a, Polygon2D b)
        {
            Gjk gjk = new Gjk(a, b);
            return gjk.CheckCollision();
        }
        
        public static bool Intersects(Polygon2D a, Rectangle b)
        {
            Polygon2D bb = ToPolygon(b);
            Gjk gjk = new Gjk(a, bb);
            return gjk.CheckCollision();
        }

        public static Polygon2D ToPolygon(Rectangle rectangle)
        {
            Polygon2D polygon = new Polygon2D();
            polygon.Add(new Vector2(rectangle.X, rectangle.Y));
            polygon.Add(new Vector2(rectangle.X, rectangle.Y + rectangle.Height));
            polygon.Add(new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height));
            polygon.Add(new Vector2(rectangle.X + rectangle.Width, rectangle.Y));
            return polygon;
        }
    }
}
