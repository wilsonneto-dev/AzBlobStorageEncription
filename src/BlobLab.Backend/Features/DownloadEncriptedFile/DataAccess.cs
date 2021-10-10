using BlobLab.Backend.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BlobLab.Backend.Features.DownloadEncriptedFile
{
    public interface IDataAccess
    {
        Task<UseCaseOutput> GetFilePath(int id);
    }

    public class DataAccess : IDataAccess
    {
        private readonly BlobLabDbContext _blobLabDbContext;

        public DataAccess(BlobLabDbContext blobLabDbContext)
        {
            _blobLabDbContext = blobLabDbContext;
        }

        public async Task<UseCaseOutput> GetFilePath(int id)
        {
            var output = await _blobLabDbContext.Files
                .Where(f => f.Id == id)
                .Select(f => new UseCaseOutput { Path = f.Path, FileName = f.Name })
                .SingleOrDefaultAsync();
            return output;
        }
    }
}
