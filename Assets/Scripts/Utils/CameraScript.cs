using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lodkod;

public class CameraScript : MonoBehaviour {

    public bool PlayerControl = true;

    public float _velocity;

    public Vector3 _target;
    Quaternion _rotation;
    public float _targetVelocity;
    public float _rotationVelocity;
    public Action _del = null;
    float _time;

    public bool _follow = false;
    public SkyObject Following = null;

    static float Zpos = -60;

    public void CameraDragWith(Vector2 points)
    {
        transform.Translate(points.x, points.y, 0f);
        //AspectCamera();
    }

    public void MoveToPoint(CameraPoint point)
    {
        Camera.main.transform.position = point.Point;
        Camera.main.transform.rotation = Quaternion.Euler(point.Rotation);
    }

    public void MoveToPointAsynk(CameraPoint point, Action del, float time = 2f)
    {
        this._time = time;
        this._rotation = Quaternion.Euler(point.Rotation);
        this._targetVelocity = Vector3.Distance(Camera.main.transform.position, point.Point) / time;
        this._rotationVelocity = Vector3.Distance(Camera.main.transform.rotation.eulerAngles, point.Rotation) / time;
        this._del = del;

        StartCoroutine(MoveCoroutine(point));
    }

    IEnumerator MoveCoroutine(CameraPoint point)
    {
        yield return StartCoroutine(MoveToPointCoroutine(point));

        if (this._del != null)
            this._del();
    }

    public IEnumerator MoveToPointCoroutine(CameraPoint point)
    {
        float delta = this._targetVelocity * Time.deltaTime;

        while (!Mathf.Approximately(this.transform.position.magnitude, point.Point.magnitude) || !Mathf.Approximately(this.transform.rotation.eulerAngles.magnitude, this._rotation.eulerAngles.magnitude))
        {
            this.transform.position = Vector3.MoveTowards(Camera.main.transform.position, point.Point, delta);
            this.transform.rotation = Quaternion.RotateTowards(transform.rotation, this._rotation, this._rotationVelocity * Time.deltaTime);
            
            yield return null;
        }
    }

    public void moveTo(Vector3 point)
    {
        Camera.main.transform.position = point;
        //AspectCamera();
    }

    public void moveToAsynk(Vector3 point, Action del, bool synk = false, float time = 2f)
    {
        this._target = point;
        this._targetVelocity = Vector3.Distance(Camera.main.transform.position, this._target) / time;
        this._del = del;

        StartCoroutine(moveToCoroutine());
    }

    IEnumerator moveToCoroutine()
    {
        yield return StartCoroutine(moveToPosition());

        if (this._del != null)
            this._del();
    }

    public IEnumerator moveToPosition()
    {
        while (!Mathf.Approximately(Camera.main.transform.position.magnitude, this._target.magnitude))
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, this._target, this._targetVelocity * Time.deltaTime);
            yield return null;
        }

        //AspectCamera();
    }

    Vector3 offsetFlyFollow = new Vector3(0.0f, 2.0f, -15f);

    public void StartFollow(SkyObject obj = null)
    {
        if (obj != null)
            Following = obj;
        else if (!Following.ID.Equals("MainShip"))
            Following = GM.GetObject("MainShip");

        _follow = true;
    }

    public void StopFollow()
    {
        _follow = false;
    }

    void LateUpdate()
    {
        if(_follow)
        {
            /*
            Vector3 lookToward = Following.position - transform.position;
            
            Vector3 newPos;
            newPos = Following.position - lookToward.normalized;
            newPos.y = Following.position.y + 6;

            _trans.position = new Vector3((newPos.x -_trans.position.x) * Time.deltaTime * 2f, (newPos.y - _trans.position.y + 6f) * Time.deltaTime * 2f, 0);
            */
            Vector3 desiredPos = Following.position + offsetFlyFollow;
            transform.position = Vector3.Lerp(transform.position, desiredPos, 0.125f);
            
        }

        if(PlayerControl)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
            {
                transform.Translate(_velocity * Time.deltaTime * Input.GetAxisRaw("Horizontal"), 0f, 0f);

            }


            if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + _velocity * Time.deltaTime * Input.GetAxisRaw("Vertical"), transform.position.z);
                //_trans.Translate(0f, _velocity * Time.deltaTime * Input.GetAxisRaw("Vertical"), 0f);
            }
        }
    }
}
