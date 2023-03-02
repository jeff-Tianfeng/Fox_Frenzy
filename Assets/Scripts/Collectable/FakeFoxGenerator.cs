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
    // Start is called before the first frame update
    void Start()
    {   
        foxBehaviour = collectable.GetComponent<FoxBehaviour>();
        foxBehaviour.setFoxFake();
        GenerateObjectFake();
    }

    // Update is called once per frame
    void Update()
    {

    }

        public void GenerateObjectFake()
    {

        Transform spawnAreaTransform = spawnArea.transform;

        Vector3 spawnAreaPosition = spawnAreaTransform.position;
        Vector3 spawnAreaSize = spawnAreaTransform.GetComponent<Renderer>().bounds.size;

        int xIndex = Random.Range(0,5);
        int zIndex = Random.Range(0,3);
        foxInstanceFake1 = Instantiate(collectable, new Vector3(xCoordinate[xIndex], 0.1f, zCoordinate[zIndex]), Quaternion.identity);
    }
}
