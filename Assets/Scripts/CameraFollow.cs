using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Transform CamRoot;
    public Transform MainGamePos;
    public Transform NearCamPos;
    public Transform FarCamPos;
    public Camera MainCam;
    public PlayerCollisionsController PlayerCollisionsController;
    
    void Start()
    {
        MainCam.transform.SetParent(CamRoot);
        MainCam.transform.localPosition = Vector3.zero;
        MainCam.transform.localRotation = Quaternion.identity;
    }

   
    void LateUpdate()
    {
        //dumping olur gibi
        CamRoot.position = Vector3.Lerp(CamRoot.position, Target.position, Time.deltaTime * 5);
        //REMAP
        var inverseValue = Mathf.InverseLerp(0, 10, PlayerCollisionsController.CollectedWood.Count);
        MainCam.transform.localPosition = Vector3.Lerp(NearCamPos.localPosition, FarCamPos.localPosition, inverseValue);
        MainCam.transform.localRotation = Quaternion.Lerp(NearCamPos.localRotation,FarCamPos.localRotation,inverseValue);
    }
}
