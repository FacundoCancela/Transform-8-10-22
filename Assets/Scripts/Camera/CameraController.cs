using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform cameraTarget;
    [SerializeField] private float cameraOffsetY;
    [SerializeField] private float cameraMoveSpeed;

    //################ #################
    //------------UNITY F--------------
    //################ #################

    private void Start()
    {
        cameraTarget = PlayerController.instance.gameObject.transform;
    }

    private void FixedUpdate()
    {
        Vector3 cameraPos = Vector3.Lerp(transform.position, cameraTarget.position, cameraMoveSpeed * Time.deltaTime);
        cameraPos.y = cameraTarget.position.y + cameraOffsetY;
        cameraPos.z = transform.position.z;
        transform.position = cameraPos;
    }


    //################ #################
    //----------CLASS METHODS-----------
    //################ #################
}
