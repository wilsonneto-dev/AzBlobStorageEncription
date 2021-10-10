using BlobLab.Backend.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BlobLab.Backend.Features.GetFileLink
{
    public interface IDataAccess
    {
        Task<string> GetFilePath(int id);
    }

    public class DataAccess : IDataAccess
    {
        private readonly BlobLabDbContext _blobLabDbContext;

        public DataAccess(BlobLabDbContext blobLabDbContext)
        {
            _blobLabDbContext = blobLabDbContext;
        }

        public async Task<string> GetFilePath(int id)
        {
            var path = await _blobLabDbContext.Files.Where(f => f.Id == id).Select(f => f.Path).SingleOrDefaultAsync();
            return path;
        }
    }
}
