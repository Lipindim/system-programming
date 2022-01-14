using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


public class Task3 : MonoBehaviour
{
    private async void Start()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var task1 = Task2.LocalTask1(cancellationTokenSource.Token);
        var task2 = Task2.LocalTask2(cancellationTokenSource.Token);
        bool result = await WhatTaskFasterAsync(cancellationTokenSource, task1, task2);
        Debug.Log($"Result: {result}");
    }

    public static async Task<bool> WhatTaskFasterAsync(CancellationTokenSource cancellationTokenSource, Task task1, Task task2)
    {
        while (true)
        {
            if (task1.IsCompleted || task2.IsCompleted)
            {
                cancellationTokenSource.Cancel();
                return task1.IsCompleted;
            }
            else if (cancellationTokenSource.IsCancellationRequested)
            {
                return false;
            }
            await Task.Yield();
        }
    }
}

