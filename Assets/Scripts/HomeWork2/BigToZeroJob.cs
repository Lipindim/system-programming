using Unity.Collections;
using Unity.Jobs;


namespace HomeWork2
{

    public struct BigToZeroJob : IJob
    {
        public NativeArray<int> Numbers;
        public int MaxValue;

        public void Execute()
        {
            for (int i = 0; i < Numbers.Length; i++)
            {
                if (MaxValue <= Numbers[i])
                    Numbers[i] = 0;
            }
        }
    }

}

