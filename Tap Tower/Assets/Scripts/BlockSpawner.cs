using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject blockPrefab;
    public Transform spawnPoint;
    public float blockHeight = 1f;

    private int blockCount = 0;
    private Transform lastBlock; // Último bloque bien colocado



    public void BlockDropped(GameObject droppedBlock)
    {
        float maxAllowedOffset = 1.5f;

        if (lastBlock != null)
        {
            float offset = Mathf.Abs(droppedBlock.transform.position.x - lastBlock.position.x);

            if (offset > maxAllowedOffset)
            {
                Debug.Log("Desalineado. ¡El bloque cae!");
                Destroy(droppedBlock);
                return;
            }

            Rigidbody rb = droppedBlock.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        lastBlock = droppedBlock.transform;
        blockCount++;

        Vector3 spawnPosition = spawnPoint.position + Vector3.up * blockCount * blockHeight;
        GameObject newBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);

        CameraFollow camFollow = Camera.main?.GetComponent<CameraFollow>();
        if (camFollow != null)
            camFollow.target = newBlock.transform;
    }


    private void Start()
    {
        // Creamos el primer bloque fijo
        Vector3 spawnPosition = spawnPoint.position + Vector3.up * blockCount * blockHeight;
        GameObject firstBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);

        Rigidbody rb = firstBlock.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        lastBlock = firstBlock.transform;
    }
}
