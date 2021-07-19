using Microsoft.AspNetCore.Http;
using System.IO;

namespace PagNet.Interface.Helpers
{
    public static class FormFileExtensions
    {
        public static void SaveTo(this IFormFile formFile, string fullFilename)
        {
            byte[] buffer = new byte[formFile.Length];
            formFile.OpenReadStream().Read(buffer, 0, buffer.Length);
            File.WriteAllBytes(fullFilename, buffer);
        }
    }
}
