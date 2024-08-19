using System.Collections.Generic;
using UnityEngine;

class ParticleSystemEvent : MonoBehaviour
{
    public delegate void ParticleSystemEventDelegate();
    public ParticleSystemEventDelegate particleSystemEventDelegate;
    public delegate void ParticleSystemCollisionDelegate(GameObject other);
    public ParticleSystemCollisionDelegate particleSystemCollisionDelegate;
    private new ParticleSystem particleSystem;
    private List<ParticleCollisionEvent> particleCollisionEvents = new List<ParticleCollisionEvent>();
    public ParticleSystem ParticleSystem { get => particleSystem; set => particleSystem = value; }

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void OnParticleSystemStopped() 
    {
        particleSystemEventDelegate?.Invoke();
    }

    private void OnParticleCollision(GameObject other) 
    {
        int totalCollision = particleSystem.GetCollisionEvents(other, particleCollisionEvents);
        Debug.Log("totalCollision: " + totalCollision);
        particleSystemCollisionDelegate?.Invoke(other);
    }
    
}