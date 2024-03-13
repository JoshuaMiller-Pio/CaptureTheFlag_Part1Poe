using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private Vector2 _rotation;
    [SerializeField] private float lookSpeed;

    private void Update()
    {
        StartCoroutine("MouseMovement");

    }

    IEnumerator MouseMovement()
    {
        _rotation.y += Input.GetAxis("Mouse X");
        _rotation.x += -Input.GetAxis("Mouse Y");

        _rotation.x = Mathf.Clamp(_rotation.x, -30f, 30f);

        //look sides
        transform.eulerAngles = new Vector2(0, _rotation.y) * lookSpeed;

        //look up
        Camera.main.transform.localRotation = Quaternion.Euler(_rotation.x * lookSpeed, 0, 0);

        yield return null;
    }

}
