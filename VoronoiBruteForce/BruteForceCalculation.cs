using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace VoronoiBruteForce
{
    public class BruteForceCalculation
    {
        readonly List<Point> sites;
        readonly int width;
        readonly int height;

        // Note: we will go through the width/height from 0 up to (excluding) the value for width/height
        public BruteForceCalculation(List<Point> sites, int width, int height)
        {
            this.sites = sites;
            this.width = width;
            this.height = height;
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
