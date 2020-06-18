using UnityEngine;
using System.Collections;


namespace TimelineIso
{
    public interface ICharacterInfo
    {

    }

    public struct Position : ICharacterInfo
    {
        public Vector3 p;
        //public float x, y, z;
    }
    public struct Velocity : ICharacterInfo
    {
        public Vector3 v;
        //public float x, y, z;
    }
    public struct Rotation : ICharacterInfo
    {
        public Quaternion r;
        //public float x, y, z, w;
    }

    public struct AnimationInfo : ICharacterInfo
    {
        public int stateHash; 
        public float normalizedTime; 
        public int layer; 
    }
}
