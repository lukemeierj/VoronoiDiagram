using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace VoronoiBowyerWatson
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(5, 5));
            points.Add(new Point(400, 400));
            points.Add(new Point(20, 15));
            points.Add(new Point(100, 350));
            points.Add(new Point(25, 350));
            points.Add(new Point(300, 10));

            double maxY = points.Max(point => point.y);
            double minY = points.Min(point => point.y);
            double maxX = points.Max(point => point.x);
            double minX = points.Min(point => point.x);

            int height = (int)(maxY + Math.Abs(minY));
            int width = (int)(maxX + Math.Abs(minX));
            int xOffset = (int) Math.Abs(minX);
            int yOffset = (int) Math.Abs(minY);

            Triangulation tri = new Triangulation(points);
            tri.Triangulate();
            DrawDiagramFromTriangulation(tri, width, height, xOffset, yOffset);
        }

        static void DrawDiagramFromTriangulation (Triangulation tri, int width, int height, int xOffset, int yOffset) {
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
            image.Save("img_efficient.bmp");

            linePen.Dispose();
            pointBrush.Dispose();
        }
    }
}
