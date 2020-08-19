using UnityEngine;

namespace PolyPerfect
{
  public class PlaySound : MonoBehaviour
  {
    [SerializeField]
    private AudioClip animalSound;
    [SerializeField]
    private AudioClip walking;
    [SerializeField]
    private AudioClip eating;
    [SerializeField]
    private AudioClip running;
    [SerializeField]
    private AudioClip attacking;
    [SerializeField]
    private AudioClip death;
    [SerializeField]
    private AudioClip sleeping;

    void AnimalSound()
    {
      if (animalSound)
      {
        AudioManager.PlaySound(animalSound, transform.position);
      }
    }

    void Walking()
    {
      if (walking)
      {
        AudioManager.PlaySound(walking, transform.position);
      }
    }

    void Eating()
    {
      if (eating)
      {
        AudioManager.PlaySound(eating, transform.position);
      }
    }

    void Running()
    {
      if (running)
      {
        AudioManager.PlaySound(running, transform.position);
      }
    }

    void Attacking()
    {
      if (attacking)
      {
        AudioManager.PlaySound(attacking, transform.position);
      }
    }

    void Death()
    {
      if (death)
      {
        AudioManager.PlaySound(death, transform.position);
      }
    }

    void Sleeping()
    {
      if (sleeping)
      {
        AudioManager.PlaySound(sleeping, transform.position);
      }
    }
  }
}