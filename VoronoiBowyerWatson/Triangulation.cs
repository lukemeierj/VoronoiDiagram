using System;
using System.Linq;
using System.Collections.Generic;

namespace VoronoiBowyerWatson
{
    public class Triangulation
    {
        private Vertex superTriangle;
        public List<Vertex> triangles = new List<Vertex>();
        private List<Point> addedPoints = new List<Point>();
        private List<Point> allPoints;

        public Triangulation(List<Point> points)
        {
            allPoints = points;
            superTriangle = SuperTriangle(points);
            triangles.Add(superTriangle);

        }

        //Creates a triangle that covers the axis aligned bounding box
        private Vertex SuperTriangle(List<Point> points){
            double maxY = points.Max(point => point.y);
            double minY = points.Min(point => point.y);
            double maxX = points.Max(point => point.x);
            double minX = points.Min(point => point.x);

            //Points to define a triangle that surrounds the axis aligned bounding box
            Point upperLeft = new Point(minX, maxY);
            Point lowerLeft = new Point(minX, minY - (maxY - minY));
            Point upperRight = new Point(maxX + (maxX - minX), maxY);

            List<Point> boundaries = new List<Point> { upperLeft, upperRight, lowerLeft };
            List<Vertex> neighbors = new List<Vertex> { Vertex.nullVertex, Vertex.nullVertex, Vertex.nullVertex };

            return new Vertex(boundaries, neighbors);

        }

        private void AddPoint(Point p){
            addedPoints.Add(p);
            HashSet<Vertex> invalidVertices = InvalidAfterPoint(p);

            List<Edge> boundary = Boundary(invalidVertices);

            //remove bad vertices
            triangles.Except(invalidVertices);

            List<Vertex> newTriangles = Retriangulate(p, boundary);
        }

        private HashSet<Vertex> InvalidAfterPoint(Point p){
            Queue<Vertex> SearchQueue = new Queue<Vertex>();
            HashSet<Vertex> invalidVertices = new HashSet<Vertex>();
            foreach (Vertex v in triangles)
            {
                if (v.InCircumsphere(p))
                {
                    SearchQueue.Enqueue(v);
                    invalidVertices.Add(v);
                }
            }
            while(SearchQueue.Count > 0){
                Vertex node = SearchQueue.Dequeue();
                // TODO: Check if its already destined to be deleted?
                foreach(Vertex neighbor in node.neighbors){
                    if (neighbor == Vertex.nullVertex){
                        continue;
                    }
                    else if (neighbor.InCircumsphere(p) && invalidVertices.Contains(neighbor))
                    {
                        invalidVertices.Add(neighbor);
                        SearchQueue.Enqueue(neighbor);
                    }
                }
            }

            return invalidVertices;

        }

        private List<Edge> Boundary(HashSet<Vertex> vertices){

            HashSet<Edge> boundary = new HashSet<Edge>();
            
            foreach(Vertex invalidVertex in vertices){
                for (int i = 0; i < 3; i++){
                    Edge e = invalidVertex.GetEdge(i);
                    if (!vertices.Contains(e.opposite))
                    {
                        boundary.Add(e);
                    }
                }
            }
            return new List<Edge>(boundary);
        }

        private List<Vertex> Retriangulate(Point p, List<Edge> boundary)
        {
            List<Vertex> newVertices = new List<Vertex>();
            foreach(Edge e in boundary){
                List<Point> points = new List<Point> { p, e.a, e.b };
                List<Vertex> neighbors = new List<Vertex> { Vertex.nullVertex, e.opposite, Vertex.nullVertex };
                newVertices.Add(new Vertex(points, neighbors));
            }

            int numTris = newVertices.Count;
            for (int i = 0; i < numTris; i++){
                newVertices[i].neighbors[0] = newVertices[(numTris + i - 1) % numTris];
                newVertices[i].neighbors[2] = newVertices[(i + 1) % numTris];
            }
            return newVertices;
         }

        public List<Vertex> Triangulate(){
            foreach(Point p in allPoints){
                AddPoint(p);
            }
            return triangles;
        }


    }
}
