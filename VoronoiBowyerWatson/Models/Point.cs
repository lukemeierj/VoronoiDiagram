using System;
using System.Drawing;
using System.Collections.Generic;

namespace VoronoiAlgorithms.Models
{
    public class Point : IComparable<Point>, IEquatable<Point>
    {
        public double x;
        public double y;
        public Color color;

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Point(int x, int y, Color c)
        {
            this.x = x;
            this.y = y;
            this.color = c;
        }

        public static double Distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));
        }

        public double Distance(Point a)
        {
            return Distance(this, a);
        }

        override public string ToString()
        {
            return "(" + x + ", " + y + ")";
        }

        // Compares by x-coordinate:
        // (What Bowyer-Watson wants)
        public int CompareTo(Point p)
        {
            if (x < p.x)
            {
                return -1;
            }
            else if (x > p.x)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public bool Equals(Point other)
        {
            return (x == other.x && y == other.y);
        }

        public static List<Point> GetRandomPoints (int numPoints, int min, int max) {
            Random rand = new Random();
            List<Point> points = new List<Point>();
            for (int i = 0; i < numPoints; i++)
            {
                int a = rand.Next(min, max);
                int b = rand.Next(min, max);
                Color randomColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                points.Add(new Point(a, b, randomColor));
            }
            return points;

        }
    }
}
