using System.Collections;
using System.Collections.Generic;
using TimelineIso;
using UnityEngine;

public class CustomCollider : MonoBehaviour
{
    // Start is called before the first frame update
    bool isSet = false;
    //ParticleSystem.MinMaxGradient neutralColor;
    //[SerializeField]
    //public ParticleSystem.MinMaxGradient HitColor;
    
    void Start()
    {
        var ParentCollider = this.GetComponentInParent<Collider>();
        var ThisCollider = this.GetComponent<Collider>();
        Physics.IgnoreCollision(ParentCollider, ThisCollider);

        OnEnable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        //if (isSet)
        //{
        //    cl.color = neutralColor;
        //}
        //else
        //{
        //    isSet = true;
        //}
    }


    private void OnTriggerEnter(Collider other)
    {
        var characterHealth = other.GetComponent<CharacterHealth>();
        if (characterHealth)
        {
            characterHealth.InflictDamage(15);
        }
    }
}
