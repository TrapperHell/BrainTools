using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BrainTools
{
    /// <summary>
    /// Methods for working with Brainfuck.
    /// </summary>
    public static class Brainfuck
    {
        static List<Type> bfInterpreterTypes, bfEncoderTypes;



        static Brainfuck()
        {
            Type bfIInterpreterType = typeof(IBrainFuckInterpreter);
            bfInterpreterTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterfaces().Contains(bfIInterpreterType)).ToList();

            Type bfIEncoderType = typeof(IBrainfuckEncoder);
            bfEncoderTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterfaces().Contains(bfIEncoderType)).ToList();
        }



        /// <summary>
        /// Finds the most relevant BF interpreter for the provided code and runs it.
        /// </summary>
        /// <param name="code">
        /// The Brainfuck code to run.
        /// </param>
        public static void Run(string code)
        {
            if (String.IsNullOrWhiteSpace(code))
                return;

            List<IBrainFuckInterpreter> bfInterpreters = new List<IBrainFuckInterpreter>();
            bfInterpreterTypes.ForEach(bfit => bfInterpreters.Add((IBrainFuckInterpreter)Activator.CreateInstance(bfit)));
            bfInterpreters = bfInterpreters.OrderBy(bfi => bfi.GetSupportedCommands().Length).ToList();

            // Get the Brainfuck interpreter with the most supported commands...
            IBrainFuckInterpreter maxBf = bfInterpreters.Last();

            // Sanitize the upper-case code, removing all unsupported commands before the first @ occurrence
            string sanitizedCode = code.Contains('@') ? code.Substring(0, code.IndexOf('@')) : code;
            sanitizedCode = String.Join<char>(String.Empty, sanitizedCode.ToUpperInvariant().Where(c => maxBf.GetSupportedCommands().Contains(c)));

            /*
             * Extended Type 3 supports hex { A-F, 0-9 } which may be prone to
             * false-positives in case comments are inserted. As such if there is are two
             * or more consecutive hex characters, these are removed as well.
            */
            sanitizedCode = Regex.Replace(sanitizedCode, "[A-F0-9]{2,}", String.Empty);

            foreach (IBrainFuckInterpreter bf in bfInterpreters)
            {
                // Check if the BF Interpreter can execute the sanitized code, but run the actual code
                if (!sanitizedCode.Any(c => !bf.GetSupportedCommands().Contains(c)))
                {
                    bf.Run(code);
                    break;
                }
            }
        }

        /// <summary>
        /// Runs the provided code with the specified Brainfuck interpreter.
        /// </summary>
        /// <typeparam name="T">
        /// The interpreter to use for running the code.
        /// </typeparam>
        /// <param name="code">
        /// The Brainfuck code to run.
        /// </param>
        public static void Run<T>(string code) where T : IBrainFuckInterpreter
        {
            IBrainFuckInterpreter bfInterpreter = Activator.CreateInstance<T>();
            bfInterpreter.Run(code);
        }

        /// <summary>
        /// Encodes the specified Stream using all the available Brainfuck Encoders and
        /// returns the shortest resultant Brainfuck code.
        /// <para>
        /// Since all encoders are being executed, this process may take time. Use
        /// sparingly.
        /// </para>
        /// </summary>
        /// <param name="input">
        /// The stream whose content is to be Brainfuck-encoded.
        /// </param>
        /// <returns>
        /// Returns the shortest generated Brainfuck-encoded code.
        /// </returns>
        public static string Encode(Stream input)
        {
            if (input != null && input.CanRead && input.CanSeek && input.Length > 0 && input.Length <= 1048576)
            {
                // Read content into memory, and use it
                byte[] data = new byte[input.Length];
                input.Read(data, 0, data.Length);

                return Encode(data);
            }
            else
                return string.Empty;
        }

        public static string Encode(string plainText)
        {
            return Encode(Encoding.UTF8.GetBytes(plainText));
        }

        public static string Encode(byte[] data)
        {
            if (data == null)
                return string.Empty;

            List<string> results = new List<string>();

            foreach (Type bfEncoderType in bfEncoderTypes)
            {
                IBrainfuckEncoder bfEncoder = (IBrainfuckEncoder)Activator.CreateInstance(bfEncoderType);
                results.Add(bfEncoder.Encode(data));
            }

            return results.OrderBy(r => r.Length).First();
        }

        /// <summary>
        /// Encodes the specified Stream into Brainfuck code using the specified encoder.
        /// </summary>
        /// <typeparam name="T">
        /// The encoder to use for running the code.
        /// </typeparam>
        /// <param name="input">
        /// The stream whose content is to be Brainfuck-encoded.
        /// </param>
        /// <returns>
        /// Returns the generated Brainfuck-encoded code.
        /// </returns>
        public static string Encode<T>(Stream input) where T : IBrainfuckEncoder
        {
            if (input.CanRead && input.CanSeek && input.Length <= 1048576)
            {
                byte[] data = new byte[input.Length];
                input.Read(data, 0, data.Length);

                return Encode<T>(data);
            }
            else
                return string.Empty;
        }

        public static string Encode<T>(string plainText) where T : IBrainfuckEncoder
        {
            return Encode<T>(Encoding.UTF8.GetBytes(plainText));
        }

        public static string Encode<T>(byte[] data) where T : IBrainfuckEncoder
        {
            IBrainfuckEncoder bfEncoder = Activator.CreateInstance<T>();
            return bfEncoder.Encode(data);
        }
    }
}