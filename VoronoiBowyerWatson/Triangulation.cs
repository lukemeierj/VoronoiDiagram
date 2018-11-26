using System;
using System.Linq;
using System.Collections.Generic;

namespace VoronoiBowyerWatson
{
    public class Triangulation
    {
        // For image generation later on:
        public int height;
        public int width;
        public int xOffset;
        public int yOffset;
        public double padding = 10;

        private List<Point> superTriangle = new List<Point>();
        public List<Vertex> triangles = new List<Vertex>();
        public List<Point> addedPoints { private set; get; } = new List<Point>(); 
        private List<Point> allPoints;

        public Triangulation(List<Point> points)
        {
            allPoints = points;
            allPoints.Sort(); // various places says this improves efficiency 
            AddSuperTriangle(points);
        }

        public Triangulation(List<Vertex> triangles, List<Point> addedPoints, List<Point> allPoints){
            this.triangles = triangles;
            this.addedPoints = addedPoints;
            this.allPoints = allPoints;
        }

        //Creates a triangle that covers the axis aligned bounding box
        private void AddSuperTriangle(List<Point> points){

            double maxY = points.Max(point => point.y);
            double minY = points.Min(point => point.y);
            double maxX = points.Max(point => point.x);
            double minX = points.Min(point => point.x);

            height = (int)(maxY + Math.Abs(minY) + 2 * Math.Abs(padding));
            width = (int)(maxX + Math.Abs(minX) + 2 * Math.Abs(padding);
            xOffset = (int)Math.Abs(minX) - (int)padding;
            yOffset = (int)Math.Abs(minY) - (int)padding;

            //Points to define a triangle that surrounds the axis aligned bounding box
            Point upperLeft = new Point(minX - padding, maxY + padding);
            Point lowerLeft = new Point(minX - padding, minY - (maxY - minY) - padding);
            Point upperRight = new Point(maxX + (maxX - minX) + padding, maxY + padding);

            List<Point> boundaries = new List<Point> { upperLeft, upperRight, lowerLeft };
            List<Vertex> neighbors = new List<Vertex> { Vertex.nullVertex, Vertex.nullVertex, Vertex.nullVertex };
            triangles.Add(new Vertex(boundaries, neighbors));

            // Add supertriangle to triangulation:
            superTriangle = boundaries;
        }

        private void AddPoint(Point p){
            addedPoints.Add(p);
            // Gets all the triangles whose circumspheres contain the added point: 
            HashSet<Vertex> badTriangles = FindBadTriangles(p);

            // Remove the bad triangles from the total triangles:
            triangles = new List<Vertex>(triangles.Except(badTriangles));

            // Calculate the boundary edges of those triangles:
            List<Edge> boundary = Boundary(badTriangles);

            // Get new triangulation between the boundary point and the added point:
            List<Vertex> newTriangles = GetNewTriangulation(p, boundary);

            // Save new triangles:
            triangles.AddRange(newTriangles);
        }

        // Given a point, returns all of the triangles of this.triangles
        // which contain the point in their circumsphere
        private HashSet<Vertex> FindBadTriangles(Point p){
            HashSet<Vertex> invalidVertices = new HashSet<Vertex>();
            foreach (Vertex triangle in triangles)
            {
                if (triangle.InCircumsphere(p))
                {
                    invalidVertices.Add(triangle);
                }
            }
             return invalidVertices;
        }

        // Given a list of several triangles, calculate the outer
        // border of those triangles for retriangulation.
        private List<Edge> Boundary(HashSet<Vertex> vertices){
            List<Edge> boundary = new List<Edge>();
            Dictionary<Point, Edge> edges = new Dictionary<Point, Edge>();
            
            foreach(Vertex invalidVertex in vertices){
                for (int j = 0; j < 3; j++){
                    Edge e = invalidVertex.GetEdge(j);
                    // If edge is NOT shared with any other
                    // triangle, then it is an outer border.
                    if (!vertices.Contains(e.opposite) && !edges.ContainsKey(e.a))
                    {
                        edges.Add(e.a, e);
                    }
                }
            }
            Edge next = edges[edges.Keys.First()];
            int i = 0;
            boundary.Add(next);
            while(!boundary[0].a.Equals(boundary[boundary.Count -1].b)){
                if(edges.ContainsKey(boundary[i].b)){
                    next = edges[boundary[i].b];
                    i++;
                    boundary.Add(next);
                }
                else {
                    throw new ArgumentException("No cycle of an opening.");
                }
            }
            return boundary;
        }

        // Given a point p and a list of boundary edges around it,
        // triangulate from edge points to point itself
        private List<Vertex> GetNewTriangulation(Point p, List<Edge> boundary)
        {
            // First, draw triangles between edge points and the new point:
            List<Vertex> newVertices = new List<Vertex>();
            foreach(Edge e in boundary){
                List<Point> points = new List<Point> { p, e.a, e.b };
                List<Vertex> neighbors = new List<Vertex> { Vertex.nullVertex, e.opposite, Vertex.nullVertex };
                Vertex newTriangle = new Vertex(points, neighbors);
                newVertices.Add(newTriangle);
                // Change neighbor's value on opposite side of the edge to point here:
                if (e.opposite != null) {
                    e.opposite.UpdateValueOfNeighborWithEdge(e, newTriangle);
                } 
            }

            // Only works if the triangles are in a certain order: (??)
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
