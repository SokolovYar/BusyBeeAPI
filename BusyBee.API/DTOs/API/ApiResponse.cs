namespace BusyBee.API.DTOs.API
{
    public class ApiResponse<T>
    {
        public required StatusInfo Status { get; set; }
        public required MetaInfo Meta { get; set; }
        public T? Data { get; set; }
    }

    public class StatusInfo
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string? ErrorCode { get; set; } = null;
    }

    public class MetaInfo
    {
        public string Method { get; set; }
        public string Service { get; set; } = "Offers API";
        public string Timestamp { get; set; } = DateTime.UtcNow.ToString("o");
        public string? RequestId { get; set; }
        public PaginationInfo? Pagination { get; set; }

        public MetaInfo(HttpContext context)
        {
            Method = context.Request.Method;
            RequestId = context.TraceIdentifier;
        }
    }

    public class PaginationInfo
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
