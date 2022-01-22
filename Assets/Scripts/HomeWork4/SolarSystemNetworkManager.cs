using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SolarSystemNetworkManager : NetworkManager
{
    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
    }

    public void StartClientButton()
    {
        singleton.StartClient();
    }

    //Остановка клиента
    public void StopClientButton()
    {
        singleton.StopClient();
    }

    //Старт сервера
    public void StartServerButton()
    {
        singleton.StartServer();
    }

    //Остановка сервера
    public void StopServerButton()
    {
        singleton.StopServer();
    }

    //Старт хоста
    public void StartHostButton()
    {
        singleton.StartHost();
    }

    //Остановка хоста
    public void StopHostButton()
    {
        singleton.StopHost();
    }
}
