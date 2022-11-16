using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] GameObject arrow;
    [SerializeField] TMP_Text shootCountText;
    [SerializeField] LayerMask ballLayer;
    [SerializeField] LayerMask rayLayer;
    [SerializeField] Transform  cameraPivot;
    [SerializeField] Camera cam;
    [SerializeField] Vector2 camSensitivity;
    [SerializeField] float shootForce;

    Vector3 lastMousePosition;
    float ballDistance;
    float forceFactor;
    Vector3 forceDir;
    bool isShooting;

    Renderer[] arrowRends;
    Color[] arrowOriginalColors;

    int shootCount=0;
    public int ShootCount { get => shootCount;}

    private void Start() 
    {
        ballDistance = Vector3.Distance(cam.transform.position,ball.Position) + 1;
        arrowRends = arrow.GetComponentsInChildren<Renderer>();
        arrowOriginalColors = new Color[arrowRends.Length];
        for (int i = 0; i < arrowRends.Length; i++)
        {
            arrowOriginalColors[i] = arrowRends[i].material.color;
        }
        arrow.SetActive(false);
        shootCountText.text = "Shoot Count: " +shootCount;
    }

    void Update()
    {
        //agar player tidak bisa memutar camera dan nge klik ball saat ball bergerak
        //ball.IsMoving || 
        if(ball.IsTeleporting)
        {
            return;
        }

        if(this.transform.position != ball.Position)
        {
            this.transform.position = ball.Position;
        }

        if(Input.GetMouseButtonDown(0))
        {
            
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,ballDistance,ballLayer))
            {
                isShooting = true;
                //Debug.Log("ball");

                //arrow
                arrow.SetActive(true);


            }
        }

        //if(Input.GetMouseButton(0))
        //{
        //    var ray = cam.ScreenPointToRay(Input.mousePosition);
        //    Debug.DrawLine(ray.origin, ray.origin + ray.direction*100);
        //}

        //shooting mode
        if(Input.GetMouseButton(0) && isShooting == true)
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, ballDistance*2,rayLayer))
            {
                Debug.DrawLine(ball.Position, hit.point);
                Debug.Log(hit.point);

                var forceVector = ball.Position - hit.point;
                forceVector = new Vector3(forceVector.x,0,forceVector.z);
                forceDir = forceVector.normalized;
                var forceMagnitude = forceVector.magnitude;
                Debug.Log(forceMagnitude);
                forceMagnitude = Mathf.Clamp(forceMagnitude,0,5);
                forceFactor = forceMagnitude/5;
            }

            //arrow
            this.transform.LookAt(this.transform.position+forceDir);
            arrow.transform.localScale = new Vector3(1+ 0.5f*forceFactor,1 + 0.5f*forceFactor,1+2*forceFactor);

            for (int i = 0; i < arrowRends.Length; i++)
            {
                arrowRends[i].material.color = Color.Lerp(arrowOriginalColors[i],Color.red, forceFactor);
            }

        }

        //camera mode
        if(Input.GetMouseButton(0) && isShooting == false)
        {
            var current = cam.ScreenToViewportPoint(Input.mousePosition);
            var last = cam.ScreenToViewportPoint(lastMousePosition);
            var delta = current - last;

            //memutar horizontal camera sesuai titik pivot benda(ball)
            cameraPivot.transform.RotateAround(ball.Position,Vector3.up,delta.x*camSensitivity.x);

            //memutar vertikal camera sesuai titik pivot benda(ball)
            cameraPivot.transform.RotateAround(ball.Position,cam.transform.right,-delta.y*camSensitivity.y);

            var angle = Vector3.SignedAngle(Vector3.up, cam.transform.up, cam.transform.right);

            //jika melewati batas putar balik
            if(angle < 3)
            {
                cameraPivot.transform.RotateAround(ball.Position,cam.transform.right,3-angle);
            }

            if(angle >65)
            {
                cameraPivot.transform.RotateAround(ball.Position,cam.transform.right,65-angle);
            }

        }

        if(Input.GetMouseButtonUp(0) && isShooting)
        {
            ball.AddForce(forceDir*shootForce*forceFactor);
            shootCount+=1;
            shootCountText.text = "Shoot Count: " +shootCount;
            forceFactor=0;
            forceDir=Vector3.zero;
            isShooting=false;
            arrow.SetActive(false);
        }

        lastMousePosition = Input.mousePosition;
    }
}
