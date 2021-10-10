using BlobLab.Backend.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlobLab.Backend.Features.DownloadEncriptedFile
{
    [Route("files")]
    public class Controller : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task Handle(
            [FromRoute] UseCaseInput input,
            [FromServices] IMediator _mediator,
            [FromServices] IStorageService _storageService
        )
        {
            UseCaseOutput fileInfo = await _mediator.Send(input);
            if (fileInfo == null)
            {
                Response.StatusCode = 404;
                return;
            }

            DownloadInfoDTO downloadInfo = await _storageService.DownloadEncriptedFile(fileInfo.Path);

            Response.Headers.Add("Content-Disposition", "Attachment;filename=" + fileInfo.FileName);
            Response.Body.Write(downloadInfo.MemoryStream.ToArray());
        }
    }
}
