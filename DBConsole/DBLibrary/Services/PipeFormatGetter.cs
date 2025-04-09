using DBLibrary.Model;

namespace DBLibrary.Services
{
    public class PipeFormatGetter : IFileFormatGetter
    {
        public FormatEnum GetFileFormat(string fileName)
        {
            FormatEnum result = new FormatEnum();
            if (fileName.Contains(Constants.Pipe))
            {
                result = FormatEnum.pipe;
            }
            return result;
        }
    }
}
