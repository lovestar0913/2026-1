using UnityEngine;
using System.Collections;

public class AttackHint : MonoBehaviour
{
    public static IEnumerator ShowHint(GameObject hintPrefab, Vector3 position, float duration)
    {
        GameObject hint = Instantiate(hintPrefab, position, Quaternion.identity);
        SpriteRenderer sr = hint.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 0f;
            sr.color = c;

            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                c.a = Mathf.PingPong(t * 2f, 0.7f);
                sr.color = c;
                yield return null;
            }
        }
        Destroy(hint);
    }
}
