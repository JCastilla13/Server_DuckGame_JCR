using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

class Client
{
    private TcpClient tcp;
    private string nick;
    private bool waitingPing;

    public Client(TcpClient tcp)
    {
        //Asignar el cliente TCP proporcionado
        this.tcp = tcp;
        //Nombre de usuario por defecto
        this.nick = "Guest";
        //El cliente no está esperando un ping inicialmente
        this.waitingPing = false;
    }

    //Obtenemos el estado de espera de ping del cliente
    public bool GetWaitingPing()
    {
        //Devolvemos el estado de espera de ping del cliente
        return this.waitingPing;
    }

    //Establecemos el estado de espera de ping del cliente
    public void SetWaitingPing(bool waitingPing)
    {
        //Asignamos el estado de espera de ping del cliente
        this.waitingPing = waitingPing;
    }

    //Obtenemos el cliente TCP asociado
    public TcpClient GetTcpClient()
    {
        //Devolvemos el cliente TCP asociado
        return this.tcp;
    }
}
