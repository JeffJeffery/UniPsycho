using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.parent.transform.parent.GetComponent<UnicycleMovment>().noseCollisionDetected(collision);
    }

    
}
