 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Constructor that contains the fox coordinates.
/// </summary>
public struct Coords
{
    public int x;
    public int y;
}

/// <summary>
/// Handles the generation and "ownership" of the current fox
/// </summary>
public class FoxHandler : MonoBehaviour
{   
    //Fox Prefab.
    public GameObject collectable;
    //Gaming Area.
    public GameObject spawnArea;
    //Game scripts.
    public GameController gameController;
    public DataCollector dataCollector;
    public FoxBehaviour foxBehaviour;
    public FakeFoxGenerator fakeFoxGenerator;
    //Three Fox Gameobject, including two fake fox (with no sound).
    private GameObject foxInstance;
    private GameObject foxInstanceFake1;
    private GameObject foxInstanceFake2;
    //Fox spawn point range.
    private int[] xCoordinate = {-28, -14, 0, 14, 28};
    private int[] zCoordinate = {-28, 0, 28};

    private List<Coords> coords= new List<Coords>();
    private string tempStr;
    //Blocker to prevent multi call.
    private bool coordBlocker = false;

    int xIndex = 0;
    int zIndex = 0;

    void Start()
    {
        if (foxInstance == null)
        {
            GenerateObjectTrue();
        }
        foxBehaviour = collectable.GetComponent<FoxBehaviour>();

    }

    void Update()
    {
        foxBehaviour.getState();
        // collect all generated fox point, store in Coords constructor and return data to dataCollector.
        if(gameController.GetTimer() == 10 && coordBlocker == false)
        {
            int length = coords.Count;
            for(int i = 0; i<length; i++)
            {
                tempStr = tempStr + "(" + coords[i].x + "," + coords[i].y + ")" + " ";
            }
            dataCollector.insertPointInfo(tempStr);
            coordBlocker = true;
        }
    }

    /// <summary>
    /// Functionality to be triggered when a collectable is collected.
    /// </summary>
    public void CollectableCollected()
    {
        GenerateObjectTrue();
    }

    /// <summary>
    /// Generates a new instance of the collectable object in a random location.
    /// Destroys any existing instances of the collectable object.
    /// </summary>
    public void GenerateObjectTrue()
    {
        if (foxInstance != null)
        {
            Destroy(foxInstance);
        }

        fakeFoxGenerator.GenerateObjectFake();

        Transform spawnAreaTransform = spawnArea.transform;

        Vector3 spawnAreaPosition = spawnAreaTransform.position;
        Vector3 spawnAreaSize = spawnAreaTransform.GetComponent<Renderer>().bounds.size;
        xIndex = Random.Range(0,5);
        zIndex = Random.Range(0,3);
        Coords temp = new Coords();
        temp.x = xCoordinate[xIndex];
        temp.y = zCoordinate[zIndex];
        if(coords.Contains(temp))
        {
            xIndex = Random.Range(0,5);
            zIndex = Random.Range(0,3);
            temp.x = xCoordinate[xIndex];
            temp.y = zCoordinate[zIndex];
        }
        coords.Add(temp);

        foxInstance = Instantiate(collectable, new Vector3(xCoordinate[xIndex], -0.2f, zCoordinate[zIndex]), Quaternion.identity);

        foxInstance.GetComponent<FoxBehaviour>().gameController = gameController;
        foxInstance.GetComponent<DataCollector>().gameController = gameController;

    }
    /// <summary>
    /// Get the fox gameObject.
    /// </summary>
    public GameObject GetFox()
    {
        if (foxInstance == null) 
        {
            GenerateObjectTrue();
        }
        return foxInstance;
    }

    public int getX()
    {
        return xIndex;
    }

    public int getY()
    {
        return zIndex;
    }
}
