using gs.chef.vcontainer.utility;
using UnityEngine;

namespace gs.chef.vcontainer.extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 ToVector3(this SerializableVector3 serializableVector3)
        {
            return new Vector3(serializableVector3.x, serializableVector3.y, serializableVector3.z);
        }

        public static SerializableVector3 FromVector3(this Vector3 vector3)
        {
            return new SerializableVector3(vector3);
        }
    }
}