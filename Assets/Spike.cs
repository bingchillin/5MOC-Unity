using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
public int hit = -1;

    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision){

        if (collision.gameObject.CompareTag("Player")){
            GameManager.Instance.UpdateHealth(hit);
        }
    }
}
