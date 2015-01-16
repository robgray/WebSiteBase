using System.IO;
using System.Web;

namespace WebBase.Mvc.Services
{
    public interface ImagePersistService
    {
        string Persist(HttpPostedFileBase file);
    }

    public class FileSystemImagePersistService : ImagePersistService
    {
        public string Persist(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);
            return "";
        }
    }
}