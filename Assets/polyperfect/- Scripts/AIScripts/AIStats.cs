using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animal Stats", menuName = "LowPolyAnimals/NewAnimalStats", order = 1)]
public class AIStats : ScriptableObject
{
    [SerializeField, Tooltip("How dominent this animal is in the food chain, agressive animals will attack less dominant animals.")]
    public int dominance = 1;

    [SerializeField, Tooltip("How many seconds this animal can run for before it gets tired.")]
    public float stamina = 10f;

    [SerializeField, Tooltip("How much this damage this animal does to another animal.")]
    public float power = 10f;

    [SerializeField, Tooltip("How much health this animal has.")]
    public float toughness = 5f;

    [SerializeField, Tooltip("Chance of this animal attacking another animal."), Range(0f, 100f)]
    public float agression = 0f;

    [SerializeField, Tooltip("How quickly the animal does damage to another animal (every 'attackSpeed' seconds will cause 'power' amount of damage).")]
    public float attackSpeed = 0.5f;

    [SerializeField, Tooltip("If true, this animal will attack other animals of the same specices.")]
    public bool territorial = false;

    [SerializeField, Tooltip("Stealthy animals can't be detected by other animals.")]
    public bool stealthy = false;
}
