namespace dotnet_rampup.Models
{
    public class TodoCreate
    {
        public string? Name { get; set; }
        public bool IsComplete { get; set; } = false;
    }
}
