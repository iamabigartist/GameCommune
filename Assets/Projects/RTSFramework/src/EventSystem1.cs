using System;
using System.Collections.Generic;
using UnityEngine;
namespace RTSFramework
{
    public class EventSystem1 : MonoBehaviour
    {
        static EventSystem1()
        {
            frame_events = new List<Event>();
        }

        public static List<Event> frame_events;

        // Update is called once per frame
        void Update()
        {
            foreach (Event e in frame_events)
            {
                switch (e.type)
                {

                    case Event.EventType.Enabled:
                        {
                            EventProcessing.ProcessEvent( e );
                            break;
                        }
                    case Event.EventType.Disabled:
                        {
                            break;
                        }
                    case Event.EventType.Temporary:
                        {
                            EventProcessing.ProcessEvent( e );
                            frame_events.Remove( e );
                            break;
                        }
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

}
