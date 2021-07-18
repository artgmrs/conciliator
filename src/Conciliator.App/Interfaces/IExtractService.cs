using Conciliator.App.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Conciliator.App.Interfaces
{
    public interface IExtractService : IDisposable
    {
        Task ProcessConciliation(List<IFormFile> files);
        XElement ParseToXElement(string path);
    }
}
