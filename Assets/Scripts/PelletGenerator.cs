using UnityEngine;
using UnityEngine.AI; 

public class PelletGenerator : MonoBehaviour
{
    [Header("Parameters")]
    public GameObject pelletPrefab;
    public float gridSpacing = 1.0f;
    public float yOffset = 0.5f;
    public Transform pelletParent;
    public BoxCollider generationArea; 

    [ContextMenu("GeneratePellets")]
    public void GeneratePellets()
    {
        if (pelletParent != null)
        {
            foreach (Transform child in pelletParent)
            {
                Destroy(child.gameObject);
            }
        }

        Bounds bounds = generationArea.bounds;

        for (float x = bounds.min.x; x < bounds.max.x; x += gridSpacing)
        {
            for (float z = bounds.min.z; z < bounds.max.z; z += gridSpacing)
            {
                Vector3 attemptPos = new Vector3(x, bounds.min.y, z);
                Debug.Log("Attempting to create pellet at " + attemptPos);
                NavMeshHit hit;
                if (NavMesh.SamplePosition(attemptPos, out hit, 1f, NavMesh.AllAreas))
                {
                    Vector3 finalPos = hit.position;
                    finalPos.y += yOffset; // 墊高一點

                    GameObject newPellet = Instantiate(pelletPrefab, finalPos, Quaternion.identity);
                    
                    if (pelletParent != null)
                    {
                        newPellet.transform.SetParent(pelletParent);
                    }
                }
                else
                {
                    Debug.Log("Could not create pellet at " + attemptPos);
                }
            }
        }
    }
}