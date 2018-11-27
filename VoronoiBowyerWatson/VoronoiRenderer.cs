using System;
using System.Drawing;
using VoronoiAlgorithms.Models;

namespace VoronoiAlgorithms
{
    public static class VoronoiRenderer
    {
        public static System.Drawing.Point TransposePoint(VoronoiAlgorithms.Models.Point p, RenderConfig config){
            return new System.Drawing.Point((int)(p.x + config.xPadding), (int)(p.y + config.yPadding));
        }

        public static void DrawPoint(this Graphics g, VoronoiAlgorithms.Models.Point center, Brush pen, RenderConfig config, int radius = 4){
            int xCoord = (int)Math.Floor(center.x - radius + config.xPadding);
            int yCoord = (int)Math.Floor(center.y - radius + config.yPadding);
            g.FillEllipse(pen, xCoord, yCoord, radius * 2, radius * 2);
        }


        public static void DrawCircle(this Graphics g, Models.Point center, Pen pen, double radius, RenderConfig config)
        {
            int minX = (int)(Math.Floor(center.x) - radius + config.xPadding);
            int minY = (int)(Math.Floor(center.y) - radius + config.yPadding);
            Rectangle rect = new Rectangle(minX, minY, (int)radius * 2, (int)radius * 2);
            g.DrawEllipse(pen, rect);

        }

        public static void DrawDiagram(VoronoiDiagram v, RenderConfig config, string filename)
        {
            // Initialize surface:
            Bitmap image = new Bitmap(config.width, config.height);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);

            // Style for site centers:
            SolidBrush pointBrush = new SolidBrush(Color.Black);

            // Style for lines:
            Pen linePen = new Pen(Brushes.SlateGray)
            {
                Width = 1.0F
            };


            // Draw edges:
            foreach (Edge e in v.edges)
            {
                System.Drawing.Point a = TransposePoint(e.a, config);
                System.Drawing.Point b = TransposePoint(e.b, config);
                g.DrawLine(linePen, a, b);
            }
            // Draw site centers:
            foreach (Models.Point point in v.sites)
            {
                g.DrawPoint(point, pointBrush, config);
            }
            image.Save(filename);

            linePen.Dispose();
            pointBrush.Dispose();
        }

        public static void DrawDiagram (BruteForceVoronoi diagram, RenderConfig config, string filename) {
            Bitmap image = new Bitmap(config.width, config.height);

            for (int row = 0; row < config.height; row++)
            {
                for (int col = 0; col < config.width; col++)
                {
                    image.SetPixel(col, row, diagram.output[col, row]);
                }
            }

            // Note: graphics draws on top of the existing bitmap by reference
            Graphics g = Graphics.FromImage(image);
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            foreach (Models.Point site in diagram.sites)
            {
                g.DrawPoint(site, whiteBrush, config);
            }
            image.Save(filename);
            whiteBrush.Dispose();

        }

        public static void DrawTriangulation(DelaunayTriangulator tri, RenderConfig config, string filename)
        {
            // Initialize surface:
            Bitmap image = new Bitmap(config.width, config.height);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);

            // Style for site centers:
            SolidBrush pointBrush = new SolidBrush(Color.Black);
            SolidBrush centerBrush = new SolidBrush(Color.Red);

            // Style for lines:
            Pen linePen = new Pen(Brushes.SlateGray)
            {
                Width = 1.0F
            };

            Pen circlePen = new Pen(Brushes.GreenYellow)
            {
                Width = 0.75F
            };

            Font font = new Font("Arial", 10);


            foreach (Vertex vertex in tri.triangles)
            {

                System.Drawing.Point[] drawPoints = new System.Drawing.Point[4];

                for (int i = 0; i < vertex.points.Count; i++)
                {
                    Models.Point p = vertex.points[i];
                    System.Drawing.Point pWithOffset = TransposePoint(p, config);
                    g.DrawString(p.ToString(), font, centerBrush, pWithOffset);
                    drawPoints[i] = pWithOffset;
                }

                drawPoints[3] = drawPoints[0];

                g.DrawLines(linePen, drawPoints);


                Models.Point point = vertex.center;
                g.DrawPoint(point, centerBrush, config);


                g.DrawCircle(point, circlePen, vertex.radius, config);
            }

            foreach (Models.Point point in tri.allPoints)
            {
                g.DrawPoint(point, pointBrush, config);
            }
            image.Save(filename);

            linePen.Dispose();
            pointBrush.Dispose();
            centerBrush.Dispose();
            circlePen.Dispose();
        }

    }
}
