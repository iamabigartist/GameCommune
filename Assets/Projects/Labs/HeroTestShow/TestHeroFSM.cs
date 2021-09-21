using UnityEngine;
using UnityEngine.AI;
public class TestHeroFSM : MonoBehaviour
{
#region Components

    NavMeshAgent m_agent;
    GameObject indicator;

#endregion

#region Statistics

    public float move_speed;

#endregion

#region Behaviour

#endregion

#region InternalBehaviour

#endregion

#region LifeCycle

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.speed = move_speed;
        m_agent.acceleration = float.MaxValue;
        m_agent.angularSpeed = float.MaxValue;
        indicator = Resources.Load<GameObject>( "MoveIndicator" );
    }


    void Update()
    {
        if (Input.GetMouseButton( 1 ))
        {
            if (
                Physics.Raycast(
                    Camera.main.ScreenPointToRay( Input.mousePosition ),
                    out RaycastHit info,
                    float.PositiveInfinity,
                    LayerMask.GetMask( "Ground" ) ))
            {
                m_agent.destination = info.point;
            }
            if (Input.GetMouseButtonDown( 1 ))
            {
                Instantiate( indicator, info.point, Quaternion.identity );
            }
        }

        Animation a = new Animation();

    }

#endregion

}
