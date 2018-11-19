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
            List<Vertex> toDelete = RemoveForPoint(p);
        }

        private List<Vertex> RemoveForPoint(Point p){
            Queue<Vertex> SearchQueue = new Queue<Vertex>();
            HashSet<Vertex> toDelete = new HashSet<Vertex>();
            foreach (Vertex v in triangles)
            {
                if (v.InCircumsphere(p))
                {
                    SearchQueue.Enqueue(v);
                    toDelete.Add(v);
                }
            }
            while(SearchQueue.Count > 0){
                Vertex node = SearchQueue.Dequeue();
                // TODO: Check if its already destined to be deleted?
                foreach(Vertex neighbor in node.neighbors){
                    if (neighbor == Vertex.nullVertex){
                        continue;
                    }
                    else if (neighbor.InCircumsphere(p) && toDelete.Contains(neighbor))
                    {
                        toDelete.Add(neighbor);
                        SearchQueue.Enqueue(neighbor);
                    }
                }
            }

            return new List<Vertex>(toDelete);

        }

        public List<Vertex> Triangulate(){
            foreach(Point p in allPoints){
                AddPoint(p);
            }
            return triangles;
        }


    }
}
