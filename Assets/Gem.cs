using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int points = 5;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision){

        if (collision.CompareTag("Player")){
            GameManager.Instance.UpdateScore(points);
            Destroy(gameObject);
        }
    }
}
