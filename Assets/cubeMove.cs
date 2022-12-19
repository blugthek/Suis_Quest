using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeMove : MonoBehaviour
{
    public Transform targetPos;
    void Update()
    {
        CubeMovement();
    }

    private void CubeMovement()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position = targetPos.position;
        }
    }
}
