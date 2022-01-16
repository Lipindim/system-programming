using System.Text;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


namespace HomeWork2
{
    
    public class Task1 : MonoBehaviour
    {
        [SerializeField] private int _arrayLength = 10;
        [SerializeField] private int _maxCutValue = 10;
        [SerializeField] private int _maxArrayValue = 20;
        
        private NativeArray<int> _array;

        private void Start()
        {
            _array = new NativeArray<int>(_arrayLength, Allocator.Persistent);
            FillRandomValues(_array, _maxArrayValue);
            OutputArray(_array);

            var job = new BigToZeroJob() { Numbers = _array, MaxValue = _maxCutValue };
            var handle = job.Schedule();
            handle.Complete();
            OutputArray(_array);
        }

        private void OutputArray(NativeArray<int> array)
        {
            var stringBuilder = new StringBuilder();
            foreach (var number in array)
                stringBuilder.Append($"{number} ");

            Debug.Log(stringBuilder.ToString());
        }

        private void FillRandomValues(NativeArray<int> array, int maxValue)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = (int)(Random.value * maxValue);
        }

        private void OnDestroy()
        {
            _array.Dispose();
        }
    }

}
