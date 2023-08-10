using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public Image[] heartImages;

    private bool isBlocking = false;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHeartUI();
        PlayerController.OnBlockingStateChanged += OnPlayer1BlockingStateChanged;
    }

    private void OnPlayer1BlockingStateChanged(bool isPlayer1Blocking)
    {
        isBlocking = isPlayer1Blocking;
    }

    private void TakeDamage(int amount)
    {
        currentHealth -= amount;
        UpdateHeartUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplyDamage(int damageAmount)
    {
        if (!isBlocking)
        {
            TakeDamage(damageAmount);
        }

    }

    private void Die()
    {

        Debug.Log("Player is dead!");
    }

    private void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentHealth)
            {
                heartImages[i].enabled = true;
            }
            else
            {
                heartImages[i].enabled = false;
            }
        }
    }


}
