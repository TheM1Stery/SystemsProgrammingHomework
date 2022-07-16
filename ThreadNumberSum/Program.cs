var number = 0;

var resetEvent = new AutoResetEvent(false);
var resetEventSecond = new AutoResetEvent(false);
void Operation()
{
    number += 500_000;
    Console.WriteLine($"{Thread.CurrentThread.Name}");
}

// this is the better solution :)
while (number != 6_000_000)
{
    var thread1 = new Thread(() =>
    {
        Operation();
        resetEvent.Set();
    })
    {
        IsBackground = true,
        Name = "Thread0"
    };
    var thread2 = new Thread(() =>
    {
        resetEventSecond.WaitOne(); // wait for the third thread to finish.
        Operation();
    })
    {
        IsBackground = true,
        Name = "Thread1"
    };
    var thread3 = new Thread(() =>
    {
        resetEvent.WaitOne(); // wait for the first thread to finish
        Operation();
        resetEventSecond.Set();
    })
    {
        IsBackground = true,
        Name = "Thread2"
    };
    thread1.Start();
    thread2.Start();
    thread3.Start();

    thread1.Join();
    thread2.Join();
    thread3.Join();
}
Console.WriteLine($"Result: {number}");

