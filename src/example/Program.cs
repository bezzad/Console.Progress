using Console.Progress;
using System.Threading;

namespace Example;

public class Program
{
    static void Main(string[] args)
    {
        System.Console.OutputEncoding = System.Text.Encoding.UTF8;
        System.Console.CursorVisible = false;
        var progress = new ProgressBar();
        var progress2 = new ProgressBar(45, format: new ProgressBarFormat(left: "", right: "", full: '▉', tip: '\u2591', empty: '\u2591'));


        for (int i = 0; i < 100; i++)
        {
            System.Console.SetCursorPosition(0, 0);

            progress2.Increment();
            System.Console.Write(progress2);

            Thread.Sleep(100);
        }
    }
}
