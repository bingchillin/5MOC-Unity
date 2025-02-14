using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pv : MonoBehaviour
{
    public int points = 10;  // Nombre de points à ajouter à la vie
    public float pickupRadius = 1.5f;  

    private void Update()
    {
      
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius);

        foreach (var collider in colliders)
        {

            if (collider.attachedRigidbody != null)
            {
                
                GameManagerSk.Instance.UpdateHealth(points);  
                Destroy(gameObject); 
                break;  
            }
        }
    }
}
