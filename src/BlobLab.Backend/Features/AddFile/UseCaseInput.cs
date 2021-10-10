using MediatR;

namespace BlobLab.Backend.Features.AddFile
{
    public class UseCaseInput : IRequest<int>
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public  bool Encripted { get; set; }
    }
}
