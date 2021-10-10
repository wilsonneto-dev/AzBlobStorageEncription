using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlobLab.Backend.Features.AddFile
{
    [Route("files")]
    public class Controller : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<IActionResult> Handle(
            [FromServices] IMediator _mediator,
            [FromServices] IStorageService _storage,
            [FromForm] IFormFile file,
            [FromForm] bool encript = false
        ) {
            string extension = Path.GetExtension(file.FileName);
            string blobFilePath = Guid.NewGuid().ToString() + extension;
            if(encript)
                await _storage.SaveFileEncripted(blobFilePath, file.OpenReadStream());
            else
                await _storage.SaveFile(blobFilePath, file.OpenReadStream());

            UseCaseInput input = new UseCaseInput { Name = file.FileName, Path = blobFilePath, Extension = extension, Encripted = encript };
            int Id = await _mediator.Send(input);
            return Ok(new { Id });
        }
    }
}
