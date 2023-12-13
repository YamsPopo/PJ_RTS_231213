using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Youchan
{
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        private string gameVersion = "2";
        public string userId = "Youchan";
        void Awake()
        {
            // ���� ���� �����鿡�� �ڵ����� ���� �ε�
            PhotonNetwork.AutomaticallySyncScene = true;
            //���� ������ �������� ���� ���
            PhotonNetwork.GameVersion = gameVersion;
            //���� ���̵� �Ҵ�
            PhotonNetwork.NickName = userId;
            //���� ������ ��� Ƚ�� ����(30������)
            Debug.Log(PhotonNetwork.SendRate);
            //���� ����
            PhotonNetwork.ConnectUsingSettings();
        }

        //���� ������ ���� �� ȣ��Ǵ� �ݹ� �Լ�
        public override void OnConnectedToMaster()
        {          
            Debug.Log("Connect Master");
            Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}"); //$ => String.Format() ���ڿ��� ��ȯ
            PhotonNetwork.JoinLobby(); // �κ� ����
        }
        //�κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
        public override void OnJoinedLobby()
        {
            Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
            PhotonNetwork.JoinRandomRoom(); //���� ��ġ����ŷ ���
        }
        //���� �뿡 ������ �� ȣ��Ǵ� �ݹ� �Լ�
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log($"JoinRandomFailed {returnCode} : {message}");

            //���� �Ӽ� ����
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 20; //�ִ� ������ �� (CCU)
            roomOptions.IsOpen = true; //���� ���� ����
            roomOptions.IsVisible = true; //�κ񿡼� �� ��Ͽ� ��Ÿ������ ����

            //�� ����
            PhotonNetwork.CreateRoom("My room", roomOptions);
        }

        //�� ������ �Ϸ�� �� ȣ��Ǵ� �ݹ� �Լ�
        public override void OnCreatedRoom()
        {
            Debug.Log("Created Room");
            Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
        }

        //�뿡 ������ �� ȣ��Ǵ� �ݹ� �Լ�
        public override void OnJoinedRoom()
        {
            Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
            Debug.Log($"PlayerCount = {PhotonNetwork.CurrentRoom.PlayerCount}");

            //�뿡 ������ ����� ���� Ȯ��
            foreach(var player in PhotonNetwork.CurrentRoom.Players)
            {
                Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");//ActorNumber = player�� ������ȣ
            }
            //ĳ���� ���� ������ �迭�� ����
            Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
            int index = Random.Range(0, points.Length-1);
            //ĳ���� ����
            PhotonNetwork.Instantiate("Player1", points[index].position, points[index].rotation, 0);
        }

    }
}
