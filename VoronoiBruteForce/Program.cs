using System;
using System.Collections.Generic;

namespace VoronoiBruteForce
{
    public struct Point {
        readonly Double x, y;
        public Point(Double x, Double y) {
            this.x = x;
            this.y = y;
        }
    }

    public struct Edge {
        readonly Point start, end;
        public Edge(Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }
    }

    public struct Cell {
        readonly Point site;
        public List<Edge> boundary;
        public Cell(Point site, List<Edge> boundary) {
            this.site = site;
            this.boundary = boundary;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Point A = new Point(5, 5);
            Point B = new Point(10, 10);
            Point C = new Point(60, 70);
            List<Point> testSitesA = new List<Point>(){ A, B, C };

            BruteForceCalculation algoTest1 = new BruteForceCalculation(testSitesA, 100, 100);
            List<Cell> res1 = algoTest1.CalculateVoronoiDiagram();
            PrintResults(res1);
        }

        public static void PrintResults(List<Cell> cells) {
            if (cells != null) {
                Console.WriteLine("There are " + cells.Count + " cells.");
            } else {
                Console.WriteLine("No results.");
            }
        }
    }
}
