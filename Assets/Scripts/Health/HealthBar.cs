using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    private float healthPercent = 0.0f; // between 0 and 1
    
    public int healthValue;
    public Transform foregroundSprite;
    public SpriteRenderer ForegroundRenderer;
    public Color MaxHealthColour = new Color(255 / 255, 63 / 255f, 63 / 255f);
    public Color MinHealthColour = new Color(64 / 255f, 137 / 255f, 255 / 255f);

    

    //Consider refactoring into 1 method to call when damage is taken.
    public void UpdateBar(int health, int maxHealth)
    {
        healthPercent = health / maxHealth;
        foregroundSprite.localScale = new Vector3(healthPercent, 1, 1);
        ForegroundRenderer.color = Color.Lerp(MaxHealthColour, MinHealthColour, healthPercent);
    }
}