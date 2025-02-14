using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerMegaman : MonoBehaviour
{
    public static GameManagerMegaman Instance{get; private set;}
    private void Awake(){
        if(Instance != null&& Instance != this){
            Destroy(this);
        }else{
            Instance = this;
        }
    }

    private int score = 0;
    private int vie = 100;

    public int timer = 5;
    private float calcTimer = 0f;

    private bool startTime = false;

    public TextMeshProUGUI vieText;
    public TextMeshProUGUI timerText;

    public GameObject player;
    public MagamanBoss boss;

    public Door[] door;

    Vector2 startPos;

    private void Start(){
        player = GameObject.FindWithTag("Player");

        startPos = player.transform.position;
    }

    public void StartTime(){
        startTime = true;
        Debug.Log("Start Time");
    }

    public void UpdateScore(int value){
        score += value;
    }

    public void UpdateHealth(int value){
        vie += value;
        Debug.Log(vie);
        vieText.text = "PV : " + vie + "/100";
        if(vie <= 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        }
        
    }

    void Update(){
        
        
        if(startTime){
            
            calcTimer += Time.deltaTime;
            if(calcTimer >= 1f){
                timer--;
                timerText.text = "Timer : " + timer + "/300";
                calcTimer = 0f;
            }
            if(timer <= 0){
                Destroy(boss.gameObject);
                foreach(Door d in door){
                    d.Open();
                }
            }
        }
    }
}
