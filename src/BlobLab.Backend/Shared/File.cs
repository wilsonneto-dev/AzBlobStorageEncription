namespace BlobLab.Backend.Shared
{
    public enum SecurityLevel
    {
        Private,
        Encripted
    }

    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public SecurityLevel SecurityLevel {  get; set; }
    }
}
