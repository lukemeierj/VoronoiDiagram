using System;
using System.Collections.Generic;

namespace VoronoiBruteForce
{
    public class BruteForceCalculation
    {
        readonly List<Point> sites;
        readonly Double width;
        readonly Double height;

        public BruteForceCalculation(List<Point> sites, Double width, Double height)
        {
            this.sites = sites;
            this.width = width;
            this.height = height;
        }

        public List<Cell> CalculateVoronoiDiagram() {
            return null;
        }
    }
}
