using UnityEngine;
using System.Collections;


namespace TimelineIso
{

    public class EntityComponent : MonoBehaviour
    {
        public EntityIdentifier identifier;

        // Use this for initialization
        void Start()
        {
            if (this.identifier.identifier <= 0)
            {
              this.identifier = EntityIdentifier.GetNew();
            }
        }
    }

}
