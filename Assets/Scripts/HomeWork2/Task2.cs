using System.Text;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


namespace HomeWork2
{
    public class Task2 : MonoBehaviour
    {
        [SerializeField] private int _arrayLength = 10;
        [SerializeField] private int _maxCoordinateValue = 10;

        private void Start()
        {
            using var positions = new NativeArray<Vector3>(_arrayLength, Allocator.Persistent);
            using var velocities = new NativeArray<Vector3>(_arrayLength, Allocator.Persistent);
            using var finishPositions = new NativeArray<Vector3>(_arrayLength, Allocator.Persistent);
            
            FillRandomValues(positions, _maxCoordinateValue);
            FillRandomValues(velocities, _maxCoordinateValue);
            
            Debug.Log("Start positions");
            OutputArray(positions);
            Debug.Log("Velocities");
            OutputArray(velocities);

            var job = new ChangePositiosJob()
            {
                Positions = positions,
                Velocities = velocities,
                FinalPositions = finishPositions
            };
            var handle = job.Schedule(positions.Length, 0);
            handle.Complete();
            Debug.Log("Finish positions");
            OutputArray(finishPositions);
        }

        private void OutputArray(NativeArray<Vector3> array)
        {
            var stringBuilder = new StringBuilder();
            foreach (var vector in array)
                stringBuilder.Append($"({vector} ");

            Debug.Log(stringBuilder.ToString());
        }

        private void FillRandomValues(NativeArray<Vector3> array, int maxValue)
        {
            for (int i = 0; i < array.Length; i++)
            {
                float randomX = Random.value * maxValue;
                float randomY = Random.value * maxValue;
                float randomZ = Random.value * maxValue;
                array[i] = new Vector3(randomX, randomY, randomZ);
            }
                
        }
    }
}

