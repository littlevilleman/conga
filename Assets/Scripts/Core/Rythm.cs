using System;

namespace Core
{
    public interface IRythm
    {
        float Cadence { get; }
        Action OnStep { get; set; }
        void Update(float time);
    }

    public class Rythm :IRythm
    {
        private float cadence = .25f;
        private float stepCooldown;
        private Action step;

        public float Cadence => cadence;
        public Action OnStep { get => step; set => step = value; }


        public void Update(float time)
        {
            stepCooldown -= time;

            if (stepCooldown <= 0f)
            {
                step?.Invoke();
                stepCooldown += cadence;
            }
        }
    }
}