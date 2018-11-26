using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace VoronoiBruteForce
{
    public class BruteForceCalculation
    {
        List<Point> sites;
        public int width { get; set; }
        public int height { get; set; }

        // Initialize the calculator with sites. We find the width & height from the sites:
        public BruteForceCalculation(List<Point> sites)
        {
            this.sites = sites;

            int maxY = sites.Max(point => point.y);
            int minY = sites.Min(point => point.y);
            int maxX = sites.Max(point => point.x);
            int minX = sites.Min(point => point.x);

            this.width = maxX - minX;
            this.height = maxY - minY;
        }

        // Some information was gathered from this StackOverflow answer:
        // https://stackoverflow.com/a/85484
        public Color[,] CalculateVoronoiDiagram() {
            Color[,] output = new Color[width, height];

            // row goes through each row from top to bottom
            for (int row = 0; row < height; row++) {
                // col goes through each col from right to left
                for (int col = 0; col < width; col++) {
                    // col = x, since col tracks rtl
                    // row = y, since row tracks up/down
                    Point curPoint = new Point(col, row);

                    // Arbitrarily start at the first site:
                    Point closestSite = sites[0];
                    double closestDistance = DistanceBetweenPoints(curPoint, closestSite);

                    // Start after the first site:
                    foreach (Point site in sites.Skip(1)) {
                        double distance = DistanceBetweenPoints(curPoint, site);
                        // Better than the previous point:
                        if (distance < closestDistance) {
                            closestSite = site;
                            closestDistance = distance;
                        }
                    }
                    output[col, row] = closestSite.color;
                }
            }
            return output;
        }

        public static double DistanceBetweenPoints (Point p1, Point p2) {
            double xVal = Math.Pow((p2.x - p1.x), 2);
            double yVal = Math.Pow((p2.y - p1.y), 2);
            return Math.Sqrt(xVal + yVal);
        }
    }
}
