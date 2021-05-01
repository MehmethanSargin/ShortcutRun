using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick PlayerJoystick;
    public Transform PlayerSprite;

    [SerializeField] private float speed = 1f;
    
    private void Update()
    {
        transform.Translate(0,0,1 * speed * Time.deltaTime );
        PlayerSprite.position = new Vector3(PlayerJoystick.Horizontal + transform.position.x, 0, PlayerJoystick.Vertical + transform.position.z);
        transform.LookAt(new Vector3(PlayerSprite.position.x,0,PlayerSprite.position.z));
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
