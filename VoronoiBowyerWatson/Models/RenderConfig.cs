using System;
namespace VoronoiAlgorithms.Models
{


    public class RenderConfig
    {
        public int height = 0;
        public int width = 0;
        public int xPadding = 0;
        public int yPadding = 0;
        public int xOffset = 0;
        public int yOffset = 0;

        public RenderConfig(int width, int height, int xPadding, int yPadding, int xOffset = 0, int yOffset = 0)
        {
            this.height = height;
            this.width = width;
            this.xPadding = xPadding;
            this.yPadding = yPadding;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
        }
    }
}
