using System.Numerics;


namespace Game.Models
{
    public class PhysicsBody
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Size { get; set; }
        public float Gravity = 800f;
        public float Mass { get; set; }
        public bool IsOnGround { get; set; }


        public void Update(float deltaTime)
        {


            Position += Velocity * deltaTime;
            float groundY = 500;


            // Gravity check
            if (Mass > 0)
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * deltaTime);
            }

            if (Position.Y + Size / 2 >= groundY)
            {
                Position = new Vector2(Position.X, groundY - Size / 2);
                Velocity = new Vector2(Velocity.X, 0);
                IsOnGround = true;

            }
            else
            {
                IsOnGround = false;
            }

            if (Position.X - Size / 2 < 0)
            {
                Position = new Vector2(Size / 2, Position.Y);
                Velocity = new Vector2(0, Velocity.Y);
            }


        }


    }


}
