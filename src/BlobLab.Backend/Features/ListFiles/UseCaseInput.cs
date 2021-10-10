using MediatR;
using System.Collections.Generic;

namespace BlobLab.Backend.Features.ListFiles
{
    public class UseCaseInput : IRequest<IEnumerable<ListItemDTO>>
    {
    }
}
