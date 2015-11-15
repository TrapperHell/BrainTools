using System;
using System.IO;

namespace BrainTools
{
    public class BrainfuckExtendedType3Interpreter : BrainfuckExtendedType2Interpreter
    {
        static readonly char[] supportedCommands = new char[]
        {
            // Basic
            '+', '-', '>', '<', '[', ']', '.', ',',
            // Extended Type 1
            '@', '$', '!', '}', '{', '~', '^', '&', '|',
            // Extended Type 2
            '?', ')', '(', '*', '/', '=', '_', '%',
            // Extended Type 3
            'X', 'x', 'M', 'm', 'L', 'l', ':', '#',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F'
        };

        bool isComment = false;



        public BrainfuckExtendedType3Interpreter(Stream stdin, Stream stdout)
        {
            this.stdin = stdin;
            this.stdout = stdout;
        }

        public BrainfuckExtendedType3Interpreter()
            : this(Console.OpenStandardInput(), Console.OpenStandardOutput())
        { }



        public override char[] GetSupportedCommands()
        {
            return supportedCommands;
        }

        protected override void ProcessInstruction(char instruction)
        {
            bool instructionProcessed = true, moveIP = true;

            if (!this.isComment)
            {
                switch (instruction)
                {
                    case 'X':
                        break;
                    case 'x':
                        break;
                    case 'M':
                        break;
                    case 'm':
                        break;
                    case 'L':
                        break;
                    case 'l':
                        break;
                    case ':':
                        break;
                    case '#':
                        this.isComment = !this.isComment;
                        break;
                    default:
                        // If the instruction is 0-9 or A-F, process it
                        if ((48 <= instruction && instruction <= 57) || (65 <= instruction && instruction <= 70))
                        {
                            int hexVal = Convert.ToInt32("0" + instruction, 16);
                            this.tape.Cell = hexVal * 16;
                        }
                        else
                            instructionProcessed = false;

                        break;
                }
            }

            if (!instructionProcessed)
                base.ProcessInstruction(instruction);
            else if (moveIP)
                this.instructionPointer++;
        }
    }
}