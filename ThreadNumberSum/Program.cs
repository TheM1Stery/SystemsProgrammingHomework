var number = 0;
void Callback()
{
    number += 500_000;
    Console.WriteLine($"{Thread.CurrentThread.Name}");
}


while (number != 6_000_000)
{
    var thread1 = new Thread(Callback)
    {
        IsBackground = true,
        Name = "Thread1"
    };
    var thread2 = new Thread(Callback)
    {
        IsBackground = true,
        Name = "Thread2"
    };
    var thread3 = new Thread(Callback)
    {
        IsBackground = true,
        Name = "Thread3"
    };
    thread1.Start();
    thread1.Join();
    thread3.Start();
    thread3.Join();
    thread2.Start();
    thread2.Join();
}

Console.WriteLine($"Result: {number}");

