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
        static int range = 500;
        static int[] numPts = { 10, 100, 200 };

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter 1 to run tests or 0 to generate a diagram. -1 to exit.");
                int res = -1;
                try
                {
                    res = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    continue;
                }
                if (res == 1)
                {
                    Console.WriteLine("Test Results for BRUTE FORCE:\n");
                    Console.WriteLine("Trial:\t\tPoints:\t\tRange:\t\tTime:\t\t");

                    foreach (int numPt in numPts)
                    {
                        long totalTime = 0;
                        for (int i = 0; i < numRuns; i++)
                        {
                            totalTime += RunTests(i, numPt, range);
                        }
                        long avg = totalTime / numRuns;
                        Console.WriteLine("Average completion time: " + avg + "\n");
                    }
                }
                else if (res == 0)
                {
                    List<Point> points = new List<Point>();

                    points.Add(new Point(5, 5));
                    points.Add(new Point(5, 200));
                    points.Add(new Point(400, 400));
                    points.Add(new Point(20, 15));
                    points.Add(new Point(100, 350));
                    points.Add(new Point(25, 350));
                    points.Add(new Point(300, 10));

                    BruteForceCalculation generator = new BruteForceCalculation(points);
                    Color[,] result = generator.CalculateVoronoiDiagram();
                    SaveAsImage(result, points, generator.width, generator.height, "brute_force_output.bmp");
                } else if (res == -1) {
                    Environment.Exit(1);
                } else {
                    continue;
                }
            }
        }

        // Returns the number of milliseconds it took to generate the diagram:
        public static long RunTests (int trial, int numPoints, int max) {
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


            long elapsed = timer.ElapsedMilliseconds;
            Console.WriteLine(trial + "\t\t" + numPoints + "\t\t" + max + "\t\t" + elapsed);
            return elapsed;
        }

        public static void SaveAsImage (Color[,] voronoi, List<Point> sites, int width, int height, string filename) {
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
            image.Save(filename);
            whiteBrush.Dispose();
        }
    }
}
