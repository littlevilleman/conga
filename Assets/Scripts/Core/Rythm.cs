using System;

public class Rythm
{
    private float stepTime = .5f;
    private float stepCooldown;

    public Action step;

    public void Update(float time)
    {
        stepCooldown -= time;

        if (stepCooldown <= 0f)
        {
            step?.Invoke();
            stepCooldown += stepTime;
        }
    }
}