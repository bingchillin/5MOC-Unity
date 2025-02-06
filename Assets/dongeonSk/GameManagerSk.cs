using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerSk : MonoBehaviour
{
    public static GameManagerSk Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public int body = 0;
    public int healthSave = 100;
    public int health = 100;
    public int powerTime = 0;
    public int key = 0;
    public bool usePower = false;
    
  
    private float timer = 0f; // Stocke le temps écoulé

    public TextMeshProUGUI bodyText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI powerTimeText;
    public TextMeshProUGUI keyText;

    void Start()
    {
        powerTime = healthSave / 20; 

        // Mise à jour immédiate des textes
        if (healthText != null)
            healthText.text = "PV: " + health;
    }

    public void UpdateBody(int amount)
    {
        body += amount; 
    }

    public void UpdateHealth(int amount)
    {
       
        if(health+amount >= healthSave){
            health = healthSave;
        }else if(health<=0){
            health = 0;
        }else {
            health += amount;
        }
        
    }

    public void UpdateHealthSave(int amount)
    {
        healthSave = amount;
    }

    public void UpdatePowerTime(int amount)
    {
        powerTime = amount;
    }
    public void UpdateUsePower(bool isUse)
    {
        usePower = isUse;
    }
    
    public int GetPowerTime()
    {
        return powerTime;
    }

    public void Updatekey(int amount)
    {
        key += amount;
    }

    void Update()
    {
        timer += Time.deltaTime;
        Debug.Log("Valeur actuelle de health: " + health);
        
        // Régénération du pouvoir seulement si le pouvoir n'est PAS utilisé
        if (timer >= 2f && !usePower)
        {
            if (powerTime < healthSave / 20) 
            {
                powerTime++;  // Incrémente le temps de pouvoir
            }
            timer = 0f; // Réinitialise le timer
        }

        if (bodyText != null)
            bodyText.text = "BODY: " + body + "%";

        if (healthText != null){
            
            healthText.text = "PV: " + health + "/" + healthSave; 

            if (health <= 20)
            {
                healthText.color = Color.red; 
            }else if(health <= 40){
                 healthText.color = new Color(1f, 0.5f, 0f);; 
            }
            else
            {
                healthText.color = Color.white; 
            }
        }
            

        

        if (powerTimeText != null)
            powerTimeText.text = "P: " + powerTime; 

        if (keyText != null)
            keyText.text = "KEY: " + key; 

        
       
    }

    
}
