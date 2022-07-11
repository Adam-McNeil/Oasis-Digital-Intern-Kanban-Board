using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField]
    private Camera player;
    Camera[] allCameras;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null){
            player = Camera.current;
        }else{
            Vector3 dir = player.transform.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation(-dir);
        }

        allCameras = FindObjectsOfType<Camera>();
    }
}
