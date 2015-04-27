using System;

namespace PopSim.Logic
{
    public class Vector2
    {
        public Vector2(double x, double y)
        {
            X = x;
            Y = y;

            IsZero = Math.Abs(x) < double.Epsilon && Math.Abs(y) < double.Epsilon;
        }

        public bool IsZero { get; private set; }

        public double X { get; private set; }
        public double Y { get; private set; }

        public Vector2 Add(Vector2 vector2)
        {
            return new Vector2(X + vector2.X,Y + vector2.Y);
        }

        public double GetDistance(Vector2 vector2)
        {
            return Math.Sqrt(Squared(vector2.X - X) + Squared(vector2.Y - Y));
        }

        private static double Squared(double value)
        {
            return Math.Pow(value, 2);
        }
        
        public double AngleBetween(Vector2 vector2)
        {
            return Math.Atan2(vector2.Y - Y, vector2.X - X);
        }

        public Vector2 GetVelocity(Vector2 destination, double speed)
        {
            var angle = AngleBetween(destination);
            return new Vector2(Math.Cos(angle) * speed,Math.Sin(angle) * speed);

        }
    }
}