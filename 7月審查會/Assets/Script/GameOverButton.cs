using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButton : MonoBehaviour
{
    public void Main()
    {
        // 清除上一首歌的資料
        GameManager.Instance.ResetGame();

        // 回 Main 場景
        SceneManager.LoadScene("Main");
    }
}