using System.Collections;
using System.Collections.Generic;
using TimelineIso;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool FixY;
    public Vector3 rotation;
    void Update()
    {
        this.transform.forward = Camera.main.transform.forward;
        if (FixY)
        {
            this.transform.forward = this.transform.forward.XZPlane();
        }
        this.transform.Rotate(rotation);
    }
}
