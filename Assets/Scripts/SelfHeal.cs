using UnityEngine;

[RequireComponent(typeof(Health))]
public class SelfHeal : MonoBehaviour
{
    [SerializeField]
    [Tooltip("After taking damage, disable healing for X seconds.")]
    private float _damageCooldown = 5.0f;

    [SerializeField]
    [Tooltip("If not at full health and not on damage cooldown, apply healing every X seconds.")]
    private float _healInterval = 1.0f;

    [SerializeField]
    [Tooltip("How much health to restore per healing instance.")]
    private float _healAmount = 1.0f;
}
