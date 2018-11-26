using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace VoronoiBowyerWatson
{
    class Program
    {
        static int numRuns = 10;
        static int range = 500;
        static int[] numPts = { 10 };

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

                if (res == 1)
                {
                    Console.WriteLine("Test Results for BOWYER/WATSON:\n");
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

                    Triangulation tri = new Triangulation(points);
                    tri.Triangulate();
                    DrawDiagram(tri.GetDiagram(), "bowyer_output.bmp");
                }
                else if (res == -1)
                {
                    Environment.Exit(1);
                } else {
                    continue;
                }
            }
        }

        // Returns the number of milliseconds it took to generate the diagram:
        public static long RunTests(int trial, int numPoints, int max)
        {
            Stopwatch timer = new Stopwatch();
            Random rand = new Random();

            List<Point> sites = new List<Point>();
            for (int i = 0; i < numPoints; i++)
            {
                int a = rand.Next(max);
                int b = rand.Next(max);
                sites.Add(new Point(a, b));
            }

            timer.Start();

            Triangulation generator = new Triangulation(sites);
            generator.Triangulate();

            timer.Stop();
            
            long elapsed = timer.ElapsedMilliseconds;
            Console.WriteLine(trial + "\t\t" + numPoints + "\t\t" + max + "\t\t" + elapsed);
            return elapsed;

        }

        public static void DrawDiagram (VoronoiDiagram v, string filename)
        {
            // Initialize surface:
            Bitmap image = new Bitmap(v.width, v.height);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);

            // Style for site centers:
            SolidBrush pointBrush = new SolidBrush(Color.Black);
            int pointRadius = 4;

            // Style for lines:
            Pen linePen = new Pen(Brushes.SlateGray)
            {
                Width = 1.0F
            };


            // Draw edges:
            foreach (Edge e in v.edges) {
                System.Drawing.Point aWithOffset = new System.Drawing.Point((int)Math.Floor(e.a.x) - v.xOffset, (int)Math.Floor(e.a.y) - v.yOffset);
                System.Drawing.Point bWithOffset = new System.Drawing.Point((int)Math.Floor(e.b.x) - v.xOffset, (int)Math.Floor(e.b.y) - v.yOffset);
                g.DrawLine(linePen, aWithOffset, bWithOffset);
            }
            // Draw site centers:
            foreach (Point point in v.sites) {
                int xCoord = (int)Math.Floor(point.x) - pointRadius - v.xOffset;
                int yCoord = (int)Math.Floor(point.y) - pointRadius - v.yOffset;
                g.FillEllipse(pointBrush, xCoord, yCoord, pointRadius * 2, pointRadius * 2);
            }
            image.Save(filename);

            linePen.Dispose();
            pointBrush.Dispose();
        }

        public static void DrawTriangulation(Triangulation tri, string filename)
        {
            // Initialize surface:
            Bitmap image = new Bitmap(tri.width, tri.height);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
          
            // Style for site centers:
            SolidBrush pointBrush = new SolidBrush(Color.Black);
            SolidBrush centerBrush = new SolidBrush(Color.Red);
            int pointRadius = 4;

            // Style for lines:
            Pen linePen = new Pen(Brushes.SlateGray)
            {
                Width = 1.0F
            };

            Pen circlePen = new Pen(Brushes.GreenYellow)
            {
                Width = 0.75F
            };


            foreach (Vertex vertex in tri.triangles)
            {
                System.Drawing.Point[] drawPoints = new System.Drawing.Point[4];

                for (int i = 0; i < vertex.points.Count; i++)
                {
                    Font f = new Font("Arial", 10);
                    Point p = vertex.points[i];
                    System.Drawing.Point pWithOffset = new System.Drawing.Point((int)Math.Floor(p.x) - tri.xOffset, (int)Math.Floor(p.y) - tri.yOffset);
                    g.DrawString(p.ToString(), f, centerBrush, pWithOffset);
                    drawPoints[i] = pWithOffset;
                }
                drawPoints[3] = drawPoints[0];


                g.DrawLines(linePen, drawPoints);


                Point point = vertex.center;
                int xCoord = (int)Math.Floor(point.x) - pointRadius - tri.xOffset;
                int yCoord = (int)Math.Floor(point.y) - pointRadius - tri.yOffset;
                g.FillEllipse(centerBrush, xCoord, yCoord, pointRadius * 2, pointRadius * 2);



                DrawCircle(circlePen, point, vertex.radius, g, tri.xOffset, tri.yOffset);
            }

            foreach(Point point in tri.allPoints){
                int xCoord = (int)Math.Floor(point.x) - pointRadius - tri.xOffset;
                int yCoord = (int)Math.Floor(point.y) - pointRadius - tri.yOffset;
                g.FillEllipse(pointBrush, xCoord, yCoord, pointRadius * 2, pointRadius * 2);
            }
            image.Save(filename);

            linePen.Dispose();
            pointBrush.Dispose();
            centerBrush.Dispose();
            circlePen.Dispose();
        }
        public static void DrawCircle(Pen pen, Point center, double radius, Graphics g, double xOffset, double yOffset){
            int minX = (int)(Math.Floor(center.x) - radius - xOffset);
            int minY = (int)(Math.Floor(center.y) - radius - yOffset);
            Rectangle rect = new Rectangle(minX, minY, (int)radius*2, (int)radius*2);
            g.DrawEllipse(pen, rect);

        }
    
    }
}
