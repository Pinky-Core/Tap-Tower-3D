using UnityEngine;

public class DestroyerZone : MonoBehaviour
{
    [SerializeField] private Tutorial tutorial;


    [SerializeField] private BasePlatformControl basePlatform;


    // El tag que debe tener el objeto para que sea destruido (por ejemplo: "Block")
    [SerializeField] private string tagID;

    // Este m�todo se llama autom�ticamente cuando otro objeto colisiona con este
    private void OnCollisionEnter(Collision collision)
    {
        // Si el objeto que colision� tiene el tag especificado...
        if (collision.gameObject.CompareTag(tagID))
        {
            if (collision.gameObject.TryGetComponent<BlockState>(out BlockState state))
            {
                if (state.yaFueColocado) return; // Ya fue colocado, ignorar
            }

            Destroy(collision.gameObject);

            bool incorrecto = false;
            tutorial.OnBlockDropped(incorrecto);
        }
    }
}
