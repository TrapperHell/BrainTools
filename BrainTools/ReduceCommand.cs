﻿using System;
using System.Drawing;
using System.IO;

using ManyConsole;

namespace BrainTools
{
    class ReduceCommand : ConsoleCommand
    {
        private int factor;
        private Stream input = Console.OpenStandardInput();
        private Stream output = Console.OpenStandardOutput();

        public ReduceCommand()
        {
            IsCommand("reduce", "Shrink an image by a given factor.");

            HasOption("o|output=", "OPTIONAL. The output image. Defaults to stdout.", v => output = File.OpenWrite(v));

            HasAdditionalArguments(2, "<image | -> <factor>");

            SkipsCommandSummaryBeforeRunning();
        }

        public override int Run(string[] remainingArguments)
        {
            if (remainingArguments[0] != "-")
                input = File.OpenRead(remainingArguments[0]);
            factor = int.Parse(remainingArguments[1]);

            Brainloller.Reduce(new Bitmap(input), factor).Save(output, System.Drawing.Imaging.ImageFormat.Png);
            return 0;
        }
    }
}
