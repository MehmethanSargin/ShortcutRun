using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameOver : MonoBehaviour
{
    public GameObject GameOverPanel;
    private void OnCollisionEnter(Collision other)
    {
        GameOverPanel.SetActive(true);
        Destroy(other.gameObject);
        Camera.main.GetComponent<CameraFollow>().enabled = false;
    }
}
