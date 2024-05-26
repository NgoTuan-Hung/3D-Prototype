using UnityEngine;

class ParticleSystemEvent : MonoBehaviour
{
    public delegate void ParticleSystemEventDelegate();
    public ParticleSystemEventDelegate particleSystemEventDelegate;
    private ParticleSystem particleSystem;
    public ParticleSystem ParticleSystem { get => particleSystem; set => particleSystem = value; }

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void OnParticleSystemStopped() 
    {
        particleSystemEventDelegate?.Invoke();
    }
}