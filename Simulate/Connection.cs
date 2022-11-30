using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Simulate {
  public class Connection {
    private byte[] buffer;
    private Socket socket;
    public Connection() {
      socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      buffer = new byte[sizeof(double)];
    }
    public static bool DataCorrect(string ip, string port) {
      string ipPattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
      string portPattern = @"^((6553[0-5])|(655[0-2][0-9])|(65[0-4][0-9]{2})|(6[0-4][0-9]{3})|([1-5][0-9]{4})|([0-5]{0,5})|([0-9]{1,4}))$";
      bool ipAns = Regex.IsMatch(ip, ipPattern);
      bool portAns = Regex.IsMatch(port, portPattern);
      if (ipAns && portAns) {
        return true;
      }
      else {
        return false;
      }
    }
    public bool TryConnect(string ip, string port) {
      int portInt = int.Parse(port);
      try {
        IPHostEntry ipHost = Dns.GetHostEntry(ip);
        IPAddress ipAddr = ipHost.AddressList[0];
        IAsyncResult result = socket.BeginConnect(ipAddr, portInt, null, null);
        bool waiter = result.AsyncWaitHandle.WaitOne(5000, true);
        if (socket.Connected) {
          socket.EndConnect(result);
          return true;
        }
        else {
          socket.Close();
          return false;
        }
      }
      catch {
        socket.Close();
        return false;
      }
    }
    public void Connect(string ip, string port) {
      int portInt = int.Parse(port);
      IPHostEntry ipHost = Dns.GetHostEntry(ip);
      IPAddress ipAddr = ipHost.AddressList[0];
      IPEndPoint endPoint = new IPEndPoint(ipAddr, portInt);
      socket.Connect(endPoint);
    }

    public int Send(double value) {
      buffer = BitConverter.GetBytes(value);
      return socket.Send(buffer);
    }
    public void Close() {
      socket.Shutdown(SocketShutdown.Both);
      socket.Close();
    }
    public bool Recieve() {
      try {
        socket.Receive(buffer);
        return BitConverter.ToBoolean(buffer, 0);
      }
      catch {
        return true;
      }
    }
  }
}
