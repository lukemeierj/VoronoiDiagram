using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using VoronoiAlgorithms.Models;

namespace VoronoiAlgorithms
{
    class Program
    {
        static readonly int numRuns = 10;
        public enum TestMode { BOWYER_WATSON, BRUTE_FORCE_SCALE_PTS, BRUTE_FORCE_SCALE_RES };

        // For diagram BMP generation:
        static readonly int numPointsForPic = 20;
        static readonly int rangeForPic = 1000;

        // For normal point scaling:
        static readonly int rangeMultiplier = 5;
        static readonly int[] numPts = { 10, 100, 1000, 5000 };

        // Variables for brute force testing resolution:
        static readonly int[] ranges = { 100, 500, 1000, 2000, 4000, 10000 };
        static readonly int numPtsForRange = 20;

        // A simple CLI program that lets the user test and generate diagrams:
        static void Main(string[] args)
        {
            while (true) {
                Console.WriteLine("Enter 1 to run tests or 0 to generate a diagram. -1 to exit.");
                int res = -1;
                try {
                    res = Convert.ToInt32(Console.ReadLine());
                } catch {
                    continue;
                }

                switch (res)
                {
                    case 1:
                        RunAllTests();
                        break;
                    case 0:
                        RenderSameDiagram();
                        break;
                    case -1:
                        Environment.Exit(1);
                        break;
                }

                continue;
            }
        }

        // Renders a diagram based off of random points.
        // Uses same points with both methods.
        public static void RenderSameDiagram()
        {
            List<Point> points = Point.GetRandomPoints(numPointsForPic, 0, rangeForPic);

            DelaunayTriangulator tri = new DelaunayTriangulator(points);
            VoronoiDiagram voroEfficient = tri.GenerateVoronoi();


            BruteForceVoronoi voroBrute = new BruteForceVoronoi(points);
            voroBrute.GenerateVoronoi();

            RenderConfig config = voroBrute.FullFrameConfig;

            config.xPadding += 50;
            config.yPadding += 50;

            VoronoiRenderer.DrawDiagram(voroEfficient, config, "bowyer_output.bmp");
            VoronoiRenderer.DrawTriangulation(tri.WithoutSupertriangle(), config, "bowyer_triangulation_no_super.bmp");
            VoronoiRenderer.DrawTriangulation(tri, config, "bowyer_triangulation_super.bmp");

            VoronoiRenderer.DrawDiagram(voroBrute, config, "brute_force_output.bmp");
        }

        // Runs test for each mode:
        static void RunAllTests () {
            RunAllTestsForMode(TestMode.BOWYER_WATSON);
            RunAllTestsForMode(TestMode.BRUTE_FORCE_SCALE_RES);
            RunAllTestsForMode(TestMode.BRUTE_FORCE_SCALE_PTS);
        }

        // Runs the test suite for a given test mode:
        static void RunAllTestsForMode (TestMode t) {
            switch (t)
            {
                case TestMode.BOWYER_WATSON:
                    Console.WriteLine("Test Results for BOWYER/WATSON:\n");
                    break;
                case TestMode.BRUTE_FORCE_SCALE_RES:
                    Console.WriteLine("Test Results for BRUTE FORCE (scaling width/height):\n");
                    break;
                case TestMode.BRUTE_FORCE_SCALE_PTS:
                    Console.WriteLine("Test Results for BRUTE FORCE (scaling num points):\n");
                    break;
            }

            Console.WriteLine("Trial:\t\tPoints:\t\tRange:\t\tTime:\t\t");

            if (t == TestMode.BRUTE_FORCE_SCALE_RES) {
                foreach (int r in ranges) {
                    long totalTime = 0;
                    for (int i = 0; i < numRuns; i++)
                    {
                        totalTime += RunTests(i, numPtsForRange, r, t);
                    }
                    long avg = totalTime / numRuns;
                    Console.WriteLine("Average completion time: " + avg + "\n");

                }
            } else {
                foreach (int numPt in numPts)
                {
                    long totalTime = 0;
                    for (int i = 0; i < numRuns; i++)
                    {
                        // The range of points is the current numbef or points
                        // times the range multipler. e.g. 10 points -> 50 range
                        totalTime += RunTests(i, numPt, numPt * rangeMultiplier, t);
                    }
                    long avg = totalTime / numRuns;
                    Console.WriteLine("Average completion time: " + avg + "\n");
                }

            }
        }

        // Runs a specific Voronoi Generation test and returns the time it took
        // to complete.
        static long RunTests(int trial, int numPoints, int max, TestMode t)
        {
            Stopwatch timer = new Stopwatch();
            List<Point> sites = Point.GetRandomPoints(numPoints, 0, max);

            if (t == TestMode.BOWYER_WATSON)
            {
                timer.Start();
                DelaunayTriangulator generator = new DelaunayTriangulator(sites);
                generator.GenerateVoronoi();
                timer.Stop();
            } else {
                timer.Start();
                BruteForceVoronoi generator = new BruteForceVoronoi(sites);
                generator.GenerateVoronoi();
                timer.Stop();
            }

            long elapsed = timer.ElapsedMilliseconds;
            Console.WriteLine(trial + "\t\t" + numPoints + "\t\t" + max + "\t\t" + elapsed);
            return elapsed;
        }
    }
}
