using System.Threading.Tasks;
using UnityEngine;

// Asegura que el GameObject tenga un LineRenderer
[RequireComponent(typeof(LineRenderer))]
public class HookControl : MonoBehaviour
{
    // Grosor de la cuerda
    [SerializeField, Range(.01f, 1)] private float lineWidth = .25f;

    // Punto desde donde cuelga la cuerda
    [SerializeField] private Rigidbody pivot;

    // Gancho donde se instancia el bloque
    [SerializeField] private Rigidbody hook;

    // Tiempo entre generaci�n de bloques
    [SerializeField] private float countDownIntancer = 1;

    // Prefabs de los edificios/bloques que se van a generar
    [SerializeField] private Rigidbody[] buildings;

    // Offset desde el nuevo punto de pivote
    [SerializeField] private Vector3 pivotOffset = 5 * Vector3.up;

    // Duraci�n de la transici�n al mover el gancho
    [SerializeField] private float rePositionDuration = 1;

    // Referencia al bloque actual instanciado
    private Rigidbody buildingBody;

    // L�nea que conecta el pivot y el gancho
    private LineRenderer line;

    // Tiempo en el que se gener� el �ltimo bloque
    private float lastInstance = 0;

    private void Awake()
    {
        // Configuramos el LineRenderer
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;

        // Posicionamos el gancho al principio
        transform.position = pivotOffset;
    }

    private void LateUpdate()
    {
        // Ajustamos el grosor de la l�nea
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        // Dibujamos la l�nea entre el pivote y el gancho
        line.SetPosition(0, pivot.position);
        line.SetPosition(1, hook.position);

        // Intentamos instanciar un nuevo bloque
        InstanceBuilding();
    }

    // Suelta el bloque actual (lo deja caer)
    public void DetachBuilding()
    {

        if (buildingBody != null)
        {
            buildingBody.isKinematic = false;
            buildingBody.constraints = RigidbodyConstraints.None;
            buildingBody.transform.SetParent(null);
            lastInstance = Time.time;

            buildingBody = null;
        }
    }


    // Instancia un nuevo bloque en el gancho si no hay uno ya colgado
    private void InstanceBuilding()
    {
        // Generador de n�meros aleatorios con semilla basada en el tiempo
        Random.InitState((int)Time.time);

        // Si no hay bloque actual y ha pasado el cooldown...
        if (!buildingBody && (Time.time - lastInstance) > countDownIntancer)
        {
            // Instanciamos un nuevo bloque como hijo del gancho
            buildingBody = Instantiate<Rigidbody>(buildings[Random.Range(0, buildings.Length)], hook.transform, true);
            buildingBody.isKinematic = true; // Sin f�sica mientras cuelga
            buildingBody.transform.localPosition = Vector3.down; // Posicionado justo debajo del gancho
        }
    }

    // Actualiza la posici�n del gancho (por ejemplo cuando sube la torre)
    public void UpdatePivotPosition(Vector3 pos)
    {
        pos.x = 0;
        pos.z = 0;

        // Movemos el gancho suavemente con una corrutina async
        UpdatePositionTask(rePositionDuration, pos + pivotOffset);
    }

    // Corrutina async para interpolar la posici�n del gancho con suavidad
    private async void UpdatePositionTask(float duration, Vector3 endValue)
    {
        float time = 0;
        Vector3 startValue = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            await Task.Yield(); // Esperamos un frame
        }
        transform.position = endValue; // Posici�n final asegurada
    }
    
    private bool IsPlacementCorrect(Vector3 position)
    {
        // Ejemplo simple: si cae cerca del centro, se considera correcto
        return Mathf.Abs(position.x) < 1f;
    }

}
