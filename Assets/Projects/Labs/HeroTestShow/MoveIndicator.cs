using UnityEngine;
namespace Labs.HeroTestShow
{
    public class MoveIndicator : MonoBehaviour
    {
        public AnimationCurve m_curve;
        float m_startTime;

        void Start()
        {
            m_startTime = Time.time;
        }

        void Update()
        {

            while (true)
            {
                var cur_scale = m_curve.Evaluate( Time.time - m_startTime );
                if (cur_scale == 0)
                {
                    break;
                }
                transform.localScale = Vector3.one * cur_scale;
                return;
            }
            Destroy( this.gameObject );
        }
    }
}
