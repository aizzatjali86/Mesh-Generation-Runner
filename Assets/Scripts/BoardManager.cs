using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int x;
    public int y;

    public List<Transform> pickedSqs = new List<Transform>();

    public GameObject mgPf;

    GameObject mg1;
    GameObject mg2;

    public GameObject target1;
    public GameObject target2;

    public PlayerController pc;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                GameObject square = Instantiate(Resources.Load("Prefabs/DrawSquare"), transform) as GameObject;

                square.transform.localPosition = new Vector3(i - x/2, 0, j - y/2);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Collider[] hitColliders = Physics.OverlapSphere(hit.point, 1.5f);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Squares"))
                    {
                        if (!hitCollider.GetComponent<TriggerBoard>().isPicked)
                        {
                            pickedSqs.Add(hitCollider.transform);
                            hitCollider.GetComponent<TriggerBoard>().isPicked = true;
                            hitCollider.GetComponent<TriggerBoard>().Picked();
                        }
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //Destroy prevoius meshes
            if (mg1 != null) Destroy(mg1);
            if (mg2 != null) Destroy(mg2);

            mg1 = Instantiate(mgPf, target1.transform) as GameObject;
            mg1.GetComponent<MeshGenerator>().CreateMesh(pickedSqs.ToArray());

            mg2 = Instantiate(mgPf, target2.transform) as GameObject;
            mg2.GetComponent<MeshGenerator>().CreateMesh(pickedSqs.ToArray());

            mg1.GetComponent<MeshGenerator>().anim = anim;
            mg2.GetComponent<MeshGenerator>().anim = anim;

            List<Transform> highest = new List<Transform>();

            int highY = -1000;

            foreach (Transform sq in pickedSqs)
            {
                //find highest, center point
                if (sq.localPosition.z == highY)
                {
                    highest.Add(sq);
                }
                else if (sq.localPosition.z > highY)
                {
                    highest.Clear();
                    highest.Add(sq);

                    highY = (int) sq.localPosition.z;
                }

                sq.GetComponent<TriggerBoard>().isPicked = false;
                sq.GetComponent<TriggerBoard>().Unpicked();
            }

            //Reposition to player and reposition player
            int leftLimit = -1000;
            int rightLimit= 1000;

            Transform leftmost = null;
            Transform rightmost = null;

            foreach (Transform sq in highest)
            {
                if (sq.localPosition.x > leftLimit)
                {
                    leftmost = sq;
                    leftLimit = (int)sq.localPosition.x;
                }

                if (sq.localPosition.x < rightLimit)
                {
                    rightmost = sq;
                    rightLimit = (int)sq.localPosition.x;
                }
            }

            //float center = (rightmost.localPosition.x + leftmost.localPosition.x) / 2;

            Vector3 pos = mg1.transform.localPosition;
            pos.x = -rightmost.localPosition.x;
            pos.z = y/2 - highest[0].localPosition.z;

            mg1.transform.localPosition = pos;
            mg2.transform.localPosition = pos;

            pickedSqs.Clear();
        }
    }
}
