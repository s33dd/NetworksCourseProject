using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace DataHandler {
  public class Connection {
    private Socket socket;
    private Socket? handler;
    private byte[] data;
    private double currentValue;
    private IPAddress ip;
    private int port;
    private bool stop;
    public Connection(string ip, string port) {
      socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      data = new byte[sizeof(double)];
      IPHostEntry ipHost = Dns.GetHostEntry(ip);
      this.ip = ipHost.AddressList[0];
      int portInt = int.Parse(port);
      this.port = portInt;
      stop = false;
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
    public void Bind() {
      IPEndPoint endPoint = new IPEndPoint(ip, port);
      socket.Bind(endPoint);
    }
    public void ListenRecieve(ObservableCollection<double> values) {
      socket.Listen();
      handler = socket.Accept();
      bool shown = false;
      while (!stop) {
        try {
          int bytes = handler.Receive(data);
          bool signal = false;
          handler.Send(BitConverter.GetBytes(signal));
          currentValue = BitConverter.ToDouble(data, 0);
          values.Add(currentValue);
        }
        catch {
          if (!shown) {
            MessageBox.Show("Error: Client disconnected");
            shown = true;
          }
        }
      }
    }
    public void CloseSock() {
      if (handler != null) {
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
      }
      socket.Close();
    }
    public double GetValue() {
      return currentValue;
    }
    public void Stop() {
      stop = true;
      bool signal = true;
      if (handler != null) {
        try { 
          handler.Send(BitConverter.GetBytes(signal));
        }
        catch {
          MessageBox.Show("Error: Client disconnected");
        }
      }
    }
  }
}
