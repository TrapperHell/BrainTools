using System;

namespace BrainTools
{
    /// <summary>
    /// The following code was adapted from https://github.com/splitbrain/ook/blob/master/util.php
    /// <para>
    /// Licensed under the GNU General Public License Version 2.
    /// </para>
    /// </summary>
    public class DefaultBrainfuckEncoder : IBrainfuckEncoder
    {
        public string Encode(byte[] data)
        {
            int value = 0;          // Value of current pointer
            string result = "";

            for (int i = 0; i < data.Length; i++)
            {
                int temp = data[i];
                int diff = temp - value;        // Difference between current value and target value

                value = temp;

                // Repeat current character
                if (diff == 0)
                {
                    result += ">.<";
                    temp = data[i];
                    continue;
                }

                // Is it worth making a loop?

                // No. A bunch of + or - consume less space than the loop.
                if (Math.Abs(diff) < 10)
                {
                    if (diff > 0)
                        result += ">" + new string('+', diff);
                    else if (diff < 0)
                        result += ">" + new string('-', Math.Abs(diff));
                }
                // Yes, create a loop. This will make the resulting code more compact.
                else
                {
                    int loop = (int)Math.Sqrt(Math.Abs(diff));

                    // Set loop counter
                    result += new string('+', loop);

                    // Execute loop, then add remainder
                    if (diff > 0)
                    {
                        result += "[->" + new string('+', loop) + "<]";
                        result += ">" + new string('+', diff - (int)Math.Pow(loop, 2));
                    }
                    else if (diff < 0)
                    {
                        result += "[->" + new string('-', loop) + "<]";
                        result += ">" + new string('-', Math.Abs(diff) - (int)Math.Pow(loop, 2));
                    }
                }

                result += ".<";
            }

            // Cleanup
            return result.Replace("<>", "");
        }
    }
}