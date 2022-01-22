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

    //��������� �������
    public void StopClientButton()
    {
        singleton.StopClient();
    }

    //����� �������
    public void StartServerButton()
    {
        singleton.StartServer();
    }

    //��������� �������
    public void StopServerButton()
    {
        singleton.StopServer();
    }

    //����� �����
    public void StartHostButton()
    {
        singleton.StartHost();
    }

    //��������� �����
    public void StopHostButton()
    {
        singleton.StopHost();
    }
}
