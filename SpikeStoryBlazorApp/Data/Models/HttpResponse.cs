namespace SpikeStoryBlazorApp.Data.Models
{
    public class HttpResponse<T>
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Data { get; set; }
    }
}
