using System;
using System.Drawing;

namespace BrainTools
{
    /// <summary>
    /// Methods for working with Brainloller.
    /// </summary>
    public static class Brainloller
    {
        private enum IPDirections
        {
            East = 1,
            South = 2,
            West = 3,
            North = 4
        };

        private static BiDictionary<char, Color> ColorMap = new BiDictionary<char, Color>()
        {
            { '>', Color.FromArgb(255, 0, 0) },
            { '<', Color.FromArgb(128, 0, 0) },
            { '+', Color.FromArgb(0, 255, 0) },
            { '-', Color.FromArgb(0, 128, 0) },
            { '.', Color.FromArgb(0, 0, 255) },
            { ',', Color.FromArgb(0, 0, 128) },
            { '[', Color.FromArgb(255, 255, 0) },
            { ']', Color.FromArgb(128, 128, 0) },
            { '»', Color.FromArgb(0, 255, 255) }, // RotateCW
            { '«', Color.FromArgb(0, 128, 128) }, // RotateCCW

            // Extended Type 1
            { '@', Color.FromArgb(0, 192, 64) },
            { '$', Color.FromArgb(192, 64, 0) },
            { '!', Color.FromArgb(64, 0, 192) },
            { '}', Color.FromArgb(64, 192, 0) },
            { '{', Color.FromArgb(192, 0, 64) },
            { '~', Color.FromArgb(0, 64, 192) },
            { '^', Color.FromArgb(0, 192, 0) },
            { '&', Color.FromArgb(192, 0, 0) },
            { '|', Color.FromArgb(0, 0, 192) }
        };



        /// <summary>
        /// Encodes the given Brainfuck code into a square-shaped bitmap.
        /// </summary>
        /// <param name="code">
        /// The Brainfuck code to insert.
        /// </param>
        /// <param name="gapFiller">
        /// The color to use for NOPs.
        /// </param>
        /// <returns>
        /// The encoded bitmap.
        /// </returns>
        public static Bitmap Encode(string code, Color gapFiller)
        {
            return Encode(code, (int)Math.Ceiling(Math.Sqrt(code.Length)), gapFiller);
        }

        /// <summary>
        /// Encodes the given Brainfuck code into a bitmap.
        /// </summary>
        /// <param name="code">
        /// The Brainfuck code to insert.
        /// </param>
        /// <param name="width">
        /// The resulting bitmap width.
        /// </param>
        /// <param name="gapFiller">
        /// The color to use for NOPs.
        /// </param>
        /// <returns>
        /// The encoded bitmap.
        /// </returns>
        public static Bitmap Encode(string code, int width, Color gapFiller)
        {
            if (ColorMap.ContainsKey(gapFiller))
                throw new ArgumentException("Specified GapFiller color is a system-reserved color.");

            int height = (int)Math.Ceiling(code.Length / (width - 2d));

            Bitmap newBmp = new Bitmap(width, height);
            int length = code.Length;

            int curX = 0, curY = 0;
            IPDirections dir = IPDirections.East;

            for (int i = 0; i < code.Length; i++)
            {
                Color clr = Color.FromArgb(0, 0, 0);

                if (curX == newBmp.Width - 1 && dir == IPDirections.East)
                {
                    newBmp.SetPixel(curX, curY, ColorMap['»']);
                    newBmp.SetPixel(curX, curY + 1, ColorMap['»']);
                    curX--;
                    curY++;
                    dir = IPDirections.West;
                }
                else if (curX == 0 && dir == IPDirections.West)
                {
                    newBmp.SetPixel(curX, curY, ColorMap['«']);
                    newBmp.SetPixel(curX, curY + 1, ColorMap['«']);
                    curX++;
                    curY++;
                    dir = IPDirections.East;
                }

                if (!ColorMap.TryGetValue(code[i], out clr))
                    continue;

                newBmp.SetPixel(curX, curY, clr);

                if (dir == IPDirections.East)
                    curX++;
                else if (dir == IPDirections.West)
                    curX--;
            }

            if (dir == IPDirections.East)
                for (int i = curX; i < newBmp.Width; i++)
                    newBmp.SetPixel(i, curY, gapFiller);
            else if (dir == IPDirections.West)
                for (int i = curX; i > -1; i--)
                    newBmp.SetPixel(i, curY, gapFiller);

            return newBmp;
        }

        /// <summary>
        /// Decodes the given Brainloller bitmap.
        /// </summary>
        /// <param name="bmp">
        /// The bitmap to decode.
        /// </param>
        /// <returns>
        /// The Brainfuck code contained in the bitmap.
        /// </returns>
        public static string Decode(Bitmap bmp)
        {
            string code = string.Empty;
            IPDirections dir = IPDirections.East;
            int curX = 0, curY = 0;

            while ((curX >= 0 && curX < bmp.Width) && (curY >= 0 && curY < bmp.Height))
            {
                Color clr = bmp.GetPixel(curX, curY);

                char c;
                if (ColorMap.TryGetValue(clr, out c))
                {
                    if (c == '»')
                    {
                        dir += 1;

                        if (!Enum.IsDefined(typeof(IPDirections), dir))
                            dir = IPDirections.East;
                    }
                    else if (c == '«')
                    {
                        dir -= 1;

                        if (!Enum.IsDefined(typeof(IPDirections), dir))
                            dir = IPDirections.North;
                    }
                    else
                        code += c;
                }

                switch (dir)
                {
                    case IPDirections.East:
                        curX++;
                        break;
                    case IPDirections.West:
                        curX--;
                        break;
                    case IPDirections.North:
                        curY--;
                        break;
                    case IPDirections.South:
                        curY++;
                        break;
                    default:
                        break;
                }
            }

            return code;
        }

        /// <summary>
        /// Shrinks the given bitmap.
        /// </summary>
        /// <param name="bmp">
        /// The bitmap to shrink.
        /// </param>
        /// <param name="pxDimension">
        /// The factor by which to shrink the bitmap.
        /// </param>
        /// <returns>
        /// The reduced bitmap.
        /// </returns>
        public static Bitmap Reduce(Bitmap bmp, int pxDimension)
        {
            Bitmap newBmp = new Bitmap(bmp.Width / pxDimension, bmp.Height / pxDimension);

            for (int row = 0; row < bmp.Height; row += pxDimension)
                for (int col = 0; col < bmp.Width; col += pxDimension)
                    newBmp.SetPixel(col / 10, row / 10, bmp.GetPixel(col, row));

            return newBmp;
        }

        /// <summary>
        /// Enlarges the given bitmap.
        /// </summary>
        /// <param name="bmp">
        /// The bitmap to enlarge.
        /// </param>
        /// <param name="pxDimension">
        /// The factor by which to enlarge the bitmap.
        /// </param>
        /// <returns>
        /// The enlarged bitmap.
        /// </returns>
        public static Bitmap Enlarge(Bitmap bmp, int pxDimension)
        {
            Bitmap newBmp = new Bitmap(bmp.Width * pxDimension, bmp.Height * pxDimension);
            Graphics g = Graphics.FromImage(newBmp);

            for (int row = 0; row < bmp.Height; row++)
            {
                for (int col = 0; col < bmp.Width; col++)
                {
                    Rectangle rect = new Rectangle(col * pxDimension, row * pxDimension, pxDimension, pxDimension);
                    Pen pen = new Pen(bmp.GetPixel(col, row));
                    SolidBrush brush = new SolidBrush(bmp.GetPixel(col, row));
                    g.DrawRectangle(pen, rect);
                    g.FillRectangle(brush, rect);
                }
            }

            g.Dispose();

            return newBmp;
        }
    }
}