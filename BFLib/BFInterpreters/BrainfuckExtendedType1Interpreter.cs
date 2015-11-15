using System;
using System.IO;

namespace BrainTools
{
    public class BrainfuckExtendedType1Interpreter : BrainfuckBasicInterpreter
    {
        static readonly char[] supportedCommands = new char[]
        {
            // Basic
            '+', '-', '>', '<', '[', ']', '.', ',',
            // Extended Type 1
            '@', '$', '!', '}', '{', '~', '^', '&', '|'
        };

        protected int storage;



        public BrainfuckExtendedType1Interpreter(Stream stdin, Stream stdout)
        {
            this.stdin = stdin;
            this.stdout = stdout;
        }

        public BrainfuckExtendedType1Interpreter()
            : this(Console.OpenStandardInput(), Console.OpenStandardOutput())
        { }



        public override char[] GetSupportedCommands()
        {
            return supportedCommands;
        }

        protected override void ProcessInstruction(char instruction)
        {
            bool instructionProcessed = true, moveIP = true;

            switch (instruction)
            {
                case '@':
                    this.instructionPointer = this.instructions.Length;
                    return;
                case '$':
                    storage = tape.Cell;
                    break;
                case '!':
                    tape.Cell = storage;
                    break;
                case '}':
                    tape.Cell = (byte)tape.Cell >> 1;
                    break;
                case '{':
                    tape.Cell = (byte)tape.Cell << 1;
                    break;
                case '~':
                    tape.Cell = (byte)~tape.Cell;
                    break;
                case '^':
                    tape.Cell = (byte)tape.Cell ^ storage;
                    break;
                case '&':
                    tape.Cell = (byte)tape.Cell & storage;
                    break;
                case '|':
                    tape.Cell = (byte)tape.Cell | storage;
                    break;
                default:
                    instructionProcessed = false;
                    break;
            }

            if (!instructionProcessed)
                base.ProcessInstruction(instruction);
            else if (moveIP)
                this.instructionPointer++;
        }
    }
}