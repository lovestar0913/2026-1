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


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Play();
    }

    // Play Control
    public void Play()
    {
        if (musicSource == null)
            return;

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
    }

    public void Restart()
    {
        if (musicSource == null)
            return;

        musicSource.Stop();
        musicSource.time = 0f;
        musicSource.Play();
    }

    public void Seek(float time)
    {
        if (musicSource == null)
            return;

        musicSource.time = Mathf.Max(0f, time);
    }

    public void SetVolume(float volume)
    {
        if (musicSource == null)
            return;

        musicSource.volume =
            Mathf.Clamp01(volume);
    }

    // Rhythm Time
    public float MusicTime
    {
        get
        {
            if (musicSource == null)
                return 0f;


            return musicSource.time + musicOffset;
        }
    }

    // 小節時間
    public float BeatTime
    {
        get
        {
            if (bpm <= 0)
                return 0;


            return 60f / bpm;
        }
    }

    // 目前第幾拍
    public float CurrentBeat
    {
        get
        {
            return MusicTime / BeatTime;
        }
    }

    // State
    public bool IsPlaying
    {
        get
        {
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