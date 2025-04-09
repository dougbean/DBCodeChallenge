using DBLibrary.Model;

namespace DBLibrary.Services
{
    public class CommaFormatGetter : IFileFormatGetter
    {
        public FormatEnum GetFileFormat(string fileName)
        {
            FormatEnum result = new FormatEnum();
            if (fileName.Contains(Constants.Comma))
            {
                result = FormatEnum.comma;
            }
            return result;
        }
    }
}
