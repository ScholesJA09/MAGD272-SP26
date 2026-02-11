using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExampleSceneScript : MonoBehaviour
{
    [Header("Players")]
    public List<GameObject> players;
    public List<Button> playerButtons;
    int activePlayer = 0;

    [Header("Health")]
    public List<HealthUI> healthOptions;
    public List<Button> healthButtons;
    int healthUI = -1;

    [Header("Texts")]
    public TextMeshProUGUI controls;

    public EnemyAttackManager[] enemies;

    public void ChooseCharacter(int choice)
    {
        activePlayer = choice;

        foreach (GameObject p in players) p.SetActive(false);
        players[choice].SetActive(true);

        foreach (Button b in playerButtons) b.interactable = true;
        playerButtons[choice].interactable = false;

        // set UI
        controls.text = "Attack Button: " + players[choice].GetComponent<Attack>().attackInput.ToString();

        if (healthUI > -1)
        {
            players[choice].GetComponent<PlayerHealth>().healthUI = healthOptions[healthUI];
            healthOptions[healthUI].setHealth(players[choice].GetComponent<PlayerHealth>().maxHealth, players[choice].GetComponent<PlayerHealth>().GetCurrentHealth());
        }
        
        // enemies re-start
        foreach (var enemy in enemies)
        {
            enemy.Start();
        }
    }

    public void ChooseUI(int choice)
    {
        healthUI = choice;

        foreach (HealthUI h in healthOptions) h.gameObject.SetActive(false);
        if (choice >= 0) healthOptions[choice].gameObject.SetActive(true);

        foreach (Button b in healthButtons) b.interactable = true;
        if (choice >= 0) healthButtons[choice].interactable = false;

        // set UI
        if(healthUI > -1)
        {
            players[activePlayer].GetComponent<PlayerHealth>().healthUI = healthOptions[healthUI];
            healthOptions[healthUI].setHealth(players[activePlayer].GetComponent<PlayerHealth>().maxHealth, players[activePlayer].GetComponent<PlayerHealth>().GetCurrentHealth());
        }
    }

    private void Start()
    {
        ChooseCharacter(0);
        ChooseUI(-1);
        enemies = FindObjectsOfType<EnemyAttackManager>();
    }
}
