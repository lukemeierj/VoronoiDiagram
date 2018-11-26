using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

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

    class Program
    {
        static int numRuns = 10;

        static void Main(string[] args)
        {
            long totalTime = 0;
            for (int i = 0; i < numRuns; i++) {
                totalTime += RunTests(100, 2000);
            }
            long avg = totalTime / numRuns;
            Console.WriteLine("Average completion time: " + avg);
        }

        // Returns the number of milliseconds it took to generate the diagram:
        public static long RunTests (int numPoints, int max) {
            Stopwatch timer = new Stopwatch();
            Random rand = new Random();

            List<Point> sites = new List<Point>();
            for (int i = 0; i < numPoints; i++) {
                int a = rand.Next(max);
                int b = rand.Next(max);
                Color randomColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                sites.Add(new Point(a, b, randomColor));
            }

            timer.Start();

            BruteForceCalculation generator = new BruteForceCalculation(sites);
            Color[,] result = generator.CalculateVoronoiDiagram();
            timer.Stop();

            //SaveAsImage(result, sites, generator.width, generator.height);

            long elapsed = timer.ElapsedMilliseconds;
            Console.WriteLine("Results for " + numPoints + " points over the range (0, " + max + "): " + elapsed);
            return elapsed;
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
            image.Save("img_brute_force.bmp");
            whiteBrush.Dispose();
        }
    }
}
