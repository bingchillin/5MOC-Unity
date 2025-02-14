using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public TextMeshProUGUI vieText;


    public GameObject player;
    Vector2 startPos;

    private void Start(){
        player = GameObject.FindWithTag("Player");

        startPos = player.transform.position;
    }

    public void UpdateScore(int value){
        score += value;
    }

    public void UpdateHealth(int value){
        vie += value;
        Debug.Log(vie);
        vieText.text = "PV : " + vie + "/100";
        if(vie <= 0){
            player.transform.position = startPos;
            vie = 100;
        }
        
    }
}
