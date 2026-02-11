using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : HealthUI // base below
{
    [Header("IMAGE that fills based on health. (This should be set in prefab)")]
    public Image healthBarFill;

    [Header("Should text show health too? Add TEXT object here.")]
    [Tooltip("There is one in this prefab, drag it here and turn it on.")]
    public Text healthText;
    string defaultText = "";

    [Header("Does the health bar change to red at low health?")]
    public bool colorChange = false;
    [Tooltip("Percent(0-100) health will be when color changes to red")]
    public int percentHealthLow = 25;
    Color32 defaultColor;

    [Header("Can you see the bar change, or will it snap to values?")]
    public bool slowBar = false;
    [Tooltip("Speed the bar will fill/shrink")]
    public float fillSpeed = 6F;
    bool fillPos = false, fillNeg = false;
    float slowFill;
    int updatedCurrentHealth;

    public override void setHealth(int max)
    {
        base.setHealth(max);

        if (!healthBarFill)
        {
            Debug.LogError("healthBarFill not found! Disabling health bar...");
            FindObjectOfType<PlayerHealth>().healthUI = null;
            gameObject.SetActive(false);
        }
        else
        {
            healthBarFill.fillAmount = 1;
            if (colorChange)
            {
                if (healthBarFill.color == new Color32(226, 58, 31, 255)) healthBarFill.color = defaultColor;
                else defaultColor = healthBarFill.color;
            }
            if (healthText)
            {
                healthText.gameObject.SetActive(true);
                if(defaultText == "") defaultText = healthText.text;
                healthText.text = defaultText + maxHealth + "/" + maxHealth;
            }
            if(slowBar && fillSpeed < 0)
            {
                Debug.LogWarning("FillSpeed cannot be less than 0! Defaulting to 6...", gameObject);
                fillSpeed = 6F;
            }
        }
    }

    public override void updateHealth(int newHealth)
    {
        if (newHealth < 0) newHealth = 0;
        else if (newHealth > maxHealth) newHealth = maxHealth;
        else if (newHealth == currentHealth) return;

        if(slowBar)
        {
            updatedCurrentHealth = newHealth;
            slowFill = currentHealth;
            if (updatedCurrentHealth < slowFill) fillNeg = true; // decreasing health
            else fillPos = true;                                 // increasing health
        }
        else
        {
            healthBarFill.fillAmount = (float)newHealth / (float)maxHealth;
            if(colorChange)
            {
                float math = (float)newHealth / (float)maxHealth * 100F;
                if (math <= percentHealthLow) healthBarFill.color = new Color32(226, 58, 31, 255);  // red
                else healthBarFill.color = defaultColor;   // set back to normal
            }
        }

        if (healthText)
        {
            healthText.text = defaultText + newHealth + "/" + maxHealth;
        }

        currentHealth = newHealth;
    }

    private void Update()
    {
        if (fillPos)
        {
            slowFill += Time.deltaTime * fillSpeed;
            if (slowFill > updatedCurrentHealth) slowFill = updatedCurrentHealth;
            healthBarFill.fillAmount = slowFill / (float)maxHealth;

            if (colorChange)
            {
                float math = slowFill / (float)maxHealth * 100F;
                if (math > percentHealthLow) healthBarFill.color = defaultColor;
            }

            if (slowFill >= updatedCurrentHealth) fillPos = false;
        }


        if (fillNeg)
        {
            slowFill -= Time.deltaTime * fillSpeed;
            if (slowFill < updatedCurrentHealth) slowFill = updatedCurrentHealth;
            healthBarFill.fillAmount = slowFill / (float)maxHealth;

            if (colorChange)
            {
                float math = slowFill / (float)maxHealth * 100F;
                if (math <= percentHealthLow) healthBarFill.color = new Color32(226, 58, 31, 255);  // red
            }

            if (slowFill <= updatedCurrentHealth) fillNeg = false;
        }
    }
}

public abstract class HealthUI : MonoBehaviour
{
    //[Header("Turn off UI health")]
    //public bool off = false;

    protected int maxHealth;
    protected int currentHealth;

    virtual public void updateHealth(int newHealth)
    {
        // does nothing.
        Debug.LogError("This should not be shown.", gameObject);
    }

    virtual public void setHealth(int max)
    {
        // "Constructor"/ "Start"
        maxHealth = currentHealth = max;

        // set ui components
    }

    public void setHealth(int max, int starting)
    {
        setHealth(max);
        if (starting < max) updateHealth(starting);
    }
}
