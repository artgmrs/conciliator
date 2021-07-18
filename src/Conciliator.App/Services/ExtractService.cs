using Conciliator.App.Interfaces;
using Conciliator.App.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Conciliator.App.Services
{
    public class ExtractService : IExtractService
    {
        private readonly IExtractRepository _extractRepo;
        private readonly string _tempFolder;

        public ExtractService(IExtractRepository extractRepo)
        {
            _extractRepo = extractRepo;
            _tempFolder = Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Temp")).FullName;
        }

        public async Task ProcessConciliation(List<IFormFile> files)
        {
            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(_tempFolder, formFile.FileName);
                    filePaths.Add(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            if (filePaths.Count > 0)
            {
                foreach (var file in filePaths)
                {
                    var extractList = new List<Extract>();

                    var xElement = ParseToXElement(file);
                    var transactions = xElement.Elements();
                    foreach (var item in transactions)
                    {
                        string transactionType = item.Element("TRNTYPE").Value;
                        DateTime datePosted = DateTime.ParseExact(item.Element("DTPOSTED").Value, "yyyyMMddHHmmss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                        decimal amount = Convert.ToDecimal(item.Element("TRNAMT").Value);
                        string memo = item.Element("MEMO").Value;

                        var extract = new Extract()
                        {
                            TransactionType = transactionType,
                            DatePosted = datePosted,
                            TransactionAmount = amount,
                            Memo = memo
                        };

                        extractList.Add(extract);
                    }

                    if (extractList.Count > 0)
                    {
                        foreach (var extract in extractList)
                        {
                            bool exists = await AlreadyExists(extract);

                            if (exists) return;

                            await _extractRepo.Add(extract);
                        }
                        await _extractRepo.SaveChanges();
                    }
                }
            }

            DeleteTrash();
        }

        public async Task<bool> AlreadyExists(Extract extract)
        {
            var exists = await _extractRepo.Search(e => e.TransactionAmount == extract.TransactionAmount &&
                                        e.TransactionType == extract.TransactionType &&
                                        e.Memo == extract.Memo &&
                                        e.DatePosted == extract.DatePosted);
            if (exists.Any()) return true;

            return false;
        }

        //reference: https://www.codeproject.com/Tips/14386/Class-to-Transform-an-OFX-Microsoft-Money-File-int
        public XElement ParseToXElement(string path)
        {
            var tags = from line in File.ReadAllLines(path)
                       where line.Contains("<STMTTRN>") ||
                       line.Contains("<TRNTYPE>") ||
                       line.Contains("<DTPOSTED>") ||
                       line.Contains("<TRNAMT>") ||
                       line.Contains("<FITID>") ||
                       line.Contains("<CHECKNUM>") ||
                       line.Contains("<MEMO>")
                       select line;

            XElement element = new XElement("extract");
            XElement child = null;

            foreach (var line in tags)
            {
                if (line.IndexOf("STMTTRN") != -1)
                {
                    child = new XElement("STMTTRN");
                    element.Add(child);
                }

                var tagName = GetTagName(line);
                var elementChild = new XElement(tagName)
                {
                    Value = GetTagValue(line)
                };

                child.Add(elementChild);
            }

            return element;
        }

        private string GetTagName(string line)
        {
            int initial = line.IndexOf("<") + 1;
            int end = line.IndexOf(">");
            end -= initial;
            return line.Substring(initial, end);
        }

        private string GetTagValue(string line)
        {
            int initial = line.IndexOf(">") + 1;
            string returnValue = line.Substring(initial).Trim();
            if (returnValue.IndexOf("[") != -1)
            {
                returnValue = returnValue.Substring(0, 14);
            }

            return returnValue;
        }

        private void DeleteTrash()
        {
            string[] allFiles = Directory.GetFiles(_tempFolder);

            if (allFiles == null) return;

            foreach (string file in allFiles)
            {
                File.Delete(file);
            }
        }

        public void Dispose()
        {
            _extractRepo?.Dispose();
        }
    }
}
