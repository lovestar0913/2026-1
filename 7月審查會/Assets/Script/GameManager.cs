using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public AudioSource musicSource;

    private void Awake()
    {
        Instance = this;
    }

    public float GetMusicTime()
    {
        return musicSource.time;
    }
}