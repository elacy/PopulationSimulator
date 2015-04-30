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
        public Vector2(double theta)
        {
            X = Math.Cos(theta); 
            Y = Math.Sin(theta);
            
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
            return new Vector2(vector2.X - X, vector2.Y - Y).VectorMagnitude();
        }
        
        public double AngleBetween(Vector2 vector2)
        {
            return Math.Atan2(vector2.Y - Y, vector2.X - X);
        }

        public Vector2 GetDirection(Vector2 destination)
        {
            var angle = AngleBetween(destination);
            var result = new Vector2(Math.Cos(angle),Math.Sin(angle));
            result = result.UnitVector();
            return result;
        }
        
        public Vector2 ScalarMultiply(double multiplier)
        {
            return new Vector2(X*multiplier,Y*multiplier);
        
        }
        
        public double VectorMagnitude()
        {
            return Math.Sqrt(X*X + Y*Y);
        }
        
        public Vector2 UnitVector()
        {
            var magnitude = VectorMagnitude();
            return new Vector2(X / magnitude, Y / magnitude);
        }
    }
}
