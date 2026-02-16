using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [Header("血條")]
    public Image fillImage; // UI Image，用於填充血量

    /// <summary>
    /// 初始化血量
    /// </summary>
    public void Initialize(float currentHealth, float maxHealth)
    {
        UpdateStats(currentHealth, maxHealth);
    }

    /// <summary>
    /// 更新血量顯示
    /// </summary>
    public void UpdateStats(float currentHealth, float maxHealth)
    {
        if (fillImage != null)
            fillImage.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
    }
}
