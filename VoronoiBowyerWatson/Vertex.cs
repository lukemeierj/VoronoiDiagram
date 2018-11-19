using System;
using System.Collections.Generic;
using System.Linq;


namespace VoronoiBowyerWatson
{

    public struct Point {
        public double x;
        public double y;
        public Point(double x, double y){
            this.x = x;
            this.y = y;
        }
        public static double Distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));
        }
        public double Distance(Point a){
            return Math.Sqrt(Math.Pow(a.x - this.x, 2) + Math.Pow(a.y - this.y, 2));
        }
    }

    public class Vertex
    {
        private List<Point> points;
        public List<Vertex> neighbors { get; private set; }

        public Point center {  get; private set; }
        public double radius { get; private set; }

        // TODO: make more unique sentinal 
        public static Vertex nullVertex = null;

        public Vertex(List<Point> points, List<Vertex> neighbors)
        {
            this.neighbors = neighbors;
            this.points    = points;
            this.center = CalculateCenter(points);
            this.radius = center.Distance(points[0]);
        }

        public static Point CalculateCenter(List<Point> points){
            double x = points.Sum(p => p.x) / 3;
            double y = points.Sum(p => p.y) / 3;
            return new Point(x, y);
            
        }

        public bool InCircumsphere(Point p){
            return p.Distance(center) <= radius;
        }
    }
}
