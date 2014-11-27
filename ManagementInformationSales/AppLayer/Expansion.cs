using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppLayer
{
    public static class Expansion
    {
        public static IList<SaleInfoFile> ParseFile(this string file)
        {
            IList<SaleInfoFile> salesInfo = new List<SaleInfoFile>();

            string[] infos = file.ParseFileName();

            string managerName = infos[0];
            DateTime dateSale = infos[1].ConvertToDate(); 

            if (File.Exists(file))
            {
                string[] lines = File.ReadAllLines(file);

                foreach (string line in lines)
                {
                    SaleInfoFile info = new SaleInfoFile()
                    {
                        ManagerName = managerName,
                        DateSale = dateSale
                    };
 
                }

            }
          
            return salesInfo;
        }

        private static string[] ParseFileContents(this string contents)
        {
            string pattern = "[,'']+";
            Regex r = new Regex(pattern);
            return r.Split(contents);
        }

        private static string[] ParseFileName(this string fileName)
        {
            string pattern = "[_.]+";
            Regex r = new Regex(pattern);
            return r.Split(fileName);
        }

        private static DateTime ConvertToDate(this string dateString)
        {
            DateTime dateValue;

            try
            {
                dateValue = DateTime.ParseExact(dateString, "DDMMYYYY", null);
            }
            catch (FormatException)
            {
                throw new Exception("Unable to convert"+ dateString + " to a date.");
            }
            return dateValue;
        }
    }
}
