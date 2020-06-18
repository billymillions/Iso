using System.Collections;
using System.Collections.Generic;
using TimelineIso;
using UnityEngine;

public class Flip : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var parent = this.transform.parent;
        //if (Vector3.Dot(Camera.main.transform.right.XZPlane(), parent.GetComponent<Rigidbody>().velocity)<0)
        if (Vector3.Dot(Camera.main.transform.right.XZPlane(), parent.forward) < 0)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        } else
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
        
    }
}
