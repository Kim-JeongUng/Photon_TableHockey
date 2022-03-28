using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace Photon.Pun.Demo.PunBasics
{
    public class BallRigid : MonoBehaviourPunCallbacks, IPunObservable
    {
        Vector3 ballDir = Vector3.zero;
        float ballSpeed = 0.15f;
        Vector3 pos;

        [SerializeField]
        private Text BlueScores;
        [SerializeField]
        private Text RedScores;
        // Start is called before the first frame update
        void Start()
        {
            pos = transform.position;
            BlueScores = GameObject.Find("BlueScore").GetComponent<Text>();
            RedScores = GameObject.Find("RedScore").GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                transform.Translate(ballDir.normalized * ballSpeed);
            }
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
                                GameManager.RedScore += 1;
                            }
                            else if (cols[i].name == "RedGoalPost")
                            {
                                Debug.Log("hitRed");
                                GameManager.BlueScore += 1;
                            }
                            ballDir = Vector3.zero;
                            if (PhotonNetwork.IsMasterClient)
                            {
                                PhotonNetwork.Destroy(this.gameObject);
                            }
                        }
                        if (cols[i].CompareTag("Wall"))
                        {
                            ballDir = Vector3.Reflect(ballDir.normalized, Vector3.Normalize(cols[i].transform.position));
                        }
                        else if (cols[i].CompareTag("Bat"))
                            ballDir = transform.position - cols[i].transform.position;
                    }

                }
                if (PhotonNetwork.IsMasterClient) 
                { 
                    pos = transform.position;
                    transform.position = new Vector3(Mathf.Clamp(pos.x, -7.6f, 7.6f), 1, Mathf.Clamp(pos.z, -17.6f, 17.6f));
                }
            }
            BlueScores.text = GameManager.BlueScore.ToString();
            RedScores.text = GameManager.RedScore.ToString();
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            Debug.Log("CHECK");
            if (stream.IsWriting)
            {
                stream.SendNext(GameManager.BlueScore);
                stream.SendNext(GameManager.RedScore);
                Debug.Log("AAAA");
            }
            else
            {
                GameManager.BlueScore = (int)stream.ReceiveNext();
                GameManager.RedScore = (int)stream.ReceiveNext();
                if (BlueScores && RedScores)
                {
                    BlueScores.text = GameManager.BlueScore.ToString();
                    RedScores.text = GameManager.RedScore.ToString();
                }
                Debug.Log(GameManager.BlueScore);

                Debug.Log("BBBB");
            }
        }
    }
}