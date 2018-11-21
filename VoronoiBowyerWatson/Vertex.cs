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
            return Distance(this, a);
        }
    }

    public struct Edge
    {
        public Point a;
        public Point b;
        public List<Vertex> adjacentVertices { private set; get; }
        public Vertex opposite {
            get {
                return adjacentVertices.Count > 0 ? adjacentVertices[0] : null;
            }
        }
        public Edge(Point a, Point b, List<Vertex> vertices){
            this.a = a;
            this.b = b;
            this.adjacentVertices = vertices;
        }
        public Edge(Point a, Point b, Vertex v)
        {
            this.a = a;
            this.b = b;

            adjacentVertices = new List<Vertex> { v };
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

        public static List<Point> SharedPoints(Vertex a, Vertex b){
            return new List<Point>(a.points.Intersect(b.points));
        }

        public List<Point> SharedPoints(Vertex a){
            return SharedPoints(this, a);
        }
        public Edge GetEdge(int i){
            Console.WriteLine(i.ToString());
            Console.WriteLine(points.Count);
            Console.WriteLine(neighbors.Count);

            Point a = points[i % 3];
            Point b = points[(i+1) % 3];
            Vertex op = neighbors[i % 3];

            return new Edge(a, b, op);
        }
    }
}
