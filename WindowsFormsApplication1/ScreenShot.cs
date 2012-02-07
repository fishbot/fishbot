using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    class ScreenShot
    {
        static Icon ic;
        static Bitmap bmp;
        static IntPtr hicon;

        public static Bitmap CaptureImage(Point SourcePoint, Point DestinationPoint, Rectangle SelectionRectangle)
        {
            Bitmap bitmap = new Bitmap(SelectionRectangle.Width, SelectionRectangle.Height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(SourcePoint, DestinationPoint, SelectionRectangle.Size);
            }
            //bitmap.Save(FilePath, ImageFormat.Jpeg);

            return bitmap;
        }

        public static Bitmap CaptureCursor(ref int x, ref int y)
        {
            Win32Stuff.CURSORINFO ci = new Win32Stuff.CURSORINFO();
            Win32Stuff.ICONINFO icInfo;
            ci.cbSize = Marshal.SizeOf(ci);
            if (Win32Stuff.GetCursorInfo(out ci))
            {
                if (ci.flags == Win32Stuff.CURSOR_SHOWING)
                {
                    hicon = Win32Stuff.CopyIcon(ci.hCursor);
                    if (Win32Stuff.GetIconInfo(hicon, out icInfo))
                    {
                        if (icInfo.hbmMask != IntPtr.Zero)
                        {
                            Win32Stuff.DeleteObject(icInfo.hbmMask);
                        }

                        if (icInfo.hbmColor != IntPtr.Zero)
                        {
                            Win32Stuff.DeleteObject(icInfo.hbmColor);
                        }

                        x = ci.ptScreenPos.x - ((int)icInfo.xHotspot);
                        y = ci.ptScreenPos.y - ((int)icInfo.yHotspot);
                    }

                    if (hicon != IntPtr.Zero)
                    {
                        ic = Icon.FromHandle(hicon);
                        bmp = ic.ToBitmap();

                        Win32Stuff.DestroyIcon(ic.Handle);

                        return bmp;
                    }
                }
            }
            return null;
        }

        public static Bitmap CaptureCursor1(ref int x, ref int y)
        {
            Win32Stuff.CURSORINFO cursorInfo = new Win32Stuff.CURSORINFO();
            cursorInfo.cbSize = Marshal.SizeOf(cursorInfo);
            if (!Win32Stuff.GetCursorInfo(out cursorInfo))
                return null;

            if (cursorInfo.flags != Win32Stuff.CURSOR_SHOWING)
                return null;

            IntPtr hicon = Win32Stuff.CopyIcon(cursorInfo.hCursor);
            if (hicon == IntPtr.Zero)
                return null;

            Win32Stuff.ICONINFO iconInfo;
            if (!Win32Stuff.GetIconInfo(hicon, out iconInfo))
                return null;

            x = cursorInfo.ptScreenPos.x - ((int)iconInfo.xHotspot);
            y = cursorInfo.ptScreenPos.y - ((int)iconInfo.yHotspot);

            using (Bitmap maskBitmap = Bitmap.FromHbitmap(iconInfo.hbmMask))
            {
                // Is this a monochrome cursor?
                if (maskBitmap.Height == maskBitmap.Width * 2)
                {
                    Bitmap resultBitmap = new Bitmap(maskBitmap.Width, maskBitmap.Width);

                    Graphics desktopGraphics = Graphics.FromHwnd(Win32Stuff.GetDesktopWindow());
                    IntPtr desktopHdc = desktopGraphics.GetHdc();

                    IntPtr maskHdc = Win32Stuff.CreateCompatibleDC(desktopHdc);
                    IntPtr oldPtr = Win32Stuff.SelectObject(maskHdc, maskBitmap.GetHbitmap());

                    using (Graphics resultGraphics = Graphics.FromImage(resultBitmap))
                    {
                        IntPtr resultHdc = resultGraphics.GetHdc();

                        // These two operation will result in a black cursor over a white background.
                        // Later in the code, a call to MakeTransparent() will get rid of the white background.
                        Win32Stuff.BitBlt(resultHdc, (IntPtr)0, (IntPtr)0, (IntPtr)32, (IntPtr)32, maskHdc, (IntPtr)0, (IntPtr)32, ((IntPtr)Win32Stuff.TernaryRasterOperations.SRCCOPY));
                        Win32Stuff.BitBlt(resultHdc, (IntPtr)0, (IntPtr)0, (IntPtr)32, (IntPtr)32, maskHdc, (IntPtr)0, (IntPtr)0, (IntPtr)Win32Stuff.TernaryRasterOperations.SRCINVERT);

                        resultGraphics.ReleaseHdc(resultHdc);
                    }

                    IntPtr newPtr = Win32Stuff.SelectObject(maskHdc, oldPtr);
                    Win32Stuff.DeleteDC(newPtr);
                    Win32Stuff.DeleteDC(maskHdc);
                    desktopGraphics.ReleaseHdc(desktopHdc);

                    // Remove the white background from the BitBlt calls,
                    // resulting in a black cursor over a transparent background.
                    resultBitmap.MakeTransparent(Color.White);
                    return resultBitmap;
                }
            }

            Icon icon = Icon.FromHandle(hicon);

            return icon.ToBitmap();
        }

        public static byte[] imageToByteArray(System.Drawing.Bitmap imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
    }
}