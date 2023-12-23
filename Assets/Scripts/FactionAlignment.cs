using UnityEngine;

public class FactionAlignment : MonoBehaviour
{
    public enum EFaction
    {
        Player,
        Enemy
    }
    [SerializeField]
    private EFaction _faction;
    public EFaction Faction => _faction;
}
