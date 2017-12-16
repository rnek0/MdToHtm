using System;
using System.IO;

namespace MdToHtm
{
    public delegate void Converted(object sender, ConvertedEventArgs arg);

    public class ConverterMD
    {
        public event Converted FileConverted;

        public string Fichier { get; set; }

        /// <summary>
        /// Convert .md to .html
        /// </summary>
        public void Convert()
        {
            var top = "<!doctype html><html lang=\"fr\"><head><meta charset=\"utf-8\"></head><body>";
            var bottom = "</body><html>";

            var pos = Fichier.LastIndexOf('\\');
            var uri = Fichier.Substring(0, pos);
            var fileName = Fichier.Substring(pos + 1);
            fileName = fileName.Substring(0, fileName.Length - 3);
            var FileResult = $"{uri}\\{fileName}.html";

            if (!string.IsNullOrEmpty(Fichier))
            {
                using (var reader = new StreamReader(Fichier))
                {
                    using (var writer = new System.IO.StreamWriter(FileResult))
                    {
                        CommonMark.CommonMarkConverter.Convert(reader, writer);
                    }
                }
                var Str = $"{top} {Environment.NewLine} {File.ReadAllText(FileResult)} {bottom}";
                File.WriteAllText(FileResult, Str);

                FileConverted?.Invoke(this, new ConvertedEventArgs { FileConverted = Fichier });
            }
        }
    }

    public class ConvertedEventArgs: EventArgs
    {
        public string FileConverted { get; set; }
    }   
}