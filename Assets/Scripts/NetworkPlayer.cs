using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkPlayer : MonoBehaviourPun
{
    public GameObject localCamera;
    void Start()
    {
        if(!photonView.IsMine)
        {
            localCamera.SetActive(false);
            MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
            foreach(MonoBehaviour script in scripts)
            {
                if(script is NetworkPlayer)
                {
                    continue;
                }
                else if(script is PhotonView)
                {
                    continue;
                }
                script.enabled = false;
            }
        }
    }
}
