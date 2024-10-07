namespace PersonalOffice.Backend.Application.CQRS.File.Queries
{
    internal class FileDataDto
    {
        public string? Name { get; set; }
        public required string FilePath { get; set; }
    }
}
