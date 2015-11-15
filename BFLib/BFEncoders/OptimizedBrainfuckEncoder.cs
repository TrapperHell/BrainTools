using System;
using System.Linq;
using System.Text;

namespace BrainTools
{
    /// <summary>
    /// The following code was adapted from http://codegolf.stackexchange.com/questions/5418/brainfuck-golfer/5440#5440
    /// Originally written by Keith Randall.
    /// <para>
    /// Licensed under Creative Commons CC-BY-SA.
    /// </para>
    /// </summary>
    public class OptimizedBrainfuckEncoder : IBrainfuckEncoder
    {
        static string[,] G = new string[256, 256];

        static OptimizedBrainfuckEncoder()
        {
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    int delta = y - x;

                    if (delta > 128)
                        delta -= 256;
                    else if (delta < -128)
                        delta += 256;

                    if (delta >= 0)
                        G[x, y] = Repeat("+", delta);
                    else
                        G[x, y] = Repeat("-", -delta);
                }
            }

            // Keep applying rules until we can't find any more shortenings  
            bool iter = true;
            while (iter)
            {
                iter = false;

                // Multiplication by n/d
                for (int x = 0; x < 256; x++)
                {
                    for (int n = 1; n < 40; n++)
                    {
                        for (int d = 1; d < 40; d++)
                        {
                            int j = x;
                            int y = 0;
                            for (int i = 0; i < 256; i++)
                            {
                                if (j == 0) break;
                                j = (j - d + 256) & 255;
                                y = (y + n) & 255;
                            }

                            if (j == 0)
                            {
                                String s = "[" + Repeat("-", d) + ">" + Repeat("+", n) + "<]>";

                                if (s.Length < G[x, y].Length)
                                {
                                    G[x, y] = s;
                                    iter = true;
                                }
                            }

                            j = x;
                            y = 0;

                            for (int i = 0; i < 256; i++)
                            {
                                if (j == 0) break;
                                j = (j + d) & 255;
                                y = (y - n + 256) & 255;
                            }

                            if (j == 0)
                            {
                                String s = "[" + Repeat("+", d) + ">" + Repeat("-", n) + "<]>";
                                if (s.Length < G[x, y].Length)
                                {
                                    G[x, y] = s;
                                    iter = true;
                                }
                            }
                        }
                    }
                }

                // Combine number schemes
                for (int x = 0; x < 256; x++)
                {
                    for (int y = 0; y < 256; y++)
                    {
                        for (int z = 0; z < 256; z++)
                        {
                            if (G[x, z].Length + G[z, y].Length < G[x, y].Length)
                            {
                                G[x, y] = G[x, z] + G[z, y];
                                iter = true;
                            }
                        }
                    }
                }
            }
        }

        private static string Repeat(string s, int n)
        {
            if (n <= 0)
                return String.Empty;
            return String.Concat(Enumerable.Repeat(s, n));
        }

        public static string Generate(string s)
        {
            char lastC = (char)0;
            StringBuilder sb = new StringBuilder();

            foreach (char c in s)
            {
                string a = G[lastC, c];
                string b = G[0, c];

                if (a.Length <= b.Length)
                    sb.Append(a);
                else
                    sb.Append(">" + b);

                sb.Append(".");
                lastC = c;
            }

            return sb.ToString();
        }

        public static string Generate(byte[] data)
        {
            byte lastD = 0;
            StringBuilder sb = new StringBuilder();

            foreach (byte d in data)
            {
                string a = G[lastD, d];
                string b = G[0, d];

                if (a.Length <= b.Length)
                    sb.Append(a);
                else
                    sb.Append(">" + b);

                sb.Append(".");
                lastD = d;
            }

            return sb.ToString();
        }


        public string Encode(byte[] data)
        {
            //return Generate(Encoding.UTF8.GetString(data));

            return Generate(data);
        }
    }
}