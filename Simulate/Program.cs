using System;
using System.Threading;

namespace Simulate {
  internal class Program {
    public static void Simulating(int value, Connection conn) {
      Simulator simulator = new Simulator();
      simulator.Simulate(value, conn); // value is expected value, can be changed. Simulation will fluctuate near it.
    }
    static void Main(string[] args) {
      string ip;
      string port;
      do {
        Console.Write("Insert IP: ");
        ip = Console.ReadLine();
        Console.WriteLine();
        Console.Write("Insert Port: ");
        port = Console.ReadLine();
        Console.WriteLine();
      } while (!Connection.DataCorrect(ip, port));
      Console.Write("Insert expected value: ");
      int expected = int.Parse(Console.ReadLine());
      Connection conn = new Connection(ip);
      if(!conn.TryConnect(ip, port)) {
        Console.WriteLine("Can`t connect");
        return;
      }
      Thread simulator = new Thread(new ThreadStart(() => Simulating(expected, conn)));
      simulator.Start();
      simulator.Join();
      conn.Close();
    }
  }
}
