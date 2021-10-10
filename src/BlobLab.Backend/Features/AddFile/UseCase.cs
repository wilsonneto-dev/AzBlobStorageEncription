using BlobLab.Backend.Shared;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlobLab.Backend.Features.AddFile
{
    public class UseCase : IRequestHandler<UseCaseInput, int>
    {
        private readonly IDataAccess _addFileDataAccess;
        public UseCase(IDataAccess addFileDataAccess)
        {
            _addFileDataAccess = addFileDataAccess;
        }

        public async Task<int> Handle(UseCaseInput request, CancellationToken cancellationToken)
        {
            File file = new File {
                Name = request.Name,
                Extension = request.Extension,
                Path = request.Path,
                SecurityLevel = request.Encripted ? SecurityLevel.Encripted : SecurityLevel.Private
            };
            int savedFileId = await _addFileDataAccess.AddFile(file);
            return savedFileId;
        }
    }
}
