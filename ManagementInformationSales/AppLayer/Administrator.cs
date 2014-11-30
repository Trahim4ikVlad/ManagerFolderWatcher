using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AppLayer;
using ManagementInformationSales;

namespace AppLayer
{
    public class Administrator
    {
        private string _fileName;
        private string _path;
        private Object _thisLock;

        public string ViewedDirictory { get; set; }

        public Administrator(string fileName, string path)
        {
            this._fileName = fileName;
            this._path = path;
            _thisLock = new object();
        }
        
        public void RegistrationSale()
        {
            
                using (SalesInfoDBEntities salesInfoDb = new SalesInfoDBEntities())
                {
                   
                        FileContents contents = _fileName.ParseFile(_path);
                        Manager manager = salesInfoDb.Managers.FirstOrDefault(x => x.Name == contents.ManagerName);

                        if (manager == null)
                        {

                            manager = new Manager()
                            {
                                Name = contents.ManagerName
                            };
                            salesInfoDb.Managers.Add(manager);
                            salesInfoDb.SaveChanges();
                        }

                        foreach (SaleInfo infoSale in contents.SalesInfo)
                        {
                            Client client = salesInfoDb.Clients.FirstOrDefault(x => x.Name == infoSale.ClientName);

                            if (client == null)
                            {
                                client = new Client { Name = infoSale.ClientName };
                                salesInfoDb.Clients.Add(client);
                                salesInfoDb.SaveChanges();
                            }

                            Order order = new Order
                            {
                                ClientID = client.ID,
                                ManagerID = manager.ID,
                                OrderDate = infoSale.DateSale,
                                ProductName = infoSale.ProductName,
                                Cost = infoSale.CostSale
                            };

                            salesInfoDb.Orders.Add(order);
                            salesInfoDb.SaveChanges();
                        }
                    } 
            MoveFile();
        }

        private void MoveFile()
        {
            string sourceFile = Path.Combine(_path, _fileName);
            string destinationFile = Path.Combine(ViewedDirictory, _fileName);

            if (!Directory.Exists(ViewedDirictory))
            {
                Directory.CreateDirectory(ViewedDirictory);
            }

            File.Move(sourceFile, destinationFile);
        }
    }
}
