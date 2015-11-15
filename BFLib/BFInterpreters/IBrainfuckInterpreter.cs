using System.IO;

namespace BrainTools
{
    public interface IBrainFuckInterpreter
    {
        char[] GetSupportedCommands();

        void Run(string code);

        string Encode(Stream input);
    }
}