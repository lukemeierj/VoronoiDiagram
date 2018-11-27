using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiAlgorithms.Models;

namespace VoronoiAlgorithms
{
    public class BruteForceVoronoi
    {
        //These are the points in space
        public List<Point> sites;
        private int width;
        private int height;
        private int padding = 20;
        private int xOffset;
        private int yOffset;
        public ushort[,] output;

        // Initialize the calculator with sites. We find the width & height from the sites:
        public BruteForceVoronoi(List<Point> sites)
        {
            this.sites = sites;
            int maxY = (int)sites.Max(point => point.y) + padding;
            int minY = (int)sites.Min(point => point.y);
            int maxX = (int)sites.Max(point => point.x) + padding;
            int minX = (int)sites.Min(point => point.x);
            minX = Math.Max(minX - padding, 0);
            minY = Math.Max(minY - padding, 0);

            this.yOffset = minY;
            this.xOffset = minX;
            this.width = maxX - minX;
            this.height = maxY - minY;
        }

        // Some information was gathered from this StackOverflow answer:
        // https://stackoverflow.com/a/85484
        public void GenerateVoronoi()
        {
            output = new ushort[width, height];

            // row goes through each row from top to bottom
            for (int row = 0; row < height; row++)
            {
                // col goes through each col from right to left
                for (int col = 0; col < width; col++)
                {
                    // col = x, since col tracks rtl
                    // row = y, since row tracks up/down
                    Point curPoint = new Point(col + xOffset, row + yOffset);

                    // Arbitrarily start at the first site:
                    double closestDistance = curPoint.Distance(sites[0]);
                    ushort closestIndex = 0;

                    // Start after the first site:
                    for (ushort i = 1; i < sites.Count; i++)
                    {
                        Point site = sites[i];
                        double distance = curPoint.Distance(site);
                        // Better than the previous point:
                        if (distance < closestDistance)
                        {
                            closestIndex = i;
                            closestDistance = distance;
                        }
                    }
                    output[col, row] = closestIndex;
                }
            }
        }

        public RenderConfig FullFrameConfig
        {
            get
            {
                return new RenderConfig(width, height, 0, 0, -xOffset, -yOffset);
            }
        }

    }
}
