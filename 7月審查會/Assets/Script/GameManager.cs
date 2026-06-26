using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public AudioSource musicSource;

    private void Awake()
    {
        Instance = this;
    }

    public float MusicTime
    {
        get
        {
            return musicSource.time;
        }
    }

    private void Start()
    {
        musicSource.Play();
    }
}