using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace VoronoiBruteForce
{
    public struct Point
    {
        public double x, y;
        public Color color;
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public Point(double x, double y, Color c) {
            this.x = x;
            this.y = y;
            color = c;
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
            Point A = new Point(5, 5, Color.Aqua);
            Point B = new Point(10, 10, Color.Beige);
            Point C = new Point(60, 70, Color.SeaGreen);
            int width = 100;
            int height = 100;
            List<Point> testSitesA = new List<Point>(){ A, B, C };

            BruteForceCalculation algoTest1 = new BruteForceCalculation(testSitesA, width, height);
            Color[,] res1 = algoTest1.CalculateVoronoiDiagram();
            SaveAsImage(res1, width, height);
        }

        public static void SaveAsImage (Color[,] voronoi, int width, int height) {
            Bitmap image = new Bitmap(width, height, PixelFormat.Canonical);

            for (int row = 0; row < width; row++) {
                for (int col = 0; col < height; col++) {
                    image.SetPixel(col, row, voronoi[col, row]);
                }
            }
            image.Save("img.bmp");
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
