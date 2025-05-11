


namespace Game.Models
{

    public enum AnimationType
    {
        // characters will not load the first idle animation without dummy
        Dummy,
        Idle,
        Move,
        Attack,
        Death
    }

    public class AnimationInfo
    {

        public int SpriteRow;
        public int FrameCount;
        public float Interval;


        public AnimationInfo(int spriteRow, int frameCount, float interval)
        {
            SpriteRow = spriteRow;
            FrameCount = frameCount;
            Interval = interval;
        }




    }
}
