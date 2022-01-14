using UnityEngine;
using UnityEngine.Jobs;


namespace HomeWork2
{
    public struct RotationsJob : IJobParallelForTransform
    {
        public Vector3 Rotation;

        public void Execute(int index, TransformAccess transform)
        {
            transform.rotation = Quaternion.AngleAxis(Rotation.x, Vector3.right) * transform.rotation;
            transform.rotation = Quaternion.AngleAxis(Rotation.y, Vector3.up) * transform.rotation;
            transform.rotation = Quaternion.AngleAxis(Rotation.z, Vector3.forward) * transform.rotation;
        }
    }
}

