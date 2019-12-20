using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EM.Core;

/* 
 * The following script is attached to the GridOrigin prefab. 
 * Its functions are called by the Main Camera through its Speech Input Handler.
 * createGrid() is called by the speech command "Create Grid".
 */

public class GridManager : MonoBehaviour
{
    public GameObject originPoint; // set the origin point publically
    private GameObject gridOrigin; // will be used to create new grid origin points
    public GameObject pointingObject; // object that will point towards field charge
    private GameObject pointer; // will be used to point at field charge. will be created as child of gridOrigin.
    public int gridResolution; // affects how many pointing objects will be created. ex: if gridRes = 3, will have a 3x3x3 grid.
    public float distanceFromCamera = 2.5f; // determines how far away from the camera the grid is created (in meters). Set to 2.5 meters by default.
    Vector3 parentPos; // saves the position in space of the origin point of grid after instantiation.

    /* 
     * The createGrid function below contains the following functionality:
     *  Creates a GridOrigin prefab a set distance in front of where the camera is currently facing. This distance is set in the Unity Inspector, or to 2.5 meters by default.
     *  Uses a triple-nested for loop to create VectorArrow prefabs as children of the new GridOrigin prefab.
     *      At each step, calls GetCoordinates function passing current for loop counter variables to position new VectorArrow relative to parent GridOrigin.
     */

    public void createGrid() 
    {
        //create origin point where looking
        gridOrigin = Instantiate(originPoint, new Vector3(0, 0, 0), Quaternion.identity) as GameObject; // create new gridOrigin
        gridOrigin.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distanceFromCamera; // immediately set its position to x meters in front of camera. Set to 2.5 meters by default.
        parentPos = gridOrigin.transform.position; // store origin point's position in space as a vector3


        // The following three for loops increment the values of x, y, and z, until gridResolution is reached for the outermost loop.
        for (int z = 0; z < gridResolution; z++)
        {
            for (int y = 0; y < gridResolution; y++)
            {
                for (int x = 0; x < gridResolution; x++)
                {
                    pointer = Instantiate(pointingObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject; // instantiate new VectorArrow object briefly at origin
                    pointer.transform.SetParent(gridOrigin.transform); // set the new grid's origin as the pointer's parent object
                    pointer.transform.position = parentPos + GetCoordinates(x, y, z); // set the position of the current VectorArrow in space relative to the origin, using GetCoordinates()
                }
            }
        }
    }

    Vector3 GetCoordinates(int x, int y, int z) // this function affect's the grid's position relative to its origin point
    {
        return new Vector3( /* 
                             * for the following lines, i.e. (x - (gridResolution - a) * b)/c:
                             * a affects GridOriginPoint object's position within the grid, offsetting it from the center. It is properly centered when a == 1 for all coordinates.
                             * b offsets the entire grid of arrows in a direction away from its GridOriginPoint parent object. Direction is dependent on coordinate. Centered when b == 0.5f.
                             * c modifies the space between the arrows of the grid in the direction of the specified axis. Increasing the value causes the arrows to draw closer to one another
                             *     in the specified axis, whereas decreasing the value causes them to spread further apart. It is best to keep this value equal for all dimensions.
                             *       
                             * The following setup below is the optimal setup for the 3D grid. The GridOriginPoint parent is centered within the field of arrows, and the arrows are evenly spaced.
                             * If you would like to change the overall size of the grid, change the value of "c" as described above, making sure that it is equal for all three coordinates.
                             */
            (x - (gridResolution - 1) * 0.5f)/2.5f, // will affect the arrow's orientation in the x axis, meaning the x and -x directions.
            (y - (gridResolution - 1) * 0.5f)/2.5f, // will affect the arrow's orientation in the y axis, meaning the y and -y directions.
            (z - (gridResolution - 1) * 0.5f)/2.5f  // will affect the arrow's orientation in the x axis, meaning the z and -z directions.
        );
    }
}
