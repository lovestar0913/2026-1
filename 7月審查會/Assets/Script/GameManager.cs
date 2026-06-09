using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public AudioSource musicSource;

    public float MusicTime
    {
        get
        {
            return musicSource.time;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        musicSource.Play();
    }
}