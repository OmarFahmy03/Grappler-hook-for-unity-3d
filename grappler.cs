using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grappler : MonoBehaviour
{
    private CharacterController ch;
    public Transform GrapLiterally;
    private Camera cam;
    private State state;
    private Vector3 makemefly;
    float hooklenght;

    private enum State
    {
        Normal,
        HookThrown,
        HookFly


    }

    void Awake()
    {
        ch = GetComponent<CharacterController>();
        cam = transform.Find("Main Camera").GetComponent<Camera>();
        state = State.Normal;
        GrapLiterally.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            default:
            case State.Normal:
                HandleShot();
                break;

            case State.HookThrown:
                OMW();
                break;
            
            case State.HookFly:
                DoTheFly();
                break;
        }
    }
    void HandleShot()
    {
        if(Input.GetButtonDown("Fire2"))
        {
           if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit raycastHit))
           {
               makemefly = raycastHit.point;
               hooklenght = 0f;
               GrapLiterally.gameObject.SetActive(true);
               GrapLiterally.localScale = Vector3.zero;
               state = State.HookThrown;
           }
        }
    }
    void OMW()
    {
        GrapLiterally.LookAt(makemefly);
        float hooklenghtSpeed = 69f;
        hooklenght += hooklenghtSpeed * Time.deltaTime;
        GrapLiterally.localScale= new Vector3 (1, 1, hooklenght);
        if(hooklenght >= Vector3.Distance(transform.position, makemefly))
            state = State.HookFly;
    }
    void DoTheFly()
    {
        GrapLiterally.LookAt(makemefly);
        Vector3 WhereHook = (makemefly - transform.position).normalized;

        float flySpeedmin = 18f;
        float flyspeedmax = 69f;
        float flySpeed = Mathf.Clamp(Vector3.Distance(transform.position, makemefly), flySpeedmin, flyspeedmax);
        float flymulti = 2f;

        ch.Move(WhereHook * flySpeed * flymulti * Time.deltaTime);


        float Reached = 3f;
        if(Vector3.Distance(transform.position, makemefly) < Reached)
        {
            GrapLiterally.gameObject.SetActive(false);
            state = State.Normal;
        }
    }
}
