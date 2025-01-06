using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public Button playButton; // Référence au bouton "Play"

    private void Start()
    {
        playButton.interactable = false; // Désactiver le bouton au démarrage
    }

    public void SetInteractable(bool state)
    {
        playButton.interactable = state; // Activer ou désactiver le bouton
    }

   public void OnPlayButtonClicked()
    {
       
        SceneManager.LoadScene("EnterScene");
    }
}
