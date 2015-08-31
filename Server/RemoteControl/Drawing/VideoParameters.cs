using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Drawing
{
    public class VideoParameters
    {
        public const int MaxWidth = 800;
        public const int MaxHeight = 480;
        public const long MaxQuality = 100;
        private int width=0;
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value > MaxWidth ? MaxWidth : value;
            }
        }
        private int height;
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value > MaxHeight ? MaxHeight : value;
            }
        }
        private long quality = 0;
        public long Quality
        {
            get
            {
                return quality;
            }
            set
            {
                if(value>0&&value<MaxQuality)
                {
                    quality = value;
                }
            }
        }
    }
}
