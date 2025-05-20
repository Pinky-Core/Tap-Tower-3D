using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Importa TextMeshPro

public class Tutorial : MonoBehaviour
{

    private bool bloqueProcesado = false;

    private int bloquesCorrectosExtra = 0; // Bloques bien colocados DESPUÉS del primer popup


    public GameObject clickControl;
    public GameObject popupPanel;
    public TMP_Text popupText; // Cambia a TMP_Text

    private string[] tutorialSteps = {
        "¡Bienvenido a Tap Tower!",
        "Presiona la pantalla para soltar el bloque."
    };
    private string approvalMsg = "¡Bien hecho! Apilaste el bloque correctamente.";
    private string failMsg = "¡Ups! Si fallas 3 veces perderás una vida. ¡Intenta apilar bien!";
    private string finalMsg = "¡Buena suerte y construye la torre más alta!";

    private int currentStep = 0;
    private int failCount = 0;
    private bool waitingForDrop = false;
    private bool tutorialFinished = false;

    void Start()
    {
        if (PlayerPrefs.GetInt("TutorialCompletado", 0) == 1)
        {
            // Saltar el tutorial
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            return;
        }

        ShowStep();
    }


    void ShowStep()
    {
        Time.timeScale = 0f;
        popupPanel.SetActive(true);
        popupText.text = tutorialSteps[currentStep];
    }

    public void NextStep()
    {
        currentStep++;
        if (currentStep < tutorialSteps.Length)
        {
            ShowStep();
        }
        else
        {
            popupPanel.SetActive(false);
            Time.timeScale = 1f;
            waitingForDrop = true; // Espera a que el jugador suelte el bloque
            clickControl.SetActive(true);
        }
    }

    public bool IsReadyForDrop()
    {
        return waitingForDrop && !tutorialFinished;
    }

    // Llama este método desde tu lógica de soltar bloque
    public void OnBlockDropped(bool correcto)
    {
        if (!waitingForDrop || tutorialFinished || bloqueProcesado) return;

        bloqueProcesado = true;
        waitingForDrop = false;

        if (correcto)
        {
            if (currentStep >= tutorialSteps.Length) // Ya pasó la parte de los pasos
            {
                bloquesCorrectosExtra++;

                // Si ya colocó 5 bloques bien luego del primero, finalizar tutorial
                if (bloquesCorrectosExtra >= 5)
                {
                    Time.timeScale = 0f;
                    popupPanel.SetActive(true);
                    popupText.text = finalMsg;
                    tutorialFinished = true;

                    PlayerPrefs.SetInt("TutorialCompletado", 1);
                    PlayerPrefs.Save();

                    return; // <-- ESTE return evita que siga y muestre "¡Bien hecho!"
                }
            }

            Time.timeScale = 0f;
            popupPanel.SetActive(true);
            popupText.text = approvalMsg;
            return; // <-- Añadí este también para que no llegue al fail
        }

        // Si falló
        failCount++;
        Time.timeScale = 0f;
        popupPanel.SetActive(true);
        popupText.text = failMsg;
    }



    // Llama este método con un botón "Continuar" en el panel
    public void ContinueAfterDrop()
    {
        if (failCount >= 3)
        {
            popupText.text = "Has perdido 3 veces. ¡Intenta de nuevo!";
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        if (tutorialFinished)
        {
            popupPanel.SetActive(false);
            Time.timeScale = 1f;
            // Aquí empieza el juego normal
            // Podés cargar otra escena si querés
            // SceneManager.LoadScene("JuegoPrincipal");
            return;
        }

        bloqueProcesado = false;
        popupPanel.SetActive(false);
        Time.timeScale = 1f;
        waitingForDrop = true;
    }

}
