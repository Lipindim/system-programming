using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


public class Task2 : MonoBehaviour
{
    private void Start()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        LocalTask1(cancellationTokenSource.Token);
        LocalTask2(cancellationTokenSource.Token);
    }

    public static async Task LocalTask1(CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        Debug.Log("Task 1 completed.");
    }

    public static async Task LocalTask2(CancellationToken cancellationToken)
    {
        for (int i = 0; i < 60; i++)
        {
            await Task.Yield();
            if (cancellationToken.IsCancellationRequested)
                return;
        }

        Debug.Log("Task 2 completed.");
    }
}
