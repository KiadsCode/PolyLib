using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PolyLib
{
    internal class Gjk
    {
        private Polygon2D _polygonA;
        private Polygon2D _polygonB;

        public Gjk(Polygon2D polygonA, Polygon2D polygonB)
        {
            _polygonA = polygonA;
            _polygonB = polygonB;
        }

        public bool CheckCollision()
        {
            List<Vector2> simplex = new List<Vector2>();
            Vector2 direction = _polygonA.Points[0] - _polygonB.Points[0];
            simplex.Add(Support(_polygonA, _polygonB, direction));

            direction = -direction;

            while (true)
            {
                simplex.Add(Support(_polygonA, _polygonB, direction));

                if (Vector2.Dot(simplex[simplex.Count - 1], direction) <= 0)
                    return false;

                if (DoSimplex(ref simplex, ref direction))
                    return true;
            }
        }

        private Vector2 Support(Polygon2D polygonA, Polygon2D polygonB, Vector2 direction)
        {
            Vector2 pointA = GetFarthestPointInDirection(new List<Vector2>(polygonA.Points), direction);
            Vector2 pointB = GetFarthestPointInDirection(new List<Vector2>(polygonB.Points), -direction);

            return pointA - pointB;
        }

        private Vector2 GetFarthestPointInDirection(List<Vector2> points, Vector2 direction)
        {
            Vector2 farthestPoint = points[0];
            float maxDistance = Vector2.Dot(points[0], direction);

            for (int i = 1; i < points.Count; i++)
            {
                float distance = Vector2.Dot(points[i], direction);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farthestPoint = points[i];
                }
            }

            return farthestPoint;
        }

        private bool DoSimplex(ref List<Vector2> simplex, ref Vector2 direction)
        {
            Vector2 a = simplex[simplex.Count - 1];
            Vector2 ao = -a;

            if (simplex.Count == 3)
            {
                Vector2 b = simplex[simplex.Count - 2];
                Vector2 c = simplex[simplex.Count - 3];

                Vector2 ab = b - a;
                Vector2 ac = c - a;

                Vector2 abPerpendicular = TripleProduct(ac, ab, ab);
                Vector2 acPerpendicular = TripleProduct(ab, ac, ac);

                if (Vector2.Dot(abPerpendicular, ao) > 0)
                {
                    simplex.RemoveAt(simplex.Count - 3);
                    direction = abPerpendicular;
                }
                else if (Vector2.Dot(acPerpendicular, ao) > 0)
                {
                    simplex.RemoveAt(simplex.Count - 2);
                    direction = acPerpendicular;
                }
                else return true;
            }
            else
            {
                Vector2 b = simplex[simplex.Count - 2];
                Vector2 ab = b - a;

                direction = TripleProduct(ab, ao, ab);
            }

            return false;
        }

        private Vector2 TripleProduct(Vector2 a, Vector2 b, Vector2 c)
        {
            return b * Vector2.Dot(c, a) - a * Vector2.Dot(c, b);
        }
    }
}
