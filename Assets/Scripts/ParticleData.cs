using UnityEngine;

public enum ParticleType { Quark, Lepton, Boson, Hadrons }
public enum ParticleFamily { Fermion, Boson, Hadrons } 

[CreateAssetMenu(fileName = "ParticleData", menuName = "Particle/ParticleData")]
public class ParticleData : ScriptableObject
{
    public ParticleType particleType;
    public ParticleFamily particleFamily;
    public string particleName;
    public string symbol;
    public string energy;
    public string mass;
    public string spin;
    public GameObject model;
    public Color color;
}
