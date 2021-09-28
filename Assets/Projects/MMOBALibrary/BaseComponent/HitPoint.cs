using System;
using MMOBALibrary.Damage;
using MMOBALibrary.Data;
using MMOBALibrary.Definitions;
using UnityEngine;
namespace MMOBALibrary.BaseComponent
{
    public class HitPoint:MonoBehaviour
    {
    #region Event

        public event Action<DamageEventArgs> Damaged;

    #endregion

    #region Data

        public modifiable_float HP_capacity;
        public float value;
        public modifiable_float HP_regen;
        
    #endregion

    #region InternalBehaviour

        

    #endregion
        
    #region UnityEventHandler

        void Start()
        {

        }

    #endregion

    }
}
