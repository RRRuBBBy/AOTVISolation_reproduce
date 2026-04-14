namespace AOTVI.Models
{
    public class Defect
    {
        public int Id { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public bool IsNG { get; set; }
    }
}