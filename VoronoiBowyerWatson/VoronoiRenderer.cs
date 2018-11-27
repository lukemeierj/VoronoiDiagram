using System;
using System.Drawing;
using VoronoiAlgorithms.Models;

namespace VoronoiAlgorithms
{
    public static class VoronoiRenderer
    {
        public static System.Drawing.Point TransposePoint(VoronoiAlgorithms.Models.Point p, RenderConfig config){
            return new System.Drawing.Point((int)(p.x + config.xPadding + config.xOffset), (int)(p.y + config.yPadding + config.yOffset));
        }

        public static void DrawPoint(this Graphics g, VoronoiAlgorithms.Models.Point center, Brush pen, RenderConfig config, int radius = 4){
            int xCoord = (int)Math.Floor(center.x - radius + config.xPadding + config.xOffset);
            int yCoord = (int)Math.Floor(center.y - radius + config.yPadding + config.yOffset);
            g.FillEllipse(pen, xCoord, yCoord, radius * 2, radius * 2);
        }


        public static void DrawCircle(this Graphics g, Models.Point center, Pen pen, double radius, RenderConfig config)
        {
            int minX = (int)(Math.Floor(center.x) - radius + config.xPadding + config.xOffset);
            int minY = (int)(Math.Floor(center.y) - radius + config.yPadding + config.yOffset);
            Rectangle rect = new Rectangle(minX, minY, (int)radius * 2, (int)radius * 2);
            g.DrawEllipse(pen, rect);

        }

        public static Bitmap CreateBitmap(RenderConfig config){
            return new Bitmap(config.width + 2 * config.xPadding, config.height + 2 * config.yPadding);
        }

        public static void DrawDiagram(VoronoiDiagram v, RenderConfig config, string filename)
        {
            // Initialize surface:
            Bitmap image = CreateBitmap(config);
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
            Bitmap image = CreateBitmap(config);
             
            for (int row = 0; row < image.Height; row++)
            {
                for (int col = 0; col < image.Width; col++)
                {
                    bool initialPadding = col < config.xPadding || row < config.yPadding;
                    bool finalPadding = (col - config.xPadding) >= diagram.output.GetLength(0) || (row - config.yPadding) >= diagram.output.GetLength(1);
                   
                    if (initialPadding || finalPadding){
                        image.SetPixel(col, row, Color.White);
                    }
                    else {
                        ushort pointIndex = diagram.output[col - config.xPadding, row - config.yPadding];
                        image.SetPixel(col, row, diagram.sites[pointIndex].color);
                    }

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
            Bitmap image = CreateBitmap(config);
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
