using HelpTrade;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace HelpTrade
{
    public class FileManager
    {
        private static readonly string configDirPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TradeHelp");

        private static readonly string fileName = "info.hkb";
        private static readonly string filePath = Path.Combine(configDirPath, fileName);

        static public void saveFile(Lastdata ld)
        {
            var json = JsonSerializer.Serialize(ld, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            });
            if (!Directory.Exists(configDirPath))
                Directory.CreateDirectory(configDirPath);

            File.WriteAllText(filePath, json);
        }

        public static Lastdata loadFile(int nbCurrencies)
        {
            Lastdata alim = new Lastdata(nbCurrencies);

            if (!Directory.Exists(configDirPath))
            {
                Directory.CreateDirectory(configDirPath);
                saveFile(alim);
                return alim;
            }

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Lastdata>(json);
        }
    }
}
