using Microsoft.AspNetCore.Mvc;

namespace BusyBee.API.DTOs
{
    public record UploadAvatarRequest
    {
        [FromForm]
        public IFormFile File { get; set; }
    }
}
