using System.Collections;
using System.Collections.Generic;
using TimelineIso;
using UnityEngine;

public class DisplayStamina : MonoBehaviour
{
    private CharacterStamina stamina;
    private PlayerController pc;
    private MeshRenderer mesh;
    private float lastStamina;

    // Start is called before the first frame update

    void Start()
    {
        this.stamina = this.GetComponentInParent<CharacterStamina>();
        this.pc = this.GetComponentInParent<PlayerController>();
        this.mesh = this.GetComponent<MeshRenderer>();
        this.lastStamina = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        this.lastStamina = Mathf.Lerp(this.lastStamina, this.stamina.Stamina.current / this.stamina.Stamina.max, 10*Time.deltaTime);
        this.mesh.materials[0].SetFloat("_Cutoff", this.lastStamina);

        this.mesh.materials[0].SetColor("_Color", (this.pc.IsBusy) ? Color.gray : Color.white);
    }
}
