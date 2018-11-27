using System;
using System.Drawing;
using VoronoiAlgorithms.Models;

namespace VoronoiAlgorithms
{
    public static class VoronoiRenderer
    {
        public static void DrawDiagram(VoronoiDiagram v, string filename)
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
            foreach (Edge e in v.edges)
            {
                System.Drawing.Point aWithOffset = new System.Drawing.Point((int)Math.Floor(e.a.x) - v.xOffset, (int)Math.Floor(e.a.y) - v.yOffset);
                System.Drawing.Point bWithOffset = new System.Drawing.Point((int)Math.Floor(e.b.x) - v.xOffset, (int)Math.Floor(e.b.y) - v.yOffset);
                g.DrawLine(linePen, aWithOffset, bWithOffset);
            }
            // Draw site centers:
            foreach (Models.Point point in v.sites)
            {
                int xCoord = (int)Math.Floor(point.x) - pointRadius - v.xOffset;
                int yCoord = (int)Math.Floor(point.y) - pointRadius - v.yOffset;
                g.FillEllipse(pointBrush, xCoord, yCoord, pointRadius * 2, pointRadius * 2);
            }
            image.Save(filename);

            linePen.Dispose();
            pointBrush.Dispose();
        }

        public static void DrawDiagram (BruteForceVoronoi diagram, string filename) {
            Bitmap image = new Bitmap(diagram.width, diagram.height);

            for (int row = 0; row < diagram.height; row++)
            {
                for (int col = 0; col < diagram.width; col++)
                {
                    image.SetPixel(col, row, diagram.output[col, row]);
                }
            }

            // Note: graphics draws on top of the existing bitmap by reference
            Graphics g = Graphics.FromImage(image);
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            int circleRadius = 4;
            foreach (Models.Point site in diagram.sites)
            {
                g.FillEllipse(whiteBrush, (int)site.x - circleRadius, (int)site.y - circleRadius, circleRadius * 2, circleRadius * 2);
            }
            image.Save(filename);
            whiteBrush.Dispose();

        }

        public static void DrawTriangulation(DelaunayTriangulator tri, string filename)
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
                    Models.Point p = vertex.points[i];
                    System.Drawing.Point pWithOffset = new System.Drawing.Point((int)Math.Floor(p.x) - tri.xOffset, (int)Math.Floor(p.y) - tri.yOffset);
                    g.DrawString(p.ToString(), f, centerBrush, pWithOffset);
                    drawPoints[i] = pWithOffset;
                }
                drawPoints[3] = drawPoints[0];


                g.DrawLines(linePen, drawPoints);


                Models.Point point = vertex.center;
                int xCoord = (int)Math.Floor(point.x) - pointRadius - tri.xOffset;
                int yCoord = (int)Math.Floor(point.y) - pointRadius - tri.yOffset;
                g.FillEllipse(centerBrush, xCoord, yCoord, pointRadius * 2, pointRadius * 2);



                DrawCircle(circlePen, point, vertex.radius, g, tri.xOffset, tri.yOffset);
            }

            foreach (Models.Point point in tri.allPoints)
            {
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

        private static void DrawCircle(Pen pen, Models.Point center, double radius, Graphics g, double xOffset, double yOffset)
        {
            int minX = (int)(Math.Floor(center.x) - radius - xOffset);
            int minY = (int)(Math.Floor(center.y) - radius - yOffset);
            Rectangle rect = new Rectangle(minX, minY, (int)radius * 2, (int)radius * 2);
            g.DrawEllipse(pen, rect);

        }
    }
}
