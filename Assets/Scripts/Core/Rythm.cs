using System;
using System.Collections;

namespace Core
{
    public interface IRythm
    {
        float Cadence { get; }
        Action OnStep { get; set; }
        void Update(float time, int participants);
        IEnumerator WaitEndOfStep();
    }

    public class Rythm :IRythm
    {
        private RythmConfig config;
        private float cadence = .5f;
        private float stepCooldown;
        private Action step;

        public float Cadence => cadence;
        public Action OnStep { get => step; set => step = value; }


        public Rythm(RythmConfig configSetup)
        {
            config = configSetup;
        }

        public void Update(float time, int participants)
        {
            stepCooldown -= time;

            if (stepCooldown <= 0f)
            {
                step?.Invoke();
                cadence = GetCadence(participants);
                stepCooldown += cadence;
            }
        }

        private float GetCadence(int participants)
        {
            return config.CadenceCurve.Evaluate(participants);
        }
        
        public IEnumerator WaitEndOfStep()
        {
            yield return stepCooldown * .5f;
        }
    }
}