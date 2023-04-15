using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Generate series of distracting fox
/// </summary>
public class FakeFoxGenerator : MonoBehaviour
{
    // Game Area
    [SerializeField]
    public GameObject spawnArea;
    // Fox prefab
    [SerializeField]
    public GameObject collectable;
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    public FoxHandler foxHandler;
    [SerializeField]
    private int lifeTime = 2;
    [SerializeField]
    public FakeFoxBehaviour fakeFoxBehaviour;
    // Predefined fox points.
    private int[] xCoordinate = {-28, -14, 0, 14, 28};
    private int[] zCoordinate = {-28, 0, 28};
    // Two fox gameObjects
    private GameObject foxInstanceFake1;
    private GameObject foxInstanceFake2;
    // If the game is difficult level.
    private bool isDifficultLevel = false;
    // two fake fox's positions.
    private Vector3 fox1Position = new Vector3(100,0,0);
    private Vector3 fox2Position = new Vector3(100,0,0);
    private float Fox1ToPlayer = 100;
    private float Fox2ToPlayer = 100;

    void Start()
    {
        // assign foxbehaviour class.
        fakeFoxBehaviour = collectable.GetComponent<FakeFoxBehaviour>();
    }
    // Update is called once per frame
    void Update()
    {
        // get the player position
        Vector3 playerPos = gameController.GetPlayerPosition();
        Fox1ToPlayer = Vector3.Distance(playerPos, fox1Position);
        Fox2ToPlayer = Vector3.Distance(playerPos, fox2Position);
    }
    /// <summary>
    /// Function to generate fake fox.
    /// </summary>
    public void GenerateObjectFake()
    {
        if(foxInstanceFake1 != null)
        {
            Destroy(foxInstanceFake1);
        }
        if(foxInstanceFake2 != null)
        {
            Destroy(foxInstanceFake2);
        }
        // get the spawn area's attributes.
        Transform spawnAreaTransform = spawnArea.transform;
        Vector3 spawnAreaPosition = spawnAreaTransform.position;
        Vector3 spawnAreaSize = spawnAreaTransform.GetComponent<Renderer>().bounds.size;
        // Generate first fox.
        int xIndex = Random.Range(0,5);
        int zIndex = Random.Range(0,3);
        Debug.Log(foxHandler.getX());
        //prevent fake fox and real fox pop is same position.
        while(true){
            xIndex = Random.Range(0,5);
            zIndex = Random.Range(0,3);
            if(xCoordinate[xIndex] != foxHandler.getX() && zCoordinate[zIndex] != foxHandler.getY())
            {
                break;
            }
        }
        // instantiate the fox.
        fox1Position =  new Vector3(xCoordinate[xIndex], 0.1f, zCoordinate[zIndex]);
        foxInstanceFake1 = Instantiate(collectable, fox1Position, Quaternion.identity);
        foxInstanceFake1.GetComponent<FakeFoxBehaviour>().gameController = gameController;
        // if is difficult level, add another fake fox.
        if(isDifficultLevel == true)
        {
            xIndex = Random.Range(0,5);
            zIndex = Random.Range(0,3);
            //prevent fake fox and real fox pop is same position.
            while(true){
                xIndex = Random.Range(0,5);
                zIndex = Random.Range(0,3);
                if(xCoordinate[xIndex] != foxHandler.getX() && zCoordinate[zIndex] != foxHandler.getY())
                {
                    break;
                }
            }
            fox2Position = new Vector3(xCoordinate[xIndex], 0.1f, zCoordinate[zIndex]);
            foxInstanceFake2 = Instantiate(collectable, fox2Position, Quaternion.identity);
            foxInstanceFake2.GetComponent<FakeFoxBehaviour>().gameController = gameController;
        }
    }
    /// <summary>
    /// Set the game difficulty level.
    /// </summary>
    public void setDifficult(bool level)
    {
        isDifficultLevel = level;
    }

}
