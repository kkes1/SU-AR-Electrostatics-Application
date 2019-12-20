using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EM.Core
{

    //[RequireComponent(typeof(EMStrength))]

    public abstract class FieldGenerator : MonoBehaviour
    {

        //public MonopoleStrength eMStrength { get; set; }
        public FieldType fieldType;


        // Start is called before the first frame update
        protected virtual void Start(){}

        public virtual void Register()
        {
            EMController.Instance.RegisterFieldGenerator(this);

        }

        public virtual void UnRegister()
        {
            EMController.Instance.UnRegisterFieldGenerator(this);
        }

        public abstract Vector3 FieldValue(Vector3 position);

        protected virtual void OnEnable()
        {
            Register();
        }

        protected virtual void OnDisable()
        {
            UnRegister();
        }
    }
}