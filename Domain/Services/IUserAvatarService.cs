using System.IO;
using Domain.Entities;

namespace Domain.Services
{
    public interface IUserAvatarService
    {
        bool SetUserAvatarFromFile(User yser, Stream srcfile, string fulldestpath, string urlbase);
        bool SetUserAvatarFromFile(User user, string srcfile, string fulldestpath, string urlbase);
        void RemoveAvatar(User user, string avatarfolder);
    }
}
