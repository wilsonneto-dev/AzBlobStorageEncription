using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BlobLab.Backend.Features.ListFiles
{
    public class UseCase : IRequestHandler<UseCaseInput, IEnumerable<ListItemDTO>>
    {
        private readonly IDataAccess _listFilesDataAccess;

        public UseCase(IDataAccess listFilesDataAccess)
        {
            _listFilesDataAccess = listFilesDataAccess;
        }

        public async Task<IEnumerable<ListItemDTO>> Handle(UseCaseInput request, CancellationToken cancellationToken)
        {
            var list = await _listFilesDataAccess.ListFiles();
            return list;
        }
    }
}
