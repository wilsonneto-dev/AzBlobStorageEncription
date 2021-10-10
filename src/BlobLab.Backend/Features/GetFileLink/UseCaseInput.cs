using MediatR;

namespace BlobLab.Backend.Features.GetFileLink
{
    public class UseCaseInput : IRequest<string>
    {
        public int Id {  get; set; }
    }
}
