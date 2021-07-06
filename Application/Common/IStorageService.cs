using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public interface IStorageService
    {
        string GetFileUrl(string fileName);
        Task SaveFile(Stream stream, string fileName);
        Task DelFile(string fileName);
    }
}
