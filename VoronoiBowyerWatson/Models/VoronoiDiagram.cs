using System;
using System.Collections.Generic;

namespace VoronoiAlgorithms.Models
{
    public class VoronoiDiagram
    {
        public HashSet<Point> sites;
        public HashSet<Edge> edges;
        public int height;
        public int width;
        public int xOffset;
        public int yOffset;

        public VoronoiDiagram()
        {
            sites = new HashSet<Point>();
            edges = new HashSet<Edge>();
        }

        public VoronoiDiagram(DelaunayTriangulator triangulation){
            sites = new HashSet<Point>();
            edges = new HashSet<Edge>();

            this.height = triangulation.height;
            this.width = triangulation.width;
            this.xOffset = triangulation.xOffset;
            this.yOffset = triangulation.yOffset;

            foreach (Vertex vertex in triangulation.triangles)
            {
                foreach (Vertex neighbor in vertex.neighbors)
                {
                    if (neighbor != null)
                    {
                        Point a = vertex.center;
                        Point b = neighbor.center;
                        this.edges.Add(new Edge(a, b));
                    }
                }
                this.sites.UnionWith(vertex.points);
            }
        }
    } 

}
