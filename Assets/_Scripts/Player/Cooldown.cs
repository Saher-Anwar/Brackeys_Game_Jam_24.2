using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour {

    [Header("Cooldown Settings")]
    [SerializeField] private float cooldownTime;
    private float nextUseTime;

    // Property to check if the cooldown is active and function to reset the cooldown
    public bool isCooldown => Time.time < nextUseTime;
    public void Use() => nextUseTime = Time.time + cooldownTime;

}
