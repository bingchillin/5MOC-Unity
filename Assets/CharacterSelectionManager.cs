using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance; // Instance globale pour un accès facile

    public Button playButton; // Référence au bouton "Play"
    public CharacterButton buttonS; // Référence au bouton Skeleton
    public CharacterButton buttonZ; // Référence au bouton Zombie

    private CharacterButton selectedCharacter; // Stocke le personnage actuellement sélectionné

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeButtons(); // Initialise les boutons au démarrage
    }

    // Initialise tous les boutons
    private void InitializeButtons()
    {
        buttonS.Initialize();
        buttonZ.Initialize();
        playButton.interactable = false; // Désactive le bouton "Play" au démarrage
    }

    // Définit le bouton sélectionné
    public void SetSelectedCharacter(CharacterButton character)
    {
        selectedCharacter = character; 
        playButton.interactable = true; 
    }

    // Désélectionne tous les boutons
    public void DeselectAll()
    {
        buttonS.Deselect();
        buttonZ.Deselect();
    }
}
