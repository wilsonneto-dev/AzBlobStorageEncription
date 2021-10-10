using MediatR;

namespace BlobLab.Backend.Features.DownloadEncriptedFile
{
    public class UseCaseInput : IRequest<UseCaseOutput>
    {
        public int Id { get; set; }
    }
}
