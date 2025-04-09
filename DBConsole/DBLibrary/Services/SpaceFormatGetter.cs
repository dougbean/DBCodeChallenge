using DBLibrary.Model;

namespace DBLibrary.Services
{
    public class SpaceFormatGetter : IFileFormatGetter
    {
        public FormatEnum GetFileFormat(string fileName)
        {
            FormatEnum result = new FormatEnum();
            if (fileName.Contains(Constants.Space))
            {
                result = FormatEnum.space;
            }
            return result;
        }
    }
}
