using UnityEngine;
using System.Collections;


namespace TimelineIso
{
    [System.Serializable]
    public enum EntityTag
    {
        Character,
        NonPlayer
    }

    [System.Serializable]
    public struct EntityIdentifier
    {
        // TODO: prob just guids or some shit
        private static uint count = 0;
        public static EntityIdentifier GetNew()
        {
            return new EntityIdentifier { identifier = EntityIdentifier.count++, tag = EntityTag.Character };
        }

        public EntityTag tag;
        public uint identifier;

        public bool Equals(EntityIdentifier other)
        {
            //return this.tag == other.tag && this.identifier == other.identifier;
            return this.identifier == other.identifier;
        }

    }
}
