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
    }

}
