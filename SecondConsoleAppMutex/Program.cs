// See https://aka.ms/new-console-template for more information

var semaphore = new Semaphore(1, 1, "SecondSemaphore");

Semaphore? foreignSemaphore;
var once = false;
while (true)
{
    if (!once)
    {
        Console.WriteLine("Waiting for other semaphore...");
        once = true;
    }
    foreignSemaphore = new Semaphore(1, 1, "FirstSemaphore", out var salam);
    if (!salam)
    {
        break;
    }
    foreignSemaphore.Dispose();
}

var number = 2;

while (number <= 10)
{
    semaphore.WaitOne();
    Console.WriteLine(number);
    number += 2;
    semaphore.Release();
    foreignSemaphore.WaitOne();
    Thread.Sleep(400); // simulate waiting
    foreignSemaphore.Release();
}