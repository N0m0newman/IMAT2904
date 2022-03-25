using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    List<AITank> aiTanks = new List<AITank>();
    int currentlyVeiwing = 0;

    Vector3 cameraStartPos;
    Quaternion cameraStartRot;

    Camera mainCam;
    public float distance;            //how far back?
    public float maxDriftRange;        //how far are we allowed to drift from the target position
    public float angleX;            //angle to pitch up on top of the target
    public float angleY;            //angle to yaw around the target
    public GameObject mapCam;

    private Transform m_transform_cache;    //cache for our transform component
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GetComponent<Camera>();

        cameraStartPos = transform.position;
        cameraStartRot = transform.localRotation;
        GameObject[] aiTanksTemp = GameObject.FindGameObjectsWithTag("Tank");
        for (int i = 0; i < aiTanksTemp.Length; i++)
        {
            aiTanks.Add(aiTanksTemp[i].GetComponent<AITank>());
        }
    }

    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (currentlyVeiwing > 0)
        {
            if(aiTanks[currentlyVeiwing-1] != null)
            {
                Vector3 targetPos = GetTargetPos();
                //calculate drift theta
                float t = Vector3.Distance(myTransform.position, targetPos) / maxDriftRange;

                //smooth camera position using drift theta
                myTransform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
                //look at our targetPos
                myTransform.LookAt(target);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(currentlyVeiwing < aiTanks.Count)
            {
                currentlyVeiwing++;
            }
            else
            {
                currentlyVeiwing = 0;
            }
        }

        CameraView();
    }

    void CameraView()
    {
        if(currentlyVeiwing == 0)
        {
            mainCam.transform.position = cameraStartPos;
            mainCam.transform.rotation = cameraStartRot;

            mapCam.SetActive(false);
        }
        else
        {
            if (aiTanks[currentlyVeiwing - 1] != null)
            {
                target = aiTanks[currentlyVeiwing - 1].gameObject.transform;
            }
            else if (aiTanks[currentlyVeiwing - 1] == null)
            {
                aiTanks.RemoveAt(currentlyVeiwing - 1);
                currentlyVeiwing = aiTanks.Count - 1;
            }
            mapCam.SetActive(true);

        }
    }






    private Transform myTransform
    {//use this instead of transform
        get
        {//myTransform is guarunteed to return our transform component, but faster than just transform alone
            if (m_transform_cache == null)
            {//if we don't have it cached, cache it
                m_transform_cache = transform;
            }
            return m_transform_cache;
        }
    }

    //this runs when values are changed in the inspector
    void OnValidate()
    {
        if (target != null)
        {//we have a target, move the camera to target position for preview purposes
            Vector3 targetPos = GetTargetPos();
            //update position
            myTransform.position = targetPos;
            //look at our target
            myTransform.LookAt(target);
        }
    }





    private Vector3 GetTargetPos()
    {
        if (target != null)
        {
            //returns where the camera should aim to be
            //opposite of (-forward) * distance
            Vector3 targetPos = new Vector3(0, 0, -distance);
            //calculate pitch and yaw
            targetPos = Quaternion.Euler(angleX, angleY, 0) * targetPos;
            //return angled target position relative to target.position
            return target.position + (target.rotation * targetPos);
        }
        return Vector3.zero;
    }
}
