using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIconsUI : HealthUI   // base in HealthBarUI.cs
{
    [Header("Health icon when full")]
    public Sprite healthIcon;

    [Header("Health icon when empty")][Tooltip("If nothing here, icons will turn off instead")]
    public Sprite emptyIcon;

    Image[] icons;
    int currentIcon = -1;
    bool turningOffIcons = false;

    public override void setHealth(int max)
    {
        base.setHealth(max);

        if (!healthIcon)
        {
            Debug.Log("No health icon found! Cannot create icons.");
            FindObjectOfType<PlayerHealth>().healthUI = null;
            gameObject.SetActive(false);
            return;
        }
        if (!emptyIcon)
        {
            Debug.Log("No empty icon found! Icons will be turned off instead of sprite swaps.");
            turningOffIcons = true;
        }

        if (icons != null)
        {
            currentIcon = -1;
            while (currentIcon < maxHealth - 1)
            {
                currentIcon++;
                if(turningOffIcons) icons[currentIcon].gameObject.SetActive(true);
                else icons[currentIcon].sprite = healthIcon;
            }
        }
        else
        {
            icons = new Image[maxHealth];
            while (currentIcon < maxHealth - 1)
            {
                currentIcon++;
                icons[currentIcon] = createNewIcon();
            }
        }
    }

    public override void updateHealth(int newHealth)
    {
        if (newHealth < 0) newHealth = 0;
        else if (newHealth > maxHealth) newHealth = maxHealth;
        else if (newHealth == currentHealth) return;

        if (newHealth > currentHealth)
        {
            // increased health
            while (currentIcon < newHealth)
            {
                if (turningOffIcons) icons[currentIcon].gameObject.SetActive(true);
                else icons[currentIcon].sprite = healthIcon;
                currentIcon++;
            }

            currentIcon--;
        }
        else
        {
            // decreased health
            while (currentIcon > newHealth - 1)
            {
                if (turningOffIcons) icons[currentIcon].gameObject.SetActive(false);
                else icons[currentIcon].sprite = emptyIcon;
                currentIcon--;
            }
        }

        currentHealth = newHealth;
    }

        Image createNewIcon()
    {
        GameObject temp = new GameObject("HealthIcon " + currentIcon);
        temp.AddComponent<RectTransform>();
        Image i = temp.AddComponent<Image>();
        i.sprite = healthIcon;
        temp.transform.SetParent(gameObject.transform);
        temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        return i;
    }
}
