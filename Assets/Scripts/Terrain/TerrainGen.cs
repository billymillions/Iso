using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainGen : MonoBehaviour
{
    public GameObject TreePrefab;
    public GameObject TreeZone;
    public int seed = 0;
    public int precision = 2;
    public int boundary;
    public int number;
    public List<GameObject> generated;
    // Start is called before the first frame update

    //public void Start()
    //{
    //    GenTerrain();
        
    //}

    public int Checksum
    {
        get
        {
            return seed + boundary + number;
        }

    }

    public void GenTerrain(int thisRun)
    {
        var r = new System.Random(seed);
        int count = 0;

        var toDestroy = new List<Transform>();
        foreach (Transform t in TreeZone.transform)
        {
            toDestroy.Add(t);
        }
        foreach (Transform t in toDestroy)
        {
            //EditorApplication.delayCall += () => CleanUp(t);
            Debug.Log(count++);
            CleanUp(t);
        }


        for (var i = 0; i < number; i++)
        {
            var x = (float)(r.NextDouble() - .5f) * boundary;
            var z = (float)(r.NextDouble() - .5f) * boundary;
            ClonePrefab(x, z);
            //EditorApplication.delayCall += () => ClonePrefab(x, z);
        }
    }

    public void OnValidate()
    {
        //var thisRun = num++;
        //EditorApplication.delayCall += () => GenTerrain(thisRun);
    }

    public void CleanUp(Transform t)
    {
        DestroyImmediate(t.gameObject);
        //if (t != null)
        //{
        //    DestroyImmediate(t.gameObject);
        //}
    }

    public void ClonePrefab(float x, float z)
    {
        var obj = Instantiate(this.TreePrefab, new Vector3(x, this.transform.position.y, z), this.TreeZone.transform.rotation, this.TreeZone.transform);
        //if (this != null)
        //{
        //    Instantiate(this.TreePrefab, new Vector3(x, this.transform.position.y, z), this.TreeZone.transform.rotation, this.TreeZone.transform);
        //}
    }


}
