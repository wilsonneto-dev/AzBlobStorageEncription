using BlobLab.Backend.Shared;
using System.Threading.Tasks;

namespace BlobLab.Backend.Features.AddFile
{
    public interface IDataAccess 
    {
        Task<int> AddFile(File file);
    }

    public class DataAccess : IDataAccess
    {
        private readonly BlobLabDbContext _dbContext;

        public DataAccess(BlobLabDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddFile(File file)
        {
            await _dbContext.Files.AddAsync(file);
            await _dbContext.SaveChangesAsync();
            return file.Id;
        }
    }
}
