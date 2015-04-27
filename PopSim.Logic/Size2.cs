namespace PopSim.Logic
{
    public class Size2
    {
        public Size2(double width, double height)
        {
            Width = width;
            Height = height;
        }
        public double Width { get; private set; }
        public double Height { get; private set; }
    }
}