using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // 按下 Start
    public void StartGame()
    {
        SceneManager.LoadScene("Play");
    }

    // 按下 Quit
    public void QuitGame()
    {
        Debug.Log("Quit Game");

        // 在編輯器中停止播放
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 打包後真正關閉遊戲
        Application.Quit();
#endif
    }
}
