using UnityEngine;

/// <summary>
/// On collision, deals damage to objects of the opposite faction.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DamageInstance : MonoBehaviour
{
    [SerializeField]
    private float _attackPower = 1.0f;

    // Faction is set by the instantiating object at runtime, so
    // this must be non-serialized but still public.
    public FactionAlignment.EFaction OwningFaction { get; set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        FactionAlignment otherFaction = other.GetComponentInParent<FactionAlignment>();
        if (otherFaction != null && otherFaction.Faction == OwningFaction)
        {
            return;
        }

        Health otherHealth = other.GetComponentInParent<Health>();
        if (otherHealth == null)
        {
            return;
        }

        otherHealth.TakeDamage(_attackPower);

        // TODO: Disable instead of destroying and set up object pooling.
        Destroy(gameObject);
    }
}
