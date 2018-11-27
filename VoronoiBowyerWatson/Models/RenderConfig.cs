using System;
namespace VoronoiAlgorithms.Models
{


    public class RenderConfig
    {
        public int height = 0;
        public int width = 0;
        public int xPadding = 0;
        public int yPadding = 0;

        public RenderConfig(int width, int height, int xPadding, int yPadding)
        {
            this.height = height;
            this.width = width;
            this.xPadding = xPadding;
            this.yPadding = yPadding;
        }
    }
}
