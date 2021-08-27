using System;
using System.Collections.Generic;
using UnityEngine;
namespace RTSFramework_v01
{
    public class EffectSystem1 : MonoBehaviour
    {
        static EffectSystem1()
        {
            frame_effects = new List<Effect>();
        }

        public static List<Effect> frame_effects;

        void Update()
        {
            foreach (Effect e in frame_effects)
            {
                switch (e.type)
                {

                    case Effect.EffectType.Enabled:
                        {
                            SingleEffectProcessing.ProcessEffect( e );
                            break;
                        }
                    case Effect.EffectType.Disabled:
                        {
                            break;
                        }
                    case Effect.EffectType.Temporary:
                        {
                            SingleEffectProcessing.ProcessEffect( e );
                            frame_effects.Remove( e );
                            break;
                        }
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

}
