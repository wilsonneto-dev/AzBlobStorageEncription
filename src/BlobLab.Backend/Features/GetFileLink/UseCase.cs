using BlobLab.Backend.Shared;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlobLab.Backend.Features.GetFileLink
{
    public class UseCase : IRequestHandler<UseCaseInput, string>
    {
        private readonly IDataAccess _getFileLinkDataAccess;
        private readonly IStorageService _storageService;

        public UseCase(IDataAccess getFileLinkDataAccess, IStorageService storageService) 
        {
            _getFileLinkDataAccess = getFileLinkDataAccess;
            _storageService = storageService;
        }

        public async Task<string> Handle(UseCaseInput request, CancellationToken cancellationToken)
        {
            string blobPath = await _getFileLinkDataAccess.GetFilePath(request.Id);
            string publicLink = _storageService.GetFileLink(blobPath);
            return publicLink;
        }
    }
}
