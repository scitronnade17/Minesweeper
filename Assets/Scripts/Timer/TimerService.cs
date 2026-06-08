using System;
using UnityEngine;
using Zenject;

public interface ITimerService
{
    float ElapsedSeconds { get; }
    bool IsRunning { get; }

    event Action<float> OnTick;

    void StartTimer();
    void Pause();
    void Resume();
    void Stop();
    void Reset();
}

public class TimerService : ITimerService, ITickable
{
    public float ElapsedSeconds { get; private set; }
    public bool IsRunning { get; private set; }

    public event Action<float> OnTick;

    private float buffer;

    public void Tick()
    {
        if (!IsRunning) return;

        buffer += Time.deltaTime;

        if (buffer < 1f) return;

        buffer -= 1f;
        ElapsedSeconds += 1f;
        OnTick?.Invoke(ElapsedSeconds);
    }

    public void StartTimer()
    {
        if (IsRunning) return;
        IsRunning = true;
    }

    public void Pause()
    {
        IsRunning = false;
    }

    public void Resume()
    {
        if (IsRunning) return;
        IsRunning = true;
    }

    public void Stop()
    {
        IsRunning = false;
    }

    public void Reset()
    {
        IsRunning = false;
        ElapsedSeconds = 0f;
        buffer = 0f;
    }
}