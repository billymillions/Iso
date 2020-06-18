using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    public Transform target;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animator.SetFloat("enemyRange", (this.transform.position - target.position).magnitude);
    }
}
