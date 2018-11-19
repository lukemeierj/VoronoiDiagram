using System;
using System.Collections.Generic;

namespace VoronoiBowyerWatson
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(5, 5));
            points.Add(new Point(400, 400));
            points.Add(new Point(20, 15));
            points.Add(new Point(100, 350));
            points.Add(new Point(25, 350));
            points.Add(new Point(300, 10));

            Triangulation tri = new Triangulation(points);
            tri.Triangulate();
            Console.WriteLine("Hello World!");
        }
    }
}
