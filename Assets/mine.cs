using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mine : MonoBehaviour
{
    public int damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Boss"))
        {
            GameManagerMegaman.Instance.UpdateHealth(damage*-1);
            Destroy(gameObject);
        }
    }
}
