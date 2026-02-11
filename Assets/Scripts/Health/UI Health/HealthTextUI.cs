using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthTextUI : HealthUI    // base in HealthBarUI.cs
{
    [Header("TEXT object being edited. (This should be set in prefab)")]
    public Text text;

    [Header("Show percentage instead of health number? (showMax = false)")]
    public bool percentage;

    [Header("Show max health in text?")][Tooltip("true: 1/10 \nfalse: 1")]
    public bool showMax = true;
    string headerText = "";

    [Header("Does the text color change to red at low health?")]
    public bool colorChange = false;
    [Tooltip("Percent(0-100) health will be when color changes to red")]
    public int percentHealthLow = 25;
    Color32 defaultColor;

    [Header("Can you see the numbers, or will it snap to values?")]
    public bool slowUpdate = false;
    [Tooltip("Speed the bar will fill/shrink")]
    public float updateSpeed = 6F;
    bool updatePos = false, updateNeg = false;
    int slowText;
    int updatedCurrentHealth;

    public override void setHealth(int max)
    {
        base.setHealth(max);

        if (!text)
        {
            Debug.LogError("text not found! Disabling health bar...");
            FindObjectOfType<PlayerHealth>().healthUI = null;
            gameObject.SetActive(false);
        }
        else
        {
            if (headerText == "" && !char.IsDigit(text.text[0])) headerText = text.text;

            if (percentage)
            {
                showMax = false;
                text.text = headerText + "100%";
            }
            else
            {
                if (showMax) text.text = headerText + maxHealth + "/" + maxHealth;
                else text.text = headerText + maxHealth;
            }

            if (colorChange)
            {
                if (text.color == new Color32(226, 58, 31, 255)) text.color = defaultColor;
                else defaultColor = text.color;
            }
            if (slowUpdate && updateSpeed < 0)
            {
                Debug.LogWarning("FillSpeed cannot be less than 0! Defaulting to 6...", gameObject);
                updateSpeed = 6F;
            }
        }
    }

    public override void updateHealth(int newHealth)
    {
        if (newHealth < 0) newHealth = 0;
        else if (newHealth > maxHealth) newHealth = maxHealth;
        else if (newHealth == currentHealth) return;

        if (slowUpdate)
        {
            updatedCurrentHealth = newHealth;
            slowText = currentHealth;
            if (updatedCurrentHealth < slowText) updateNeg = true; // decreasing health
            else updatePos = true;                                 // increasing health
        }
        else
        {
            if (percentage)
            {
                int percent = Mathf.RoundToInt((float)newHealth / (float)maxHealth * 100);
                text.text = headerText + percent + "%";
            }
            else
            {
                if (showMax) text.text = headerText + newHealth + "/" + maxHealth;
                else text.text = headerText + newHealth;
            }

            if (colorChange)
            {
                float math = (float)newHealth / (float)maxHealth * 100F;
                if (math <= percentHealthLow) text.color = new Color32(226, 58, 31, 255);  // red
                else text.color = defaultColor;   // set back to normal
            }
        }

        currentHealth = newHealth;
    }

    private void Update()
    {
        if (updatePos)
        {
            slowText += Mathf.CeilToInt(Time.deltaTime * updateSpeed);
            if (slowText > updatedCurrentHealth) slowText = updatedCurrentHealth;

            if (percentage)
            {
                int percent = Mathf.RoundToInt((float)slowText / (float)maxHealth * 100);
                text.text = headerText + percent + "%";
            }
            else
            {
                if (showMax) text.text = headerText + slowText + "/" + maxHealth;
                else text.text = headerText + slowText;
            }

            if (colorChange)
            {
                float math = slowText / (float)maxHealth * 100F;
                if (math > percentHealthLow) text.color = defaultColor;
            }

            if (slowText >= updatedCurrentHealth) updatePos = false;
        }


        if (updateNeg)
        {
            slowText -= Mathf.CeilToInt(Time.deltaTime * updateSpeed);
            if (slowText < updatedCurrentHealth) slowText = updatedCurrentHealth;

            if (percentage)
            {
                int percent = Mathf.RoundToInt((float)slowText / (float)maxHealth * 100);
                text.text = headerText + percent + "%";
            }
            else
            {
                if (showMax) text.text = headerText + slowText + "/" + maxHealth;
                else text.text = headerText + slowText;
            }

            if (colorChange)
            {
                float math = slowText / (float)maxHealth * 100F;
                if (math <= percentHealthLow) text.color = new Color32(226, 58, 31, 255);  // red
            }

            if (slowText <= updatedCurrentHealth) updateNeg = false;
        }
    }
}
