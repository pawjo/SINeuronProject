namespace SINeuronWPFApp.Models
{
    public class Point
    {
        public double X { get; set; }
        
        public double Y { get; set; }

        public int Value { get; set; }

        public override string ToString()
        {
            return $"({X}, {Y}, {Value})";
        }
    }
}
