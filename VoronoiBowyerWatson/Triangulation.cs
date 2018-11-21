using System;
using System.Linq;
using System.Collections.Generic;

namespace VoronoiBowyerWatson
{
    public class Triangulation
    {
        private List<Point> superTriangle = new List<Point>();
        public List<Vertex> triangles = new List<Vertex>();
        public List<Point> addedPoints { private set; get; } = new List<Point>(); 
        private List<Point> allPoints;

        public Triangulation(List<Point> points)
        {
            allPoints = points;
            AddSuperTriangle(points);
        }

        public Triangulation(List<Vertex> triangles, List<Point> addedPoints, List<Point> allPoints){
            this.triangles = triangles;
            this.addedPoints = addedPoints;
            this.allPoints = allPoints;
        }

        //Creates a triangle that covers the axis aligned bounding box
        private void AddSuperTriangle(List<Point> points){
            double padding = 100;

            double maxY = points.Max(point => point.y);
            double minY = points.Min(point => point.y);
            double maxX = points.Max(point => point.x);
            double minX = points.Min(point => point.x);

            //Points to define a triangle that surrounds the axis aligned bounding box
            Point upperLeft = new Point(minX - padding, maxY + padding);
            Point lowerLeft = new Point(minX - padding, minY - (maxY - minY) - padding);
            Point upperRight = new Point(maxX + (maxX - minX) + padding, maxY + padding);

            List<Point> boundaries = new List<Point> { upperLeft, upperRight, lowerLeft };
            List<Vertex> neighbors = new List<Vertex> { Vertex.nullVertex, Vertex.nullVertex, Vertex.nullVertex };
            triangles.Add(new Vertex(boundaries, neighbors));

            allPoints.AddRange(boundaries);
            addedPoints.AddRange(boundaries);
            superTriangle = boundaries;
        }

        private void AddPoint(Point p){
            addedPoints.Add(p);
            HashSet<Vertex> invalidVertices = InvalidAfterPoint(p);

            List<Edge> boundary = Boundary(invalidVertices);

            //remove bad vertices
            triangles = new List<Vertex>(triangles.Except(invalidVertices));
            foreach(Edge e in boundary){
                Console.WriteLine(e.EdgeString);
            }
            Console.WriteLine();

            List<Vertex> newTriangles = Retriangulate(p, boundary);

            triangles.AddRange(newTriangles);
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
            //while(SearchQueue.Count > 0){
            //    Vertex node = SearchQueue.Dequeue();
            //    // TODO: Check if its already destined to be deleted?
            //    foreach(Vertex neighbor in node.neighbors){
            //        if (neighbor == Vertex.nullVertex){
            //            continue;
            //        }
            //        else if (neighbor.InCircumsphere(p) && invalidVertices.Contains(neighbor))
            //        {
            //            invalidVertices.Add(neighbor);
            //            SearchQueue.Enqueue(neighbor);
            //        }
            //    }
            //}

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
            int i = 0;
            foreach(Point p in allPoints){
                Triangulation tri = WithoutSupertriangle();
                tri.RenderDelaunay("efficient_" + i.ToString() + ".bmp");
                RenderDelaunay("super_" + i.ToString() + ".bmp");
                AddPoint(p);
                i++;
            }
            return triangles;
        }

        private void RenderVoronoi(string filename)
        {

            double maxY = allPoints.Max(point => point.y);
            double minY = allPoints.Min(point => point.y);
            double maxX = allPoints.Max(point => point.x);
            double minX = allPoints.Min(point => point.x);

            int height = (int)(maxY + Math.Abs(minY));
            int width = (int)(maxX + Math.Abs(minX));
            int xOffset = (int)minX;
            int yOffset = (int)minY;

            Program.DrawDiagramFromTriangulation(this, width, height, xOffset, yOffset, 50, filename);
        }

        private void RenderDelaunay(string filename)
        {

            double maxY = allPoints.Max(point => point.y);
            double minY = allPoints.Min(point => point.y);
            double maxX = allPoints.Max(point => point.x);
            double minX = allPoints.Min(point => point.x);

            int height = (int)(maxY + Math.Abs(minY));
            int width = (int)(maxX + Math.Abs(minX));
            int xOffset = (int)minX;
            int yOffset = (int)minY;

            Program.DrawTriangulation(this, width, height, xOffset, yOffset, 50, filename);
        }

        public Triangulation WithoutSupertriangle(){
            List<Vertex> vertices = new List<Vertex>(triangles.Where(triangle => !triangle.points.Intersect(superTriangle).Any()));
            List<Point> newAllPoints = new List<Point>(allPoints.Except(superTriangle));
            List<Point> newAddedPoints = new List<Point>(addedPoints.Except(superTriangle));
            return new Triangulation(vertices, newAddedPoints, newAllPoints);

        }

    }
}
