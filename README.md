# Project: Augmented Reality Particle Physics Visualizer

## 1. Overview

This is a project developed in **Unity** that utilizes **Augmented Reality (AR)** technology
via the **Vuforia Engine** to create an educational and interactive experience about the
Standard Model of Particle Physics.
The main goal is to allow users to visualize fundamental and composite particles,
understand their properties, and observe simulations of fundamental interactions of
nature, all by overlaying digital content onto the real world through physical markers.

## 2. Main Features

```
● AR Visualization: Uses image markers to instantiate and anchor visual
representations of particles in the user's environment.
● Dynamic Data Panel: Each particle is accompanied by an "InfoCard" that
displays its fundamental properties (mass, charge, spin, etc.) in a clear and
aesthetically pleasing way.
● Procedural Animations: The particles are not static. They feature animations
that reflect their nature:
○ Fundamental Particles (Leptons): Smooth rotation and pulse animations
that vary according to their mass and stability.
○ Hadrons (Protons, Neutrons): Visual simulation of a composite system, with
internal quarks orbiting chaotically and bound by a gluon field.
● Interaction Simulation: The system is capable of detecting the simultaneous
presence of multiple markers to simulate fundamental interactions:
○ Strong Force: Gluon exchange between quarks and the formation of hadrons
(Protons and Neutrons) when the correct combination of quarks (u,u,d or
d,d,u) is presented.
```
## 3. Project Architecture

The project is built on a **data-driven architecture** to ensure maximum organization,
code reusability, and ease of expansion.
**Data Layer: ScriptableObjects**
The heart of the system. Instead of hardcoding particle data in scripts, we use
ScriptableObjects to create a flexible database.
● **ParticleData.cs** : This is the "template" for any particle. It contains all the
information:


○ Physical Data: particleName, particleSymbol, mass, electricCharge, spin.
○ Categorization: ParticleType (Quark, Lepton, Boson) for interaction logic.
○ Visual Style: frameColor for the UI, corePrefab for the 3D model, and
coreMaterial for its appearance.
**View Layer: Smart Prefabs**
The appearance of each particle is controlled by a single, powerful Prefab.
● **ParticleInfoCard_Prefab** : This is the visual template. It doesn't know _what_ an Up
Quark is, it only knows _how to display_ the data it receives.
● **ParticleUIController.cs** : The "brain" of the prefab. This script holds references to
all UI elements (texts, images) and an anchor point for the 3D model. Its main
function, SetData(ParticleData data), receives a ParticleData asset and populates
the entire UI, instantiating the correct 3D model.
● **Object Pooling:** For maximum performance, the ParticleUIController
pre-instantiates all possible 3D models at the start and simply
activates/deactivates them, avoiding the performance cost of Instantiate and
Destroy during runtime.
**Logic Layer (Controller): InteractionManager**
This is the orchestra's conductor. It's a Singleton script that centralizes all interaction
logic.
● **InteractionManager.cs** :
○ Maintains a list of all currently visible particles (markers).
○ Constantly checks if the conditions for an interaction have been met (e.g., 2
visible quarks for gluon exchange; 3 specific quarks to form a hadron).
○ Starts the animation coroutines for the interactions (e.g., AnimateGluon,
AnimateGluonCycle).
**AR Layer: Vuforia**
Vuforia serves as the real-world input layer.
● **ImageTarget** : Each marker in the scene is an ImageTarget. It serves as the
"trigger" and "anchor" for our content.
● **Connection:** The link between Vuforia and our system is made directly in the
Unity editor, where an instance of the ParticleInfoCard_Prefab is placed as a child
of an ImageTarget and configured with the corresponding ParticleData asset.

## 4. How to Add a New Particle

The architecture makes expanding the project extremely simple:


1. **Create the Data:** In the ScriptableObjects/Particles folder, right-click and go to
    Create > Particle System > Particle Data. Name the new file (e.g., Muon_Data).
2. **Fill in the Data:** Select the new file and fill in all its properties in the Inspector
    (name, mass, Lepton type, core prefab, material, etc.).
3. **Create the Trigger:** Add a new ImageTarget to your scene and configure it with
    the corresponding marker image.
4. **Connect Everything:** Drag the ParticleInfoCard_Prefab to be a child of the new
    ImageTarget. In the ParticleInfoCard's Inspector, drag your new Muon_Data asset
    into the appropriate field in the script.
Done! The new particle is now fully integrated into the system.

## 5. Dependencies

```
● Unity 2020.x or higher
● Vuforia Engine (installed via Package Manager)
● TextMeshPro (essential for UI)
```
## 6. Next Steps & Future Ideas

```
● Implement Weak Force interactions (particle decay).
● Implement Electromagnetic interactions (matter-antimatter annihilation,
scattering).
● Add a "Sandbox Mode" where the user can select and instantiate particles
without needing markers.
● Create more complex UI animations for the appearance of the InfoCards.
```

