using System;
using System.Linq;
using System.Collections.Generic;
using VoronoiAlgorithms.Models;

namespace VoronoiAlgorithms
{
    public class DelaunayTriangulator
    {
        private List<Point> superTriangle = new List<Point>();
        public List<Vertex> triangles = new List<Vertex>();
        public List<Point> allPoints;

        public DelaunayTriangulator(List<Point> points)
        {
            allPoints = points;
            allPoints.Sort(); // various places says this improves efficiency 
            AddSuperTriangle(points);
        }

        public DelaunayTriangulator(List<Vertex> triangles, List<Point> allPoints){
            this.triangles = triangles;
            this.allPoints = allPoints;
        }

        //Creates a triangle that covers the axis aligned bounding box
        private void AddSuperTriangle(List<Point> points){

            double maxY = points.Max(point => point.y);
            double minY = points.Min(point => point.y);
            double maxX = points.Max(point => point.x);
            double minX = points.Min(point => point.x);

            int margin = Math.Max((int)(maxY-minY)/2, (int)(maxX-minX)/2);

            //Points to define a triangle that surrounds the axis aligned bounding box
            // The bigger this box, the less likely we triangulate in such a way
            // that leaves us with an incomplete voronoi diagram around the edges
            Point upperLeft = new Point(minX - margin, maxY + margin);
            Point lowerLeft = new Point(minX - margin, minY - (maxY - minY) - margin);
            Point upperRight = new Point(maxX + (maxX - minX) + margin, maxY + margin);
             
            List<Point> boundaries = new List<Point> { upperLeft, upperRight, lowerLeft };
            List<Vertex> neighbors = new List<Vertex> { Vertex.nullVertex, Vertex.nullVertex, Vertex.nullVertex };
            triangles.Add(new Vertex(boundaries, neighbors));

            // Add supertriangle to triangulation:
            superTriangle = boundaries;
        }

        public RenderConfig FullFrameConfig {
            get {

                double maxY = allPoints.Max(point => point.y);
                double minY = allPoints.Min(point => point.y);
                double maxX = allPoints.Max(point => point.x);
                double minX = allPoints.Min(point => point.x);

                int height = (int)(maxY + Math.Abs(minY));
                int width = (int)(maxX + Math.Abs(minX));

                return new RenderConfig(width, height, 0, 0);
            }
        }

        private void AddPoint(Point p){
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
            Queue<Vertex> search = new Queue<Vertex>();
            search.Enqueue(GetEnclosedVertex(p));


            while(search.Count > 0){
                Vertex node = search.Dequeue();
                if(node == Vertex.nullVertex){
                    continue;
                }
                else if(node.InCircumsphere(p) && !invalidVertices.Contains(node))
                {
                    invalidVertices.Add(node);

                    foreach (Vertex neighbor in node.neighbors)
                    {
                        search.Enqueue(neighbor);
                    }
                }
                
            }

            return invalidVertices;
        }

        //http://graphics.zcu.cz/files/106_REP_2010_Soukal_Roman.pdf
        private Vertex GetEnclosedVertex(Point p)
        {
            int index = (new Random()).Next(0, triangles.Count - 1);

            Vertex tri = triangles[index];
            bool found = false;

            while (!found)
            {
                found = true;
                for (int i = 0; i < 3; i++)
                {
                    if (tri.AcrossEdge(i, p) < 0)
                    {
                        tri = tri.neighbors[i];
                        found = false;
                        break;
                    }
                }
            }

            return tri;
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

        public List<Vertex> Triangulate () {
            foreach(Point p in allPoints){
                AddPoint(p);
            }
            return triangles;
        }

        public DelaunayTriangulator WithoutSupertriangle () {
            List<Vertex> vertices = new List<Vertex>(triangles.Where(triangle => !triangle.points.Intersect(superTriangle).Any()));
            return new DelaunayTriangulator(vertices, allPoints);
        }

        public VoronoiDiagram GenerateVoronoi () {
            Triangulate();
            return this.VoronoiDiagram;
        }

        public VoronoiDiagram VoronoiDiagram {
            get {
                return new VoronoiDiagram(WithoutSupertriangle());
            }
        }
    

    }
}
