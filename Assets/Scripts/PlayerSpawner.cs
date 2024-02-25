using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private void Awake()
    {
        Instantiate(StartMenu.instance.currentCharacter.prefab, transform.position, Quaternion.identity);
    }
}
