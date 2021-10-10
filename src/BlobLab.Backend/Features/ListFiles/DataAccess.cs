using BlobLab.Backend.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlobLab.Backend.Features.ListFiles
{
    public interface IDataAccess
    {
        Task<IEnumerable<ListItemDTO>> ListFiles();
    }

    public class DataAccess : IDataAccess
    {
        private readonly BlobLabDbContext _blobLabDbContext;

        public DataAccess(BlobLabDbContext blobLabDbContext)
        {
            _blobLabDbContext = blobLabDbContext;
        }

        public async Task<IEnumerable<ListItemDTO>> ListFiles()
        {
            var list = await _blobLabDbContext.Files
                .Select(file => new ListItemDTO { 
                    Id = file.Id, 
                    Extension = file.Extension, 
                    Name = file.Name, 
                    Security = file.SecurityLevel.ToString() 
                })
                .OrderByDescending(file => file.Id)
                .ToListAsync();
            return list;
        }
    }
}
