using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private void Start()
    {
        Instantiate(StartMenu.instance.currentCharacter.prefab, transform.position, Quaternion.identity);
    }
}
