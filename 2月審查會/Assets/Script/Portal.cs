using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string targetSceneName = "Play";

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player == null) return;

        // ª½±µ¤Á´«³õ´º
        SceneManager.LoadScene(targetSceneName);
    }
}
