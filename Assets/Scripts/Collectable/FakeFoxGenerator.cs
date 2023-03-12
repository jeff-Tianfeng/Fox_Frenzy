using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Generate series of distracting fox
/// </summary>
public class FakeFoxGenerator : MonoBehaviour
{
    // Game Area
    public GameObject spawnArea;
    // Fox prefab
    public GameObject collectable;
    // Predefined fox points.
    private int[] xCoordinate = {-28, -14, 0, 14, 28};
    private int[] zCoordinate = {-28, 0, 28};
    private GameObject foxInstanceFake1;
    private GameObject foxInstanceFake2;
    public FoxHandler foxHandler;
    private bool isDifficultLevel = false;
    // Start is called before the first frame update
    void Start()
    {   

    }

    // Update is called once per frame
    void Update()
    {

    }

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
        Transform spawnAreaTransform = spawnArea.transform;
        Vector3 spawnAreaPosition = spawnAreaTransform.position;
        Vector3 spawnAreaSize = spawnAreaTransform.GetComponent<Renderer>().bounds.size;
        // Generate first fox.
        int xIndex = Random.Range(0,5);
        int zIndex = Random.Range(0,3);
        //prevent fake fox and real fox pop is same position.
        while(true){
            xIndex = Random.Range(0,5);
            zIndex = Random.Range(0,3);
            if(xIndex != foxHandler.getX() && zIndex != foxHandler.getY())
            {
                break;
            }
        }
        foxInstanceFake1 = Instantiate(collectable, new Vector3(xCoordinate[xIndex], 0.1f, zCoordinate[zIndex]), Quaternion.identity);
        // if is difficult level, add another fake fox.
        if(isDifficultLevel == true)
        {
            xIndex = Random.Range(0,5);
            zIndex = Random.Range(0,3);
            //prevent fake fox and real fox pop is same position.
            while(true){
                xIndex = Random.Range(0,5);
                zIndex = Random.Range(0,3);
                if(xIndex != foxHandler.getX() && zIndex != foxHandler.getY())
                {
                    break;
                }
            }
            foxInstanceFake2 = Instantiate(collectable, new Vector3(xCoordinate[xIndex], 0.1f, zCoordinate[zIndex]), Quaternion.identity);
        }
    }

    public void setDifficult(bool level)
    {
        isDifficultLevel = level;
    }
}
