using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlobLab.Backend.Features.DownloadEncriptedFile
{
    public class UseCase : IRequestHandler<UseCaseInput, UseCaseOutput>
    {
        private readonly IDataAccess _downloadEncriptedFileDataAccess;

        public UseCase(IDataAccess downloadEncriptedFileDataAccess)
        {
            _downloadEncriptedFileDataAccess = downloadEncriptedFileDataAccess;
        }

        public async Task<UseCaseOutput> Handle(UseCaseInput request, CancellationToken cancellationToken)
        {
            var output = await _downloadEncriptedFileDataAccess.GetFilePath(request.Id);
            return output;
        }
    }
}
