using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public bool followActive = true;
    public bool followX = false;
    public bool followY = false;

    public Transform playerTransform;

    // Update is called once per frame
    void Update()
    {
        if(followActive && playerTransform != null) {
            Vector3 newPosition = transform.position;
            if(followX) {
                newPosition.x = playerTransform.position.x;
            }
            if(followY) {
                newPosition.y = playerTransform.position.y;
            }
            transform.position = newPosition;
        }
    }
}
