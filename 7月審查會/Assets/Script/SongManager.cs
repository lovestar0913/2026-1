using System.Collections;
using UnityEngine;


public class SongManager : MonoBehaviour
{
    public static SongManager Instance;


    [Header("Audio")]
    public AudioSource musicSource;


    [Header("Chart")]
    public ChartData chartData;


    [Header("Song Info")]
    public float bpm = 180f;


    [Header("Offset")]
    public float musicOffset = 0f;


    [Header("Start Delay")]
    public float startDelay = 3f;



    public bool Started { get; private set; }


    private Coroutine startCoroutine;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }



    private void Start()
    {
        ResetSong();
    }



    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }



    //=========================
    // Reset Song
    //=========================

    public void ResetSong()
    {
        if (startCoroutine != null)
        {
            StopCoroutine(startCoroutine);
        }


        Stop();


        Started = false;



        //重新取得Chart
        if (ChartLoader.Instance != null)
        {
            chartData =
                ChartLoader.Instance.GetChart();


            if (chartData != null)
            {
                bpm =
                    chartData.bpm;

                musicOffset =
                    chartData.offset;
            }
        }



        startCoroutine =
            StartCoroutine(StartGame());
    }



    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(startDelay);


        Started = true;


        Play();
    }



    //=========================
    // Audio Control
    //=========================


    public void Play()
    {
        if (musicSource == null)
        {
            Debug.LogError("沒有 AudioSource");
            return;
        }


        musicSource.Stop();

        musicSource.time = 0f;

        musicSource.Play();
    }



    public void Pause()
    {
        if (musicSource == null)
            return;


        musicSource.Pause();
    }



    public void Stop()
    {
        if (musicSource == null)
            return;


        musicSource.Stop();

        musicSource.time = 0f;
    }



    public void Restart()
    {
        ResetSong();
    }



    public void Seek(float time)
    {
        if (musicSource == null)
            return;


        musicSource.time =
            Mathf.Max(0f, time);
    }



    public void SetVolume(float volume)
    {
        if (musicSource == null)
            return;


        musicSource.volume =
            Mathf.Clamp01(volume);
    }



    //=========================
    // Rhythm Time
    //=========================


    public float MusicTime
    {
        get
        {
            if (!Started)
                return 0f;


            if (musicSource == null)
                return 0f;


            return musicSource.time + musicOffset;
        }
    }



    public float BeatTime
    {
        get
        {
            if (bpm <= 0)
                return 0f;


            return 60f / bpm;
        }
    }



    public float CurrentBeat
    {
        get
        {
            if (!Started)
                return 0f;


            return MusicTime / BeatTime;
        }
    }



    public bool IsPlaying
    {
        get
        {
            if (!Started)
                return false;


            if (musicSource == null)
                return false;


            return musicSource.isPlaying;
        }
    }



    public float Length
    {
        get
        {
            if (musicSource == null)
                return 0f;


            if (musicSource.clip == null)
                return 0f;


            return musicSource.clip.length;
        }
    }
}