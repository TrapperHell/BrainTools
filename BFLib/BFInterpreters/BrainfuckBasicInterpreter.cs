using System;
using System.IO;

namespace BrainTools
{
    public class BrainfuckBasicInterpreter : IBrainFuckInterpreter
    {
        static readonly char[] supportedCommands = new char[]
        {
            // Basic
            '+', '-', '>', '<', '[', ']', '.', ','
        };

        protected Tape tape;
        protected int instructionPointer;
        protected int loopCounter = 0;
        protected Stream stdin, stdout;
        protected char[] instructions;



        public BrainfuckBasicInterpreter(Stream stdin, Stream stdout)
        {
            this.stdin = stdin;
            this.stdout = stdout;
        }

        public BrainfuckBasicInterpreter()
            : this(Console.OpenStandardInput(), Console.OpenStandardOutput())
        { }



        public virtual char[] GetSupportedCommands()
        {
            return supportedCommands;
        }

        public virtual void Run(string code)
        {
            this.tape = new Tape();
            this.instructions = code.ToCharArray();

            while (this.instructionPointer >= 0 && this.instructionPointer < instructions.Length)
                ProcessInstruction(instructions[this.instructionPointer]);
        }

        public virtual string Encode(Stream input)
        {
            /* The following code was adapted from https://github.com/splitbrain/ook/blob/master/util.php
             * Licensed under the GNU General Public License Version 2.
             */

            int value = 0;          // Value of current pointer
            string result = "";

            int temp = input.ReadByte();

            while (temp != -1)
            {
                int diff = temp - value;        // Difference between current value and target value

                value = temp;

                // Repeat current character
                if (diff == 0)
                {
                    result += ">.<";
                    temp = input.ReadByte();
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

                temp = input.ReadByte();
            }

            // Cleanup
            return result.Replace("<>", "");
        }

        protected virtual void ProcessInstruction(char instruction)
        {
            bool moveIP = true;

            switch (instruction)
            {
                case '+':
                    tape.Cell++;
                    break;
                case '-':
                    tape.Cell--;
                    break;
                case '>':
                    tape.MoveRight();
                    break;
                case '<':
                    tape.MoveLeft();
                    break;
                case '[':
                    if (tape.Cell == 0)
                    {
                        this.loopCounter++;
                        while (this.loopCounter != 0 && ++this.instructionPointer < this.instructions.Length)
                        {
                            if (this.instructions[this.instructionPointer] == '[')
                                this.loopCounter++;
                            else if (this.instructions[this.instructionPointer] == ']')
                                this.loopCounter--;
                        }
                    }
                    break;
                case ']':
                    this.loopCounter++;
                    while (this.loopCounter != 0 && --this.instructionPointer >= 0)
                    {
                        if (this.instructions[this.instructionPointer] == ']')
                            this.loopCounter++;
                        else if (this.instructions[this.instructionPointer] == '[')
                            this.loopCounter--;
                    }
                    moveIP = false;
                    break;
                case '.':
                    stdout.WriteByte((byte)tape.Cell);
                    break;
                case ',':
                    tape.Cell = stdin.ReadByte();
                    break;
            }

            if (moveIP)
                this.instructionPointer++;
        }
    }
}