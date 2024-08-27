namespace Client
{
    public class FrameAnimator
    {
        private float frameTime = .1f;
        private float time = 0f;
        private int frames = 4;
        private int currentFrame;

        public int CurrentFrame => currentFrame;


        public FrameAnimator(int framesSetup)
        {
            frames = framesSetup;
        }

        public void Update(float time)
        {
            this.time -= time;

            if (this.time <= 0f)
            {
                currentFrame = GetNextFrame();
                this.time += frameTime;
            }
        }

        private int GetNextFrame()
        {
            if (currentFrame == frames - 1)
                return 0;

            return currentFrame + 1;
        }
    }
}