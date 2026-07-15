using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public Text scoreText;
    public Text perfectText;
    public Text greatText;
    public Text goodText;
    public Text missText;

    public float digitDelay = 0.08f;
    public float randomSpeed = 0.02f;

    void Start()
    {
        StartCoroutine(ShowResult());
    }

    IEnumerator ShowResult()
    {
        yield return RollNumber(scoreText,
            GameManager.Instance.score,
            6);

        yield return RollNumber(perfectText,
            GameManager.Instance.perfectCount,
            4);

        yield return RollNumber(greatText,
            GameManager.Instance.greatCount,
            4);

        yield return RollNumber(goodText,
            GameManager.Instance.goodCount,
            4);

        yield return RollNumber(missText,
            GameManager.Instance.missCount,
            4);
    }

    IEnumerator RollNumber(Text text, int value, int digits)
    {
        string target =
            value.ToString().PadLeft(digits, '0');

        char[] current =
            new char[digits];

        // 一開始全部亂數
        for (int i = 0; i < digits; i++)
            current[i] = (char)('0' + Random.Range(0, 10));

        text.text = new string(current);

        // 從右邊(個位數)開始固定
        for (int fixedCount = 1; fixedCount <= digits; fixedCount++)
        {
            float timer = 0f;

            while (timer < digitDelay)
            {
                timer += randomSpeed;

                for (int i = 0; i < digits; i++)
                {
                    // 右邊已固定
                    if (i >= digits - fixedCount)
                    {
                        current[i] = target[i];
                    }
                    else
                    {
                        current[i] = (char)('0' + Random.Range(0, 10));
                    }
                }

                text.text = new string(current);

                yield return new WaitForSeconds(randomSpeed);
            }
        }

        text.text = target;
    }
}