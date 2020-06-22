using UnityEngine;
using System.Collections;
using TimelineIso;

public class Enemy : MonoBehaviour
{
    private CharacterSelector charSelector;
    [SerializeField]
    public bool highlighted;
    [SerializeField]
    public Material highlightMaterial;
    public Material defaultMaterial;

    public void Start()
    {
        this.charSelector = GameObject.Find("CharSelector").GetComponent<CharacterSelector>();
        this.highlighted = false;
        this.defaultMaterial = this.GetComponent<MeshRenderer>().materials[0];
    }

    public void Update()
    {
        var renderer = this.GetComponent<MeshRenderer>();
        var mats = this.GetComponent<MeshRenderer>().materials;
        //if (this.charSelector.Selected.GetComponent<TimelineIso.PlayerController>().locked == this)
        //{
        //    this.highlighted = true;
        //    renderer.materials[1] = this.highlightMaterial;
        //    //mats[0] = this.highlightMaterial;
        //    //renderer.materials = mats;

        //    //    new Material[] {
        //    //    Resources.Load<Material>("Materials/Highlighted")
        //    //};
        //} else {
        //    this.highlighted = false;
        //    renderer.materials[1] = null;
        //    //this.GetComponent<MeshRenderer>().materials = new Material[] {
        //    //    Resources.Load<Material>("Materials/Flat")
        //    //};
        //}
    }

}
