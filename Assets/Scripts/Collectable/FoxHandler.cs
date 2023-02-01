using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the generation and "ownership" of the current fox
/// </summary>
public class FoxHandler : MonoBehaviour
{
    public GameObject collectable;
    public GameObject spawnArea;
    public GameController gameController;

    private GameObject foxInstance;

    void Start()
    {
        if (foxInstance == null) GenerateObject();
    }

    /// <summary>
    /// Functionality to be triggered when a collectable is collected.
    /// </summary>
    public void CollectableCollected()
    {
        GenerateObject();
    }

    /// <summary>
    /// Generates a new instance of the collectable object in a random location.
    /// Destroys any existing instances of the collectable object.
    /// </summary>
    public void GenerateObject()
    {
        if (foxInstance != null)
        {
            Destroy(foxInstance);
        }

        Transform spawnAreaTransform = spawnArea.transform;

        Vector3 spawnAreaPosition = spawnAreaTransform.position;
        Vector3 spawnAreaSize = spawnAreaTransform.GetComponent<Renderer>().bounds.size;

        //calculate random x and z positions within the bounds of the plane
        float xPosition = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float zPosition = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);

        float yPosition = spawnAreaPosition.y + collectable.transform.localScale.y;

        foxInstance = Instantiate(collectable, new Vector3(xPosition, yPosition, zPosition), Quaternion.identity);

        foxInstance.GetComponent<FoxBehaviour>().gameController = gameController;
        foxInstance.GetComponent<DataCollector>().gameController = gameController;

    }

    public GameObject GetFox()
    {
        if (foxInstance == null) GenerateObject();
        return foxInstance;
    }
}
