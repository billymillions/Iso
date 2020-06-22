using UnityEngine;
using System.Collections;

namespace TimelineIso
{
    public class PlayerShootComponent : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        void TriggerShoot()
        {
            this.GetComponent<Animator>().Play("PlayerArrow");
        }

        void Shoot()
        {
            //if ReadValue<Vector2(this.TimelineEntity)
            //{
            //    var ec = this.ArrowPrefab.GetComponent<EntityComponent>();
            //    var obj = this.TimelineEntity.Spawn(ec, this.ArrowSpawn.position, this.ArrowSpawn.rotation);
            //    obj.GetComponent<Arrow>().speed = 100f;
            //}
            //var obj = Instantiate(this.ArrowPrefab, this.ArrowSpawn.position, this.ArrowSpawn.rotation);
            //obj.velocity = this.ArrowSpawn.forward.normalized * 100f;
            //StartCoroutine(ArrowCleanup(obj.gameObject));
        }

        IEnumerator ArrowCleanup(GameObject obj)
        {
            yield return new WaitForSeconds(1);
            Destroy(obj);
        }

    }
}
