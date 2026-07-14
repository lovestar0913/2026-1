using UnityEngine;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;

    [Header("Audio")]
    public AudioSource musicSource;

    [Header("Offset")]
    public float musicOffset = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

    // 播放
    public void Play()
    {
        if (musicSource == null)
            return;

        musicSource.Play();
    }

    // 暫停
    public void Pause()
    {
        if (musicSource == null)
            return;

        musicSource.Pause();
    }

    // 停止
    public void Stop()
    {
        if (musicSource == null)
            return;

        musicSource.Stop();
    }

    // 重播
    public void Restart()
    {
        if (musicSource == null)
            return;

        musicSource.Stop();
        musicSource.time = 0f;
        musicSource.Play();
    }

    // 跳到指定時間
    public void Seek(float time)
    {
        if (musicSource == null)
            return;

        musicSource.time = Mathf.Max(0f, time);
    }

    // 音量
    public void SetVolume(float volume)
    {
        if (musicSource == null)
            return;

        musicSource.volume = Mathf.Clamp01(volume);
    }

    // 目前播放時間
    public float MusicTime
    {
        get
        {
            if (musicSource == null)
                return 0f;

            return musicSource.time + musicOffset;
        }
    }

    // 是否播放中
    public bool IsPlaying
    {
        get
        {
            if (musicSource == null)
                return false;

            return musicSource.isPlaying;
        }
    }

    // 音樂長度
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