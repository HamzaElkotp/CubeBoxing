using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
 
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;

    [SerializeField] private AudioSource audiossSource;

    private enum State
    {
        Lobby,
        Loading,
        CountDownStart,
        GamePlaying,
        GameOver,
        GameWin
    }

    private State state;
    private float mockloading = 0f;
    private float waitingToStartTimer = 3f;

    void Awake()
    {
        Instance = this; 
    }

    private void Start()
    {
        state = State.Loading;
        if (!audiossSource.isPlaying) audiossSource.Play();
    }

    void Update()
    {
        switch (state)
        {
            case State.Loading:
                mockloading -= Time.deltaTime;
                if(mockloading < 0f)
                {
                    state = State.CountDownStart;
                    mockloading = 2f;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountDownStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                if (audiossSource.isPlaying) audiossSource.Stop();
                break;
            case State.GameOver:
                if (!audiossSource.isPlaying) audiossSource.Play();
                break;
            case State.GameWin:
                if (!audiossSource.isPlaying) audiossSource.Play();
                break;
        }

        Debug.Log(state);
        
    }

    public bool IsCountDownToSTartActive() { return state == State.CountDownStart; }
    public bool IsGamePlaying()             { return state == State.GamePlaying; }
    public bool IsGameOver()                { return state == State.GameOver; }
    public bool IsGameWin()                 { return state == State.GameWin; }

    public void SetGameOver()
    {
        if (state == State.GamePlaying)
        {
            state = State.GameOver;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetGameWin()
    {
        if (state == State.GamePlaying)
        {
            state = State.GameWin;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public float GetCountDownToStartTimer()
    {
        return waitingToStartTimer;
    }
}
