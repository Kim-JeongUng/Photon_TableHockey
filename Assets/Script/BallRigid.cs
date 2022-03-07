using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRigid : MonoBehaviour
{
    GameObject[] Bat;
    float batRadius = 3;
    float ballRadius = 4;

    float distance;
    float collisionDistance;
    Vector3 prevPos;

    Vector3 ballDir = Vector3.zero;
    float ballSpeed = 0.1f;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        Bat = GameObject.FindGameObjectsWithTag("Bat");
        collisionDistance = batRadius / 2 + ballRadius / 2;
        prevPos = Vector3.zero;
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(ballDir.normalized * ballSpeed);

        Vector3 Pos = transform.position;
        Collider[] cols = Physics.OverlapSphere(transform.position, 2.0f); // 반경 내의 모든 Collider 탐색
        transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);

        if (cols.Length >1)
        {

            Vector3[] PosCols = new Vector3[cols.Length];
            Vector3[] prevPosCols = new Vector3[cols.Length];
            //충돌
            for (int i = 0; i < cols.Length; i++)
            {
                distance = Vector3.Distance(this.transform.position, cols[i].transform.position);
                if (!cols[i].CompareTag("Ball"))
                {
                    if (cols[i].CompareTag("Wall"))//Pos != prevPos)
                        ballDir = Vector3.Reflect(ballDir.normalized, Vector3.zero); //cols[i].transform.position - transform.position));//new Vector3(cols[i].transform.position.x, 1, cols[i].transform.position.z));
                    if (cols[i].CompareTag("Bat"))//PosCols[i] != prevPosCols[i])
                        ballDir = transform.position - cols[i].transform.position;
                }

            }
            pos = transform.position;
        }
        
        prevPos = transform.position;

    }
}
