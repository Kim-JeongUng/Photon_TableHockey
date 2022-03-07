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

    // Start is called before the first frame update
    void Start()
    {
        Bat = GameObject.FindGameObjectsWithTag("Bat");
        collisionDistance = batRadius / 2 + ballRadius / 2;
        prevPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        /*transform.rotation = Quaternion.Euler(0,0,0);
        distance = Vector3.Distance(this.transform.position, Bat[0].transform.position);
        if(distance < collisionDistance)
        {
            Vector3 Pos = transform.position;
            Debug.Log("충돌");
            this.transform.Translate(Vector3.Reflect(Pos - prevPos,Bat[0].transform.) * Time.deltaTime * (collisionDistance-distance));
            prevPos = transform.position;
            Collider[] cols = Physics.OverlapSphere(transform.position, 4.0f);

            if (cols != null)
            {
                for(int i=0; i<cols.Length; i++)
                {
                    this.transform.Translate(Vector3.Reflect(Pos - prevPos, cols[i].transform.position) * Time.deltaTime * (collisionDistance - distance));
                }
            }

        }*/
        Vector3 Pos = transform.position;
        Collider[] cols = Physics.OverlapSphere(transform.position, 2.0f); // 반경 내의 모든 Collider
        transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);

        if (cols.Length >1)
        {

            Vector3[] PosCols = new Vector3[cols.Length];
            Vector3[] prevPosCols = new Vector3[cols.Length];
            Debug.Log("충돌");
            for (int i = 0; i < cols.Length; i++)
            {
                distance = Vector3.Distance(this.transform.position, cols[i].transform.position);
                Debug.Log(distance);
                if (!cols[i].CompareTag("Ball"))
                {
                    PosCols[i] = cols[i].transform.position;
                    if (cols[i].CompareTag("Wall"))//Pos != prevPos)
                        this.transform.Translate(Vector3.Reflect(new Vector3(Pos.x, 1, Pos.z) - new Vector3(prevPos.x, 1, prevPos.z), new Vector3(cols[i].transform.position.x, 1, cols[i].transform.position.z)) * Time.deltaTime * (collisionDistance - distance));
                    if (cols[i].CompareTag("Bat"))//PosCols[i] != prevPosCols[i])
                        this.transform.Translate((transform.position - cols[i].transform.position) * Time.deltaTime * (collisionDistance - distance)*1000);
                    
                    prevPosCols[i] = PosCols[i];
                }

            }
        }
        
        prevPos = transform.position;

    }
}
