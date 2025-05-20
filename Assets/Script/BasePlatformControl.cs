using UnityEngine;
using UnityEngine.Events;

public class BasePlatformControl : MonoBehaviour
{

    [SerializeField] private Tutorial tutorial;

    // Referencia al BoxCollider del objeto que act�a como plataforma
    private BoxCollider boxCollider;

    // Evento que se dispara cuando un bloque llega a la plataforma, pasando su posici�n
    public UnityEvent<Vector3> onBuildingReachPlatform;

    private void Awake()
    {
        // Obtener el componente BoxCollider en el mismo GameObject
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si el punto de contacto no viene desde arriba (comparando con el eje Y de la plataforma), ignorar
        if (Vector3.Dot(collision.contacts[0].point, transform.up) <= 0) return;

        // Si el objeto que colision� tiene un ParticleSystem, emitir 50 part�culas
        if (collision.gameObject.TryGetComponent<ParticleSystem>(out ParticleSystem ps))
            ps.Emit(50);

        // Si el objeto que colision� tiene un Rigidbody...
        if (collision.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
            // Detener su movimiento
            rigidbody.isKinematic = true;

            // Convertir el bloque en hijo de la plataforma (se mantiene su posici�n global)
            rigidbody.transform.SetParent(transform, true);

            // Alinear su rotaci�n con la plataforma
            rigidbody.transform.localRotation = Quaternion.Euler(Vector3.zero);

            // Calcular la nueva posici�n para mover el centro del BoxCollider
            Vector3 newPosition = rigidbody.transform.localPosition;
            newPosition.y += rigidbody.transform.localScale.y / 2;

            // Mover el centro del BoxCollider hacia el nuevo bloque
            boxCollider.center = newPosition;

            // Desmarcar el bloque para que no sea detectado por otros scripts que usen tags
            rigidbody.gameObject.tag = "Untagged";

            // Invocar el evento pasando la posici�n del �ltimo hijo (�ltimo bloque colocado)
            onBuildingReachPlatform?.Invoke(transform.GetChild(transform.childCount - 1).position);


            // Aquí deberías determinar si el bloque está bien colocado o no
            bool correcto = CheckAlignment(rigidbody.transform.localPosition);
            // Avisar al tutorial que se soltó el bloque
            tutorial.OnBlockDropped(correcto);
        }

        if (collision.gameObject.TryGetComponent<BlockState>(out BlockState state))
        {
            state.yaFueColocado = true;
        }
    }

    // Ejemplo sencillo de CheckAlignment
    private bool CheckAlignment(Vector3 localPos)
    {
        return Mathf.Abs(localPos.x) <= 0.2f;
    }
}
