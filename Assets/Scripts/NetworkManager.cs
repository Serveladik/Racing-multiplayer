using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public const string RoomName = "TestRoom";
    public GameObject camera;
    public Transform[] spawnTransform;

    public static int playerCount = 0;
    void Awake()
    {
        PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = 0;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

        Time.timeScale = 0;
        photonView.RPC("PauseGame", RpcTarget.All);
    }
    void Update()
    {
        //Debug.LogError(PhotonNetwork.CurrentRoom.Players.Count);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Connection failed: " + cause.ToString());
    }

    public override void OnConnectedToMaster()
    {
        if(!PhotonNetwork.IsConnected)
        {
            return;
        }
        Debug.Log("Connected");
        RoomOptions roomOptions = new RoomOptions() {IsVisible = true, MaxPlayers = 4};
        PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Success");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Created Failed");
    }

    public override void OnJoinedRoom()
    {
        Debug.LogError("OnJoinedRoom");
        
        if(PhotonNetwork.CurrentRoom.Players.Count >= 2)
        {
            photonView.RPC("PlayGame", RpcTarget.AllBuffered);
        }
        else
        {
            photonView.RPC("PauseGame", RpcTarget.All);
        }
        GameObject player = PhotonNetwork.Instantiate("KartClassic_Player", spawnTransform[PhotonNetwork.CurrentRoom.Players.Count - 1].position, spawnTransform[PhotonNetwork.CurrentRoom.Players.Count - 1].rotation, 0);
        Debug.LogError(PhotonNetwork.CurrentRoom.Players.Count);
    }

    [PunRPC]
    private void AddPlayerCount(int count)
    {
       playerCount = count++;
    }

    [PunRPC]
    private void PauseGame()
    {
        Debug.LogError("PAUSE");
        Time.timeScale = 0;
    }

    [PunRPC]
    private void PlayGame()
    {
        Time.timeScale = 1;
        Debug.LogError("PLAY");
        Debug.LogError("TIMESACLE: " + Time.timeScale);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New player connected");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player disconnected");
    }
    public override void OnJoinedLobby()
    {
        camera.SetActive(false);
        Debug.Log("JOINED");
    }
}
