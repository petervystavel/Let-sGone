using UnityEngine;

public class Timer 
{
    public float Duration;
    float mProgress;

    public Timer(float duration)
    {
        Duration = duration;
        mProgress = -1;
    }

    public bool Update() 
    {
        if (IsRunning() == false)
            return false;

        mProgress += Time.deltaTime;

        if (mProgress < Duration)
            return false;

        Stop();

        return true;
    }

    public bool IsRunning()
    {
        return mProgress >= 0;
    }

    public void Start()
    {
        mProgress = 0;
    }

    public void Stop()
    {
        mProgress = -1;
    }
}
