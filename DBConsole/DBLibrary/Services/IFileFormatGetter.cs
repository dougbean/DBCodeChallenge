using DBLibrary.Model;

namespace DBLibrary.Services
{
    public interface IFileFormatGetter
    {
        public FormatEnum GetFileFormat(string fileName);
    }
}
