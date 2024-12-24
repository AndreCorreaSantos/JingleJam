using UnityEngine;
using UnityEngine.UI;

public class MultiplierBar : MonoBehaviour
{
    [Header("Bar References")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Image backgroundImage; 
    
    [Header("Colors")]
    [SerializeField] private Color tier1Color = new Color(1f, 0f, 0f, 1f);   
    [SerializeField] private Color tier2Color = new Color(0f, 0f, 1f, 1f);  
    [SerializeField] private Color tier3Color = new Color(0.54f, 0.17f, 0.89f, 1f); 
    [SerializeField] private Color tier4Color = new Color(1f, 0.84f, 0f, 1f); 
    
    private void Start()
    {
        MatchManager.Instance.OnMultiplierChanged += UpdateMultiplierBar;
        
        UpdateMultiplierBar(MatchManager.Instance.GetCurrentMultiplier());
    }

    private void UpdateMultiplierBar(float multiplier)
    {
        int tier = Mathf.FloorToInt(multiplier);
        float progressInTier = multiplier - tier;
        
        Color targetColor = tier switch
        {
            1 => tier1Color,
            2 => tier2Color,
            3 => tier3Color,
            4 => tier4Color,
            _ => tier1Color
        };

        if (fillImage != null)
        {
            fillImage.color = targetColor;
        }

        if (tier == 4)
        {
            progressInTier = 1;
        }
            
        fillImage.fillAmount = progressInTier;
    }

    private void OnDestroy()
    {
        if (MatchManager.Instance != null)
        {
            MatchManager.Instance.OnMultiplierChanged -= UpdateMultiplierBar;
        }
    }
}