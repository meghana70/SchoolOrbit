using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Web;
using System.Configuration;
namespace DomainModel.BusinessLayer
{
  public class BlobStorage
    {

      public static String BlobStorageURL
      {
          get
          {
              String StrURL =  ConfigurationManager.AppSettings["BlobStorageURL"];
              return StrURL;
          }
      }
        public enum StorageContainer
        {
            finance = 100, Avatar = 90, Employee = 80
        };

       public Boolean  SaveBlob(int BlobType,String filename, string filefullpath){
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        String Strcontainer = GetContainer(BlobType);
        CloudBlobContainer container = blobClient.GetContainerReference(Strcontainer);
        CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
        using (var fileStream = System.IO.File.OpenRead(filefullpath))
        {
            blockBlob.UploadFromStream(fileStream);
        }
        return false;
       }

       string GetContainer(int Blobtype)
       {
           String container = string.Empty;
           switch (Blobtype) {
               case (int)StorageContainer.finance :
                   container = "finance";
                   break;
               case (int)StorageContainer.Employee:
                   container = "employee";
                   break;
           }
           return container;
       }



    }
}
