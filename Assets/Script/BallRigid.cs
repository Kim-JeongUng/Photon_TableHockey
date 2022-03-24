using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Pun.Demo.PunBasics
{
    public class BallRigid : MonoBehaviourPunCallbacks
    {
        Vector3 ballDir = Vector3.zero;
        float ballSpeed = 0.15f;
        Vector3 pos;

        // Start is called before the first frame update
        void Start()
        {
            pos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(ballDir.normalized * ballSpeed);

            Collider[] cols = Physics.OverlapSphere(transform.position, 2.0f); // 반경 내의 모든 Collider 탐색

            if (cols.Length > 1)
            {

                //충돌
                for (int i = 0; i < cols.Length; i++)
                {
                    if (!cols[i].CompareTag("Ball"))
                    {
                        if (cols[i].CompareTag("GoalPost"))
                        {
                            if (cols[i].name == "BlueGoalPost")
                            {
                                Debug.Log("hitBlue");
                                GameObject.FindGameObjectsWithTag("Bat")[1].GetComponent<PlayerManager>().Score += 1;
                            }
                            else if (cols[i].name == "RedGoalPost")
                            {
                                Debug.Log("hitRed");
                                GameObject.FindGameObjectsWithTag("Bat")[0].GetComponent<PlayerManager>().Score += 1;
                            }
                            ballDir = Vector3.zero;
                            PhotonNetwork.Destroy(this.gameObject);
                        }
                        if (cols[i].CompareTag("Wall"))
                        { 
                            ballDir = Vector3.Reflect(ballDir.normalized, Vector3.Normalize(cols[i].transform.position));
                        }
                        else if (cols[i].CompareTag("Bat"))
                            ballDir = transform.position - cols[i].transform.position;
                    }

                }
            }
            pos = transform.position;
            transform.position = new Vector3(Mathf.Clamp(pos.x, -7.6f, 7.6f), 1, Mathf.Clamp(pos.z, -17.6f, 17.6f));
        }
    }
}