using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int points = 10;
    private Transform playerTransform; // position du joueur
    public float pickupRadius = 0.5f; // Distance pour ramasser la gemme

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else 
        {
            player = GameObject.FindGameObjectWithTag("Solid");
            if (player != null){
                playerTransform = player.transform;
            }else{
                player = GameObject.FindGameObjectWithTag("PassThrough");
                if (player != null )
                {
                    playerTransform = player.transform;
                }else {
                    Debug.LogError("Aucun joueur trouvé avec le tag 'Player' !");
                }
            }
            
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            
            float distance = Vector2.Distance(playerTransform.position, transform.position);
            
            if (distance <= pickupRadius)
            {
                GameManager.Instance.UpdateBody(points);
                Destroy(gameObject);
            }
        }
    }
}
