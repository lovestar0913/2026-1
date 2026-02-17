using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    public Image flashImage;
    public float flashDuration = 0.2f;

    private Coroutine flashRoutine;

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        flashImage.color = new Color(1, 0, 0, 0.6f); // ¥b³z©ú¬õ
        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            flashImage.color = new Color(1, 0, 0, Mathf.Lerp(0.6f, 0f, elapsed / flashDuration));
            yield return null;
        }

        flashImage.color = new Color(1, 0, 0, 0f);
    }
}
