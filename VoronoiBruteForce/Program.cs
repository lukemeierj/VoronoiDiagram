using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace VoronoiBruteForce
{
    public struct Point
    {
        public int x, y;
        public Color color;
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Point(int x, int y, Color c) {
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
            Point A = new Point(5, 5, Color.Pink);
            Point B = new Point(5, 200, Color.IndianRed);
            Point C = new Point(400, 400, Color.DeepSkyBlue);
            Point D = new Point(20, 15, Color.DarkBlue);
            Point E = new Point(100, 350, Color.DarkRed);
            Point F = new Point(25, 350, Color.LightSkyBlue);
            Point G = new Point(300, 10, Color.Blue);

            int width = 500;
            int height = 500;
            List<Point> testSitesA = new List<Point>(){ A, B, C, D, E, F, G };

            BruteForceCalculation algoTest1 = new BruteForceCalculation(testSitesA, width, height);
            Color[,] res1 = algoTest1.CalculateVoronoiDiagram();
            SaveAsImage(res1, testSitesA, width, height);
        }

        public static void SaveAsImage (Color[,] voronoi, List<Point> sites, int width, int height) {
            Bitmap image = new Bitmap(width, height);

            for (int row = 0; row < height; row++) {
                for (int col = 0; col < width; col++) {
                    image.SetPixel(col, row, voronoi[col, row]);
                }
            }

            // Note: graphics draws on top of the existing bitmap by reference
            Graphics g = Graphics.FromImage(image);
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            int circleRadius = 4;
            foreach (Point site in sites)
            {
                g.FillEllipse(whiteBrush, site.x - circleRadius, site.y - circleRadius, circleRadius * 2, circleRadius * 2);
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
