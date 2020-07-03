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

    private void OnEnable()
    {
    }


    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        var characterHealth = other.GetComponent<CharacterHealth>();
        if (enemy && characterHealth)
        {
            characterHealth.InflictDamage(15);
        }
    }
}
