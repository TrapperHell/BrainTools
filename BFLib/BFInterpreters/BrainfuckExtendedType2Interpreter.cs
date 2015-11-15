using System;
using System.IO;
using System.Linq;

namespace BrainTools
{
    public class BrainfuckExtendedType2Interpreter : BrainfuckExtendedType1Interpreter
    {
        static readonly char[] supportedCommands = new char[]
        {
            // Basic
            '+', '-', '>', '<', '[', ']', '.', ',',
            // Extended Type 1
            '@', '$', '!', '}', '{', '~', '^', '&', '|',
            // Extended Type 2
            '?', ')', '(', '*', '/', '=', '_', '%'
        };



        public BrainfuckExtendedType2Interpreter(Stream stdin, Stream stdout)
        {
            this.stdin = stdin;
            this.stdout = stdout;
        }

        public BrainfuckExtendedType2Interpreter()
            : this(Console.OpenStandardInput(), Console.OpenStandardOutput())
        { }



        public override char[] GetSupportedCommands()
        {
            return supportedCommands;
        }

        public override void Run(string code)
        {
            this.instructions = code.ToCharArray();

            this.tape = new Tape();
            // Insert Storage at index #0
            this.tape.Content.Insert(0, this.storage);
            // Insert Source at index #1
            this.tape.Content.InsertRange(1, code.Select(c => (int)c));
            this.tape.Pointer = this.tape.Content.Count - 1;

            // Insert Cell initializer data
            if (code.Contains('@'))
                this.tape.Content.InsertRange(this.tape.Content.Count - 1, code.Substring(code.LastIndexOf('@') + 1).Select(c => (int)c));

            while (this.instructionPointer < this.tape.Content.Count)
                ProcessInstruction((char)this.tape.Content[this.instructionPointer]);
        }

        protected override void ProcessInstruction(char instruction)
        {
            bool instructionProcessed = true, moveIP = true;

            switch (instruction)
            {
                case '?':
                    this.instructionPointer = this.tape.Pointer;
                    moveIP = false;
                    break;
                case ')':
                    this.tape.Content.Insert(this.tape.Pointer, 0);
                    moveIP = false;
                    break;
                case '(':
                    this.tape.Content.RemoveAt(this.tape.Pointer);
                    moveIP = false;
                    break;
                case '*':
                    this.tape.Cell = this.tape.Cell * this.storage;
                    break;
                case '/':
                    this.tape.Cell = this.tape.Cell / this.storage;
                    break;
                case '=':
                    this.tape.Cell = this.tape.Cell + this.storage;
                    break;
                case '_':
                    this.tape.Cell = this.tape.Cell - this.storage;
                    break;
                case '%':
                    this.tape.Cell = this.tape.Cell % this.storage;
                    break;

                // The looping operators should no longer work on the instruction set, but on the tape itself
                case '[':
                    if (tape.Cell == 0)
                    {
                        this.loopCounter++;
                        while (this.loopCounter != 0)
                        {
                            this.instructionPointer++;
                            if (this.tape.Content[this.instructionPointer] == '[')
                                this.loopCounter++;
                            else if (this.tape.Content[this.instructionPointer] == ']')
                                this.loopCounter--;
                            // If @ exists before the partner-bracket, look no further
                            else if (this.tape.Content[this.instructionPointer] == '@')
                                this.loopCounter = 0;
                        }
                    }
                    break;
                case ']':
                    this.loopCounter++;
                    while (this.loopCounter != 0)
                    {
                        this.instructionPointer--;
                        if (this.tape.Content[this.instructionPointer] == ']')
                            this.loopCounter++;
                        else if (this.tape.Content[this.instructionPointer] == '[')
                            this.loopCounter--;
                    }
                    moveIP = false;
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