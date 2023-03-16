using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

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
    private List<Coords>  playerTest = new List<Coords>();
    private int foxGenerateCounter = 0;

    private string tempStr;
    //Blocker to prevent multi call.
    private bool coordBlocker = false;

    int xIndex = 0;
    int zIndex = 0;

    void Start()
    {
        fakeRandom();
        playerTest = Outoforder(playerTest);
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

    public void GenerateNewFakeFox()
    {
        fakeFoxGenerator.GenerateObjectFake();
    }
    private void fakeRandom()
    {
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                Coords fakeTemp = new Coords();
                fakeTemp.x = xCoordinate[i];
                fakeTemp.y = zCoordinate[j];
                playerTest.Add(fakeTemp);
            }
        }
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
        // xIndex = Random.Range(0,5);
        // zIndex = Random.Range(0,3);
        // Coords temp = new Coords();
        // temp.x = xCoordinate[xIndex];
        // temp.y = zCoordinate[zIndex];
        // if(coords.Contains(temp))
        // {
        //     xIndex = Random.Range(0,5);
        //     zIndex = Random.Range(0,3);
        //     temp.x = xCoordinate[xIndex];
        //     temp.y = zCoordinate[zIndex];
        // }
        Coords temp = playerTest[foxGenerateCounter];
        coords.Add(temp);

        foxInstance = Instantiate(collectable, new Vector3(temp.x, -0.2f, temp.y), Quaternion.identity);

        foxInstance.GetComponent<FoxBehaviour>().gameController = gameController;
        foxInstance.GetComponent<DataCollector>().gameController = gameController;

        foxGenerateCounter++;

    }
    /// <summary>
    /// Rearrange the item in List
    /// </summary>
    public List<T> Outoforder<T>(List<T> bag)
    {
        Random randomNum = new Random();
        int index = 0;
        T temp;
        for (int i = 0; i < bag.Count; i++)
        {
            index = randomNum.Next(0, bag.Count - 1);
            if (index != i)
            {
                temp = bag[i];
                bag[i] = bag[index];
                bag[index] = temp;
            }
        }
        return bag;
    }

    /// <summary>
    /// Get the fox gameObject.
    /// </summary>
    public GameObject GetFox()
    {
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
    public float getDeviation()
    {
        float deviation = foxBehaviour.getAngle(); 
        return deviation;
    }
}
