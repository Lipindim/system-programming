using UnityEngine;
using UnityEngine.Jobs;


namespace HomeWork2
{
    public class Task3 : MonoBehaviour
    {
        [SerializeField] private Transform[] _transforms;
        [SerializeField] private Vector3 _rotation = new Vector3(1.0f, 0.0f, 0.0f);
        [SerializeField] private float _rotationSpeed = 10.0f;
        
        private void Update()
        {
            using var transformArray = new TransformAccessArray(_transforms);
            var job = new RotationsJob()
            {
                Rotation = _rotation * Time.deltaTime * _rotationSpeed
            };
            var handle = job.Schedule(transformArray);
            handle.Complete();
        }
    }
}

