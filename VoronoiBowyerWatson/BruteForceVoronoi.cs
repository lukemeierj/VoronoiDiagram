using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiAlgorithms.Models;

namespace VoronoiAlgorithms
{
    public class BruteForceVoronoi
    {
        public List<Point> sites;
        private int width;
        private int height;
        public System.Drawing.Color[,] output;

        // Initialize the calculator with sites. We find the width & height from the sites:
        public BruteForceVoronoi(List<Point> sites)
        {
            this.sites = sites;
            int maxY = (int)sites.Max(point => point.y);
            int minY = (int)sites.Min(point => point.y);
            int maxX = (int)sites.Max(point => point.x);
            int minX = (int)sites.Min(point => point.x);

            this.width = maxX - minX;
            this.height = maxY - minY;
        }

        // Some information was gathered from this StackOverflow answer:
        // https://stackoverflow.com/a/85484
        public void GenerateVoronoi() {
            output = new System.Drawing.Color[width, height];

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
        }

        public RenderConfig FullFrameConfig {
            get {
                return new RenderConfig(width, height, 0, 0);
            }
        }

        public static double DistanceBetweenPoints (Point p1, Point p2) {
            double xVal = Math.Pow((p2.x - p1.x), 2);
            double yVal = Math.Pow((p2.y - p1.y), 2);
            return Math.Sqrt(xVal + yVal);
        }
    }
}
