using System.IO;
using System.Linq;

namespace Selenium.WebDriver.ChromeDriver.NuPkg.Test.Lib
{
    public class ExecutableFile
    {
        public enum Format
        {
            Unknown,
            PE,
            ELF,
            MachO
        }

        internal static Format DetectFormat(string path)
        {
            const int maxHeaderBytesLength = 4;
            if (new FileInfo(path).Length < maxHeaderBytesLength) return Format.Unknown;
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var headerBytes = new byte[maxHeaderBytesLength];
                stream.Read(headerBytes, 0, headerBytes.Length);
                if (Enumerable.SequenceEqual(headerBytes.Take(2), new[] { (byte)'M', (byte)'Z' })) return Format.PE;
                if (Enumerable.SequenceEqual(headerBytes.Take(4), new[] { (byte)0x7f, (byte)'E', (byte)'L', (byte)'F' })) return Format.ELF;
                if (Enumerable.SequenceEqual(headerBytes.Take(4), new[] { (byte)0xcf, (byte)0xfa, (byte)0xed, (byte)0xfe })) return Format.MachO;
            }
            return Format.Unknown;
        }
    }
}
