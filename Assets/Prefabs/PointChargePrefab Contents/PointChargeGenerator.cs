using System.Collections;
using System.Collections.Generic;
using EM.Core;
using UnityEngine;

public class PointChargeGenerator : FieldGenerator
{
    public MonopoleStrength monopoleStrength;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (!monopoleStrength)
        {
            monopoleStrength = GetComponent<MonopoleStrength>();
        }
    }

    public override Vector3 FieldValue(Vector3 position)
    {
        Vector3 Field = new Vector3(0, 0, 0);

        Field = monopoleStrength.Strength / Mathf.Pow(Vector3.Distance(this.gameObject.transform.position, position),2) * Vector3.Normalize(position-this.gameObject.transform.position);
        

        return Field;
    }
}
