using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
 
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;

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
    private float mockloading = 2f;
    private float waitingToStartTimer = 3f;
    private float mockPlaying = 30f;

    void Awake()
    {
        Instance = this; 
    }

    private void Start()
    {
        state = State.Loading;
    }

    // Update is called once per frame
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
                if(waitingToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                mockPlaying -= Time.deltaTime;
                if(mockPlaying < 0f)
                {
                    state = State.GameWin;
                    mockPlaying = 10f;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

        }

        Debug.Log(state);
        
    }

    public bool IsCountDownToSTartActive() { return state == State.CountDownStart; }
    public bool IsGamePlaying() { return state == State.GamePlaying; }

    public float GetCountDownToStartTimer()
    {
        return waitingToStartTimer;
    }
}
