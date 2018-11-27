using System;
using System.Collections.Generic;

namespace VoronoiAlgorithms.Models
{
    public class VoronoiDiagram
    {
        public HashSet<Point> sites;
        public HashSet<Edge> edges;

        public VoronoiDiagram()
        {
            sites = new HashSet<Point>();
            edges = new HashSet<Edge>();
        }

        public VoronoiDiagram(DelaunayTriangulator triangulation){
            sites = new HashSet<Point>();
            edges = new HashSet<Edge>();

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
