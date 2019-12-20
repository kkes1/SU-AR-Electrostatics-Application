using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSphere : MonoBehaviour
{
    public GameObject myPrefab;
    public List<GameObject> createdSpheres = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateObject()
    {
        GameObject newSP = (GameObject)Instantiate(myPrefab, new Vector3(0, 1, 3), Quaternion.identity);
        createdSpheres.Add(newSP);
    }
}
