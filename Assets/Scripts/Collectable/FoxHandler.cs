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
    [SerializeField]
    public GameObject collectable;
    //Gaming Area.
    [SerializeField]
    public GameObject spawnArea;
    //Game scripts.
    [SerializeField]
    public GameController gameController;
    [SerializeField]
    public DataCollector dataCollector;
    [SerializeField]
    public FoxBehaviour foxBehaviour;
    [SerializeField]
    public FakeFoxGenerator fakeFoxGenerator;
    //Three Fox Gameobject, including two fake fox (with no sound).
    private int lifeTime = 8;
    private GameObject foxInstance;
    private GameObject foxInstanceFake1;
    private GameObject foxInstanceFake2;
    //Fox spawn point range.
    private int[] xCoordinate = {-28, -14, 0, 14, 28};
    private int[] zCoordinate = {-28, 0, 28};
    // List to store fox generate points.
    private List<Coords> coords= new List<Coords>();
    // default fox generate seed.
    private List<Coords> playerTest = new List<Coords>();
    private int foxGenerateCounter = 0;
    private string tempStr;
    //Blocker to prevent multi call.
    private bool coordBlocker = false;

    private int xIndex = 0;
    private int zIndex = 0;

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
    /// <summary>
    /// Use a random seed to disrupt original list.
    /// </summary>
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
        // get the spawn area attributes.
        Transform spawnAreaTransform = spawnArea.transform;
        Vector3 spawnAreaPosition = spawnAreaTransform.position;
        Vector3 spawnAreaSize = spawnAreaTransform.GetComponent<Renderer>().bounds.size;
        // generate new constructor instance.
        Coords temp = playerTest[foxGenerateCounter];
        coords.Add(temp);
        // instantiate the fox object.
        foxInstance = Instantiate(collectable, new Vector3(temp.x, -0.2f, temp.y), Quaternion.identity);
        foxInstance.GetComponent<FoxBehaviour>().gameController = gameController;
        foxInstance.GetComponent<DataCollector>().gameController = gameController;
        // assign values to the constructor.
        xIndex = temp.x;
        zIndex = temp.y;

        fakeFoxGenerator.GenerateObjectFake();
        foxGenerateCounter++;
        if(foxGenerateCounter == 15)
        {
            fakeRandom();
            foxGenerateCounter = 0;
        }

    }
    /// <summary>
    /// Use a random seed to disrupt original list.
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

    public void collectTrueFox()
    {
        Title.Instance.Show(
            "You found the fox!",
            "New search begin now",
            50,
            lifeTime: lifeTime
        );
    }

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
