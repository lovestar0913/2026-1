using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject playerPrefab;
    private GameObject playerObj;

    public GameObject gameOverUI;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Play")
        {
            // 如果玩家已經存在，就不生成新的
            if (playerObj == null)
            {
                if (playerPrefab == null)
                {
                    Debug.LogError("Player Prefab 沒有指定！");
                    return;
                }

                playerObj = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                playerObj.tag = "Player";
                DontDestroyOnLoad(playerObj);
            }

            // 延遲綁定 DamageFlash UI
            StartCoroutine(BindDamageFlashCoroutine());
        }
        else if (scene.name == "Chose")
        {
            // 回到選擇場景才刪除玩家
            if (playerObj != null)
            {
                Destroy(playerObj);
                playerObj = null;
            }
        }
    }

    private IEnumerator BindDamageFlashCoroutine()
    {
        float timer = 0f;
        DamageFlash df = null;

        while (timer < 1f)
        {
            df = Object.FindFirstObjectByType<DamageFlash>();
            if (df != null)
                break;

            timer += Time.deltaTime;
            yield return null;
        }

        if (df != null)
        {
            if (playerObj != null)
            {
                PlayerController pc = playerObj.GetComponent<PlayerController>();
                if (pc != null)
                    pc.damageFlash = df;
            }
        }
        else
        {
            Debug.LogWarning("Play 場景裡找不到 DamageFlash UI！");
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverUI != null)
            gameOverUI.SetActive(true);
        Debug.Log("遊戲結束");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Chose");
    }
}
