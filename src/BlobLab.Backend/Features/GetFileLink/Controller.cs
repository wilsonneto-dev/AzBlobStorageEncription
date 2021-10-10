using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlobLab.Backend.Features.GetFileLink
{
    [Route("files")]
    public class Controller : ControllerBase
    {
        public class GetFileLinkViewModel
        {
            public GetFileLinkViewModel(string link)
            {
                Link = link;
            }

            public string Link { get; set; }
        }

        [ProducesResponseType(typeof(GetFileLinkViewModel), StatusCodes.Status200OK)]
        [HttpGet("{id}/link")]
        public async Task<IActionResult> Handle(
            [FromServices] IMediator _mediator, 
            [FromRoute] int id
        ) {
            UseCaseInput input = new UseCaseInput { Id = id };
            string link = await _mediator.Send(input);
            return Ok(new GetFileLinkViewModel(link));
        }

    }
}
