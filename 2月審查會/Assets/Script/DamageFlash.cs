using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    private Image image;
    public float flashDuration = 0.2f;

    // 設定閃紅顏色（0~1 範圍）
    private Color flashColor = new Color(255f / 255f, 225f / 255f, 225f / 255f, 60f / 255f);

    void Awake()
    {
        image = GetComponent<Image>();
        if (image != null)
        {
            // 一開始完全透明
            image.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
        }
    }

    public void Flash()
    {
        if (image != null)
            StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // 顯示閃紅顏色
        image.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        // 重新設成透明
        image.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
    }
}
