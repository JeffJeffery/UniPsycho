using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.parent.transform.parent.GetComponent<UnicycleMovment>().headCollisionDetected(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.parent.transform.parent.GetComponent<UnicycleMovment>().noseCollisionDetected(collision);
    }

    public void addNose(GameObject nose, float nose_x) {
        GameObject myNose;
        myNose =  Instantiate(nose, new Vector3(0, 0, 0), Quaternion.identity);
        myNose.transform.parent = transform;
        myNose.transform.localPosition = new Vector3(nose_x, 0, 0);
    }


}
