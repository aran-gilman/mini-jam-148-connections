using UnityEngine;

public class PersistentSingleton : MonoBehaviour
{
    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag(tag).Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
