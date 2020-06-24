using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwaway : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        this.transform.position = pos;
        
    }
}
