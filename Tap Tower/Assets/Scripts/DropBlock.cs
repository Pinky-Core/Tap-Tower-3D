using UnityEngine;

public class DropBlock : MonoBehaviour
{
    private bool hasDropped = false;
    private Rigidbody rb;
    private BlockSwing swingScript;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        swingScript = GetComponent<BlockSwing>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if (!hasDropped)
        {
            hasDropped = true;
            GetComponent<BlockSwing>().enabled = false;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;

            FindObjectOfType<BlockSpawner>().BlockDropped(gameObject);

        }

    }
}
