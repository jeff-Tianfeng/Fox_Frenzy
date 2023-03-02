using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeFoxGenerator : MonoBehaviour
{
    public GameObject spawnArea;
    public GameObject collectable;
    private int[] xCoordinate = {-28, -14, 0, 14, 28};
    private int[] zCoordinate = {-28, 0, 28};
    private GameObject foxInstanceFake1;
    public FoxBehaviour foxBehaviour;
    public FoxHandler foxHandler;
    // Start is called before the first frame update
    void Start()
    {   
        foxBehaviour = collectable.GetComponent<FoxBehaviour>();
        foxBehaviour.setFoxFake();
        if (foxInstanceFake1 == null) 
        {
            GenerateObjectFake();
        }
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

        Transform spawnAreaTransform = spawnArea.transform;

        Vector3 spawnAreaPosition = spawnAreaTransform.position;
        Vector3 spawnAreaSize = spawnAreaTransform.GetComponent<Renderer>().bounds.size;

        int xIndex = Random.Range(0,5);
        int zIndex = Random.Range(0,3);
        //prevent fake fox and real fox pop is same position.
        if(xIndex == foxHandler.getX() && zIndex == foxHandler.getY())
        {
            if(xIndex == 0)
            {
                xIndex ++;
            }else if(xIndex == 5)
            {
                xIndex --;
            }else{
                xIndex++;
            }
        }
        foxInstanceFake1 = Instantiate(collectable, new Vector3(xCoordinate[xIndex], 0.1f, zCoordinate[zIndex]), Quaternion.identity);
    }
}
