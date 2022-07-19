// See https://aka.ms/new-console-template for more information

var semaphore = new Semaphore(1, 1, "FirstSemaphore");

Semaphore? foreignSemaphore;
var once = false;
while (true)
{
    if (!once)
    {
        Console.WriteLine("Waiting for other semaphore...");
        once = true;
    }
    foreignSemaphore = new Semaphore(1, 1, "SecondSemaphore", out var salam);
    if (!salam)
    {
        break;
    }
    foreignSemaphore.Dispose();
}
var number = 1;

while (number <= 9)
{
    semaphore.WaitOne();
    Console.WriteLine(number);
    number += 2;
    semaphore.Release();
    foreignSemaphore.WaitOne();
    Thread.Sleep(400); // simulate waiting
    foreignSemaphore.Release();
}