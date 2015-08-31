using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace RemoteControl.Drawing
{
    public class Screenshot
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(out CURSORINFO pci);
        private const Int32 CURSOR_SHOWING = 0x00000001;
        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            public Int32 x;
            public Int32 y;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct CURSORINFO
        {
            public Int32 cbSize;
            public Int32 flags;
            public IntPtr hCursor;
            public POINT ptScreenPos;
        }
        public Image GetFullScreenImage(VideoParameters v)
        {
            try
            {
                System.Drawing.Image MyImage = new Bitmap((int)SystemParameters.VirtualScreenWidth, (int)SystemParameters.VirtualScreenHeight);
                CURSORINFO pci;
                pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
                GetCursorInfo(out pci);
                Graphics g = Graphics.FromImage(MyImage);
                g.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), new System.Drawing.Size((int)SystemParameters.VirtualScreenWidth, (int)SystemParameters.VirtualScreenHeight));

                System.Windows.Forms.Cursor cur = new System.Windows.Forms.Cursor(pci.hCursor);
                cur.Draw(g, new System.Drawing.Rectangle(pci.ptScreenPos.x - 10, pci.ptScreenPos.y - 10, cur.Size.Width, cur.Size.Height));


                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                MyImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                Image img = MyImage.GetThumbnailImage(v.Height, v.Width, myCallback, IntPtr.Zero);
                return img;
            }
            catch
            {
                throw;
            }
        }

        public Image GetRectangleScreenshot(VideoParameters v)
        {
            try
            {
                int videoWidth = (int)SystemParameters.VirtualScreenWidth;
                int videoHeight = (int)SystemParameters.VirtualScreenHeight;
                System.Drawing.Image MyImage = new Bitmap(v.Width,v.Height );
                CURSORINFO pci;
                pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
                GetCursorInfo(out pci);
                Graphics g = Graphics.FromImage(MyImage);
                int px = 0;
                int py = 0;
                int wx= videoWidth - pci.ptScreenPos.x;
                int wy = videoHeight - pci.ptScreenPos.y;
                int mouseX=0;
                int mouseY=0;
                if(pci.ptScreenPos.x>v.Width && wx<v.Width)
                {
                    px = videoWidth - v.Width;
                    mouseX=pci.ptScreenPos.x-(videoWidth - v.Width);
                }
                else if(pci.ptScreenPos.x<v.Width && wx>v.Width)
                {
                    px = 0;
                    mouseX=pci.ptScreenPos.x;
                }
                else if(pci.ptScreenPos.x-(v.Width/2)>0 && pci.ptScreenPos.x+(v.Width/2)<videoWidth)
                {
                    px = pci.ptScreenPos.x - (v.Width / 2);
                    mouseX=v.Width/2;
                }

                if(pci.ptScreenPos.y>v.Height && wy<v.Height)
                {
                    py = videoHeight - v.Height;
                    mouseY=pci.ptScreenPos.y-(videoHeight-v.Height);
                }
                else if (pci.ptScreenPos.y < v.Height && wy > v.Height)
                {
                    py = 0;
                    mouseY=pci.ptScreenPos.y;
                }
                else if(pci.ptScreenPos.y-(v.Height/2)>0 && pci.ptScreenPos.y+(v.Height/2)<videoHeight)
                {
                    py = pci.ptScreenPos.y - (v.Height / 2);
                    mouseY=v.Height/2;
                }
                  
                g.CopyFromScreen(new System.Drawing.Point(px, py), new System.Drawing.Point(0, 0), new System.Drawing.Size(v.Width, v.Height));

                System.Windows.Forms.Cursor cur = new System.Windows.Forms.Cursor(pci.hCursor);
                cur.Draw(g, new System.Drawing.Rectangle(mouseX,mouseY, cur.Size.Width, cur.Size.Height));


                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                MyImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                Image img = MyImage.GetThumbnailImage(v.Height, v.Width, myCallback, IntPtr.Zero);
                //img.Save(@"D:\1.jpg");
                return img;
            }
            catch
            {
                throw;
            }
        }

        public static bool ThumbnailCallback()
        {
            return false;
        }
    }
}
