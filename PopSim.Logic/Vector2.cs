namespace PopSim.Logic
{
    public class Vector2
    {
        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }
        public double X { get; private set; }
        public double Y { get; private set; }

        public Vector2 Add(Vector2 vector2)
        {
            return new Vector2(X + vector2.X,Y + vector2.Y);
        }
    }
}