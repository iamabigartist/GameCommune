using System;
using UnityEngine;
public class MOBACamera : MonoBehaviour
{
    public Rect move_range;
    public float horizontal_velocity;
    public Vector2 height_range;
    public float vertical_velocity;

    public Transform m_hero;

    void Start()
    {
        transform.rotation = Quaternion.Euler( 60, 0, 0 );
    }


    void Update()
    {
        if (Input.mousePosition.x / Screen.width < float.Epsilon)
        {
            transform.position += Vector3.left * horizontal_velocity * Time.deltaTime;
        }
        if (Input.mousePosition.x / Screen.width > 1 + float.Epsilon)
        {
            transform.position += Vector3.right * horizontal_velocity * Time.deltaTime;
        }
        if (Input.mousePosition.y / Screen.height < float.Epsilon)
        {
            transform.position += Vector3.back * horizontal_velocity * Time.deltaTime;
        }
        if (Input.mousePosition.y / Screen.height > 1 + float.Epsilon)
        {
            transform.position += Vector3.forward * horizontal_velocity * Time.deltaTime;
        }

        transform.position +=
            Vector3.up * vertical_velocity * Time.deltaTime *
            Input.GetAxis( "Mouse ScrollWheel" ) * -1f;

        if (Input.GetKey( KeyCode.Space ))
        {
            if (!Physics.Raycast(
                Camera.main.ScreenPointToRay(
                    new Vector3( Screen.width / 2f, Screen.height / 2f, 0 ) ),
                out RaycastHit info, float.PositiveInfinity,
                LayerMask.GetMask( "Ground" ) )) { throw new Exception( "Camera out of Map!" ); }
            var cur_view_center = info.point;
            var offset = m_hero.position - cur_view_center;
            offset.y = 0;
            transform.position += offset;
        }

    }
}
