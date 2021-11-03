using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    private static GameObject TimerCollector;

    public event UnityAction OnTimerRanOut;
    public event UnityAction OnTick;

    public bool IsLoop;

    public float MaxTime => maxTime;
    public float CurrentTime => currentTime;
    public bool IsPause => isPause;
    public bool IsCompleted => currentTime <= 0;

    private float maxTime;
    private float currentTime;
    private bool isPause;

    private void Update()
    {
        if (isPause == true) return;

        currentTime -= Time.deltaTime;

        if (OnTick != null) OnTick.Invoke();


        if(currentTime <= 0)
        {
            currentTime = 0;

            if (OnTimerRanOut != null) OnTimerRanOut.Invoke();

            if (IsLoop == true)
            {
                currentTime = maxTime;
            }
        }
    }

    public static Timer CreateTimer(float time, bool isLoop)
    {
        if (TimerCollector == null)
        {
            TimerCollector = new GameObject("Timers");
        }

        Timer timer = TimerCollector.AddComponent<Timer>();

        timer.maxTime = time;
        timer.IsLoop = isLoop;

        return timer;
    }
    public static Timer CreateTimer(float time)
    {
        if (TimerCollector == null)
        {
            TimerCollector = new GameObject("Timers");
        }

        Timer timer = TimerCollector.AddComponent<Timer>();

        timer.maxTime = time;

        return timer;
    }

    public void Play()
    {
        isPause = false;
    }

    public void Pause()
    {
        isPause = true;
    }

    public void Completed()
    {
        isPause = false;

        currentTime = 0;
    }

    public void CompletedWithoutEvent()
    {
        Destroy(this);
    }

    public void Restart(float time)
    {
        maxTime = time;
        currentTime = maxTime;
    }

    public void Restart()
    {
        currentTime = maxTime;
    }



}
