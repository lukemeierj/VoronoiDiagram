using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace VoronoiBowyerWatson
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<Point> points = new List<Point>();
            //points.Add(new Point(5, 100));
            //points.Add(new Point(400, 400));
            //points.Add(new Point(100, 15));
            //points.Add(new Point(100, 350));
            //points.Add(new Point(25, 350));
            //points.Add(new Point(300, 10));

            //double maxY = points.Max(point => point.y);
            //double minY = points.Min(point => point.y);
            //double maxX = points.Max(point => point.x);
            //double minX = points.Min(point => point.x);

            //int height = (int)(maxY + Math.Abs(minY));
            //int width = (int)(maxX + Math.Abs(minX));
            //int xOffset = (int) Math.Abs(minX);
            //int yOffset = (int) Math.Abs(minY);

            //Triangulation tri = new Triangulation(points);
            //tri.Triangulate();
            //DrawDiagramFromTriangulation(tri, width, height, xOffset, yOffset, "efficient_img");

            RunTests(100, 2000);
        }

        // Returns the number of milliseconds it took to generate the diagram:
        public static long RunTests(int numPoints, int max)
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
            Console.WriteLine("Results for " + numPoints + " points over the range (0, " + max + "): " + elapsed);
            return elapsed;
        }

        public static void DrawDiagramFromTriangulation (Triangulation tri, int width, int height, int xOffset, int yOffset, int padding, string filename) {
            xOffset -= padding;
            yOffset -= padding;
            height += 2 * Math.Abs(padding);
            width += 2 * Math.Abs(padding);

            // Initialize surface:
            Bitmap image = new Bitmap(width, height);
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


            foreach (Vertex vertex in tri.triangles) {
                foreach (Vertex neighbor in vertex.neighbors) {
                    if (neighbor != null)
                    {
                        Point a = vertex.center;
                        Point b = neighbor.center;

                        System.Drawing.Point aWithOffset = new System.Drawing.Point((int)Math.Floor(a.x) - xOffset, (int)Math.Floor(a.y) - yOffset);
                        System.Drawing.Point bWithOffset = new System.Drawing.Point((int)Math.Floor(b.x) - xOffset, (int)Math.Floor(b.y) - yOffset);

                        g.DrawLine(linePen, aWithOffset, bWithOffset);
                    }
                }
                foreach (Point point in vertex.points) {
                    int xCoord = (int)Math.Floor(point.x) - pointRadius - xOffset;
                    int yCoord = (int)Math.Floor(point.y) - pointRadius - yOffset;
                    g.FillEllipse(pointBrush, xCoord, yCoord, pointRadius * 2, pointRadius * 2);
                }
            }
            image.Save(filename);

            linePen.Dispose();
            pointBrush.Dispose();
        }

        public static void DrawTriangulation(Triangulation tri, int width, int height, int xOffset, int yOffset, int padding, string filename)
        {
            xOffset -= padding;
            yOffset -= padding;
            height += 2 * Math.Abs(padding);
            width += 2 * Math.Abs(padding);

            // Initialize surface:
            Bitmap image = new Bitmap(width, height);
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
                    System.Drawing.Point pWithOffset = new System.Drawing.Point((int)Math.Floor(p.x) - xOffset, (int)Math.Floor(p.y) - yOffset);
                    g.DrawString(p.ToString(), f, centerBrush, pWithOffset);
                    drawPoints[i] = pWithOffset;
                }
                drawPoints[3] = drawPoints[0];


                g.DrawLines(linePen, drawPoints);


                Point point = vertex.center;
                int xCoord = (int)Math.Floor(point.x) - pointRadius - xOffset;
                int yCoord = (int)Math.Floor(point.y) - pointRadius - yOffset;
                g.FillEllipse(centerBrush, xCoord, yCoord, pointRadius * 2, pointRadius * 2);



                DrawCircle(circlePen, point, vertex.radius, g, xOffset, yOffset);
            }

            foreach(Point point in tri.addedPoints){
                int xCoord = (int)Math.Floor(point.x) - pointRadius - xOffset;
                int yCoord = (int)Math.Floor(point.y) - pointRadius - yOffset;
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
