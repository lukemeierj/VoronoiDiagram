﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace VoronoiAlgorithms.Models
{
    public class Vertex
    {
        public List<Point> points;
        public List<Vertex> neighbors { get; private set; }

        public Point center { get; private set; }
        public double radius { get; private set; }

        // TODO: make more unique sentinal 
        public static Vertex nullVertex = null;

        public Vertex(List<Point> points, List<Vertex> neighbors)
        {
            this.neighbors = neighbors;
            this.points = points;
            this.center = CalculateCenter(points);
            this.radius = center.Distance(points[0]);
        }

        public static Point CalculateCenter(List<Point> points)
        {
            Point a = points[0], b = points[1], c = points[2];

            double det = 2 * (a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y));
            double x = 1 / det * ((Math.Pow(a.x, 2) + Math.Pow(a.y, 2)) * (b.y - c.y)
                                + (Math.Pow(b.x, 2) + Math.Pow(b.y, 2)) * (c.y - a.y)
                                + (Math.Pow(c.x, 2) + Math.Pow(c.y, 2)) * (a.y - b.y));
            double y = 1 / det * ((Math.Pow(a.x, 2) + Math.Pow(a.y, 2)) * (c.x - b.x)
                                + (Math.Pow(b.x, 2) + Math.Pow(b.y, 2)) * (a.x - c.x)
                                + (Math.Pow(c.x, 2) + Math.Pow(c.y, 2)) * (b.x - a.x));


            return new Point(x, y);

        }

        public bool InCircumsphere(Point p)
        {
            return p.Distance(center) <= radius;
        }

        public static List<Point> SharedPoints(Vertex a, Vertex b)
        {
            return new List<Point>(a.points.Intersect(b.points));
        }

        public List<Point> SharedPoints(Vertex a)
        {
            return SharedPoints(this, a);
        }

        public double AcrossEdge(int i, Point p)
        {
            Edge e = GetEdge(i);
            return (p.x - e.a.x) * (e.b.y - e.a.y) - (p.y - e.a.y) * (e.b.x - e.a.x);
        }

        public Edge GetEdge(int i)
        {
            Point a = points[i % 3];
            Point b = points[(i + 1) % 3];
            Vertex op = neighbors[i % 3];

            return new Edge(a, b, op);
        }

        public int FindNeighborIndex(Edge e)
        {
            for (int i = 0; i < neighbors.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (neighbors[i] != null)
                    {
                        Edge curEdge = neighbors[i].GetEdge(j);
                        if (curEdge.Equals(e))
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }

        // Search for a neighbor that has an edge "e". Update its value
        // to the given parameter.
        public void UpdateValueOfNeighborWithEdge(Edge e, Vertex update)
        {
            int index = FindNeighborIndex(e);
            if (index >= 0)
            {
                neighbors[index] = update;
            }
        }
    }
}
