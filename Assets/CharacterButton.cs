using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public Image characterImage; // Image du personnage sur le bouton
    public Sprite idleSprite; // Sprite statique par défaut
    public Sprite[] movementSprites; // Sprites pour l'animation
    public float animationSpeed = 0.1f; // Vitesse de l'animation

    private bool isSelected = false; // État du bouton (sélectionné ou non)
    private int currentSpriteIndex = 0; // Index du sprite actuel pour l'animation
    private float animationTimer = 0f; // Timer pour gérer le rythme de l'animation

    // Initialisation (appelée par le CharacterSelectionManager)
    public void Initialize()
    {
        characterImage.sprite = idleSprite; // Définir le sprite initial
        isSelected = false; // Désactive l'état sélectionné
    }

    // Action déclenchée lorsqu'on clique sur le bouton
    public void OnButtonClicked()
    {
        CharacterSelectionManager.Instance.DeselectAll(); // Désélectionne les autres boutons
        isSelected = true; // Définit ce bouton comme sélectionné
        CharacterSelectionManager.Instance.SetSelectedCharacter(this); // Met à jour la sélection
    }

    // Fonction appelée à chaque frame
    void Update()
    {
        if (isSelected && movementSprites.Length > 0)
        {
            AnimateCharacter(); // Lance l'animation si le bouton est sélectionné
        }
    }

    // Animation du personnage (basée sur les sprites)
    private void AnimateCharacter()
    {
        animationTimer += Time.deltaTime;
        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f; // Réinitialise le timer
            currentSpriteIndex = (currentSpriteIndex + 1) % movementSprites.Length; // Passe au sprite suivant
            characterImage.sprite = movementSprites[currentSpriteIndex]; // Met à jour l'image
        }
    }

    // Désélectionne ce bouton
    public void Deselect()
    {
        isSelected = false; // Désactive l'état sélectionné
        characterImage.sprite = idleSprite; // Revient à l'image statique
    }
}
