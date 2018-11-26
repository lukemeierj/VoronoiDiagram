using System;
using System.Collections.Generic;

namespace VoronoiAlgorithms.Models
{
    public class Edge : IEquatable<Edge>
    {
        public Point a { get; set; }
        public Point b { get; set; }
        public List<Vertex> adjacentVertices { private set; get; }

        public Vertex opposite
        {
            get
            {
                return adjacentVertices.Count > 0 ? adjacentVertices[0] : null;
            }
        }

        public Edge(Point a, Point b, List<Vertex> vertices)
        {
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

        public Edge(Point a, Point b)
        {
            this.a = a;
            this.b = b;
            adjacentVertices = new List<Vertex>();
        }

        public string EdgeString
        {
            get
            {
                return a.ToString() + " -- " + b.ToString();
            }
        }

        public bool Equals(Edge other)
        {
            if (a.Equals(other.a) && b.Equals(other.b))
            {
                return true;
            }
            else if (a.Equals(other.b) && b.Equals(other.a))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            // Ensures that a -> b and b -> a
            // are considered duplicates by HashSet
            return a.GetHashCode() + b.GetHashCode();
        }
    }
}
