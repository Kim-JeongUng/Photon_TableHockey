using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRigid : MonoBehaviour
{
    Vector3 ballDir = Vector3.zero;
    float ballSpeed = 0.1f;

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

        Vector3 Pos = transform.position;
        Collider[] cols = Physics.OverlapSphere(transform.position, 2.0f); // 반경 내의 모든 Collider 탐색

        if (cols.Length >1)
        {

            Vector3[] PosCols = new Vector3[cols.Length];
            Vector3[] prevPosCols = new Vector3[cols.Length];
            //충돌
            for (int i = 0; i < cols.Length; i++)
            {
                if (!cols[i].CompareTag("Ball"))
                {
                    if (cols[i].CompareTag("GoalPost"))
                    {
                        Destroy(this.gameObject,1.0f);
                        ballDir = Vector3.zero;
                    }
                    if (cols[i].CompareTag("Wall"))
                    { //Pos != prevPos)
                        ballDir = Vector3.Reflect(ballDir.normalized, Vector3.Normalize(cols[i].transform.position));
                    }
                    else if (cols[i].CompareTag("Bat"))//PosCols[i] != prevPosCols[i])
                        ballDir = transform.position - cols[i].transform.position;
                }

            }
        }
        pos = transform.position;
        transform.position = new Vector3(Mathf.Clamp(pos.x, -7.6f, 7.6f), 1, Mathf.Clamp(pos.z, -15.6f, 15.6f));
    }
}
