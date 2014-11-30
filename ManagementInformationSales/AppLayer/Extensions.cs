using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppLayer
{
    public static class Extensions
    {
        public static FileContents ParseFile(this string fileName, string path)
        {
            FileContents content = new FileContents();
            IList<SaleInfo>  saleInfoList = new List<SaleInfo>();

            string[] infos = fileName.ParseFileName();

            content.ManagerName  = infos[0];

            using (var streamReader = new StreamReader(Path.Combine(path,fileName)))
            {
                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine();

                    if (line != null)
                    {

                        string[] data = line.ParseFileLine();

                        SaleInfo sale = new SaleInfo()
                        {
                            DateSale = data[0].ConvertToDate(),
                            ClientName = data[1],
                            ProductName = data[2],
                            CostSale = double.Parse(data[3])
                        };

                        saleInfoList.Add(sale);
                    }
                }
            }
           
            content.SalesInfo = saleInfoList;

            return content;
        }

        private static string[] ParseFileLine(this string contents)
        {
            string pattern = "[,]+";
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
                dateValue = DateTime.ParseExact(dateString, "dd/mm/yyyy", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                throw new Exception("Unable to convert"+ dateString + " to a date.");
            }
            return dateValue;
        }    
    }
}
