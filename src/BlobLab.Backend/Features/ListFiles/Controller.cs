using BlobLab.Backend.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BlobLab.Backend.Features.ListFiles
{
    [Route("files")]
    public class Controller : ControllerBase
    {
        [ProducesResponseType(typeof(IEnumerable<ListItemDTO>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> Handle([FromServices] IMediator _mediator)
        {
            var list = await _mediator.Send(new UseCaseInput());
            return Ok(list);
        }
    }
}
