https://github.com/user-attachments/assets/a780592b-cf4f-4111-9247-6f7dc7f6daf2

# Unity Boid Simulation
A simple implementation of Craig Reynolds' Boids algorithm for flocking behavior, built entirely within Unity using C#. This project demonstrates the classic three rules of flocking (Separation, Alignment, Cohesion) managed by a single centralized simulation script.

## What are Boids?
"Boids" is an artificial life program, developed by Craig Reynolds in 1986, which simulates the flocking behaviour of birds. The core idea is that complex global flocking behavior can emerge from simple local rules followed by individual agents (the "boids").

This simulation implements the three fundamental rules described by Reynolds:
1.  **Separation:** Steer to avoid crowding local flockmates. Each boid tries to maintain a minimum distance from its immediate neighbors.
2.  **Alignment:** Steer towards the average heading of local flockmates. Boids try to match the velocity of their nearby neighbors.
3.  **Cohesion:** Steer to move towards the average position (center of mass) of local flockmates. Boids try to stay close to the group.


## Getting Started

### Prerequisites
* [Unity Hub](https://unity.com/download)
* Unity Editor (Developed with version 6.1.0, but likely compatible with most recent LTS versions)
* Git LFS

### How to Use
1.  **Clone or Download:** Get a copy of this repository.
    ```bash
    git clone <your-repository-url>
    ```
2.  **Git LFS Initilization:** CD into the projects root directory and initialize git lfs
    ```bash
    git lfs install
    ```
3.  **Open Project:** Launch Unity Hub, click "Add", and navigate to the cloned project folder. Select it to add it to your Hub projects list, then open it.
4.  **Run:** Press the Play button in the Unity Editor. You should see the boids spawning and flocking!

## Configuration
All simulation parameters can be adjusted in the Unity Inspector by selecting the GameObject that has the `BoidSimulation.cs` script attached:

* **Dependencies:**
    * `Boid Sprite`: The sprite used to visually represent each boid.
* **General Settings:**
    * `Number Of Boids`: Total number of boids to simulate.
    * `Arena X Bounds`/`Arena Y Bounds`: Half-width and half-height of the simulation area. Boids will wrap around these boundaries.
* **Boid Settings:**
    * `Min Speed`/`Max Speed`: The speed range the boids will adhere to.
    * `Perception Radius`: How far a boid can "see" neighbors for Alignment and Cohesion.
    * `Separation Radius`: The distance within which a boid actively tries to move away from neighbors. Should be smaller than Perception Radius.
    * `Max Steer Force`: Limits how quickly a boid can change direction, affecting its agility.
* **Rule Weights:**
    * `Separation Weight`/`Alignment Weight`/`Cohesion Weight`: Control the influence of each respective rule on the boid's movement. Experimenting with these is key to achieving different flocking styles.
    * `Boundary Weight`: How strongly boids steer away from the edges (used by the boundary steering calculation, complements wrapping).
