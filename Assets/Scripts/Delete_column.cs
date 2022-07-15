using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete_column : MonoBehaviour
{
    [SerializeField] private GameObject playerTransform;
    public int pickRange = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerTransform.transform.position, playerTransform.transform.forward, out hit, pickRange))
        {
            if (hit.transform.gameObject == this)
            {
                if (Input.GetKey("e"))
                Destroy(transform.parent.gameObject);
            }

        }
    }
}
