# HoloSplat · 3DGS Robot Digital Twin

**Spatial registration, Gaussian scene presentation, and ROS-driven robot state mirroring for HoloLens 2 interaction workflows.**

This core-code repository brings together robot state mapping and a registration/presentation layer for 3D Gaussian Splatting environments. The static reconstructed scene and the dynamic mobile manipulator are separate layers sharing a calibrated map frame.

## Architecture

```text
3DGS reconstruction → Unity Gaussian renderer → Gaussian scene root
                                                 ↑ shared registration
ROS /odom + /joint_state → robot state mapping → robot map root
```

## Core modules

- `Runtime/Robot/ros_loc.cs`: maps ROS odometry to the virtual mobile base position and heading.
- `Runtime/Robot/ros_arm.cs`: maps six joint values to the articulated robot model.
- `Runtime/Registration/GaussianSceneRegistration.cs`: planar landmark registration with optional uniform scale; aligns reconstruction and robot map roots.
- `Runtime/Gaussian/GaussianLayerController.cs`: opacity, spherical-harmonics order, visibility, and sorting-frequency controls for the Gaussian renderer.

## Integration

1. Use Unity 2022.3 with MRTK 2.8 and the Microsoft Mixed Reality OpenXR integration for HoloLens interaction. Import [Unity ROS-TCP-Connector](https://github.com/Unity-Technologies/ROS-TCP-Connector).
2. Install [UnityGaussianSplatting](https://github.com/aras-p/UnityGaussianSplatting), matching the renderer API used by the local 0.9.1 package. Create a Gaussian asset from your reconstructed scene and configure its renderer and shaders using the upstream importer.
3. Place the Gaussian scene and robot model under independent roots. Keep their parent transforms at unit scale. Assign both roots to `GaussianSceneRegistration`; provide matching model-space and world-space floor-plane landmarks. Units are meters; optional scaling applies to both map layers.
4. Assign the base object to `ros_loc.Robot` and the six robot links to `ros_arm`. The preserved base mapping uses `(ROS y, -3.93, ROS x)` and negates heading; adjust the vertical offset and joint zero offsets for the installed robot.
5. ROS topics: `/odom` uses `nav_msgs/Odometry`. `/joint_state` uses the project's six-value `geometry_msgs/Twist` contract, with joint values in degrees and the model-specific axis/zero offsets in `ros_arm`.

The Gaussian rendering host and graphics API must satisfy the upstream renderer's requirements. HoloLens interaction can use the project's host/remoting arrangement; renderer configuration is separate from robot-state synchronization.

## Scope

Selected runtime sources only. Reconstruction assets, point clouds, robot meshes, imported SDKs, Unity caches, scenes, and application builds are kept outside this repository.

## Dependencies and credit

Gaussian rendering is supplied by Aras Pranckevičius's MIT-licensed UnityGaussianSplatting; ROS communication is supplied by Unity Robotics ROS-TCP-Connector. This repository contains the spatial registration and robot application integration layers.

MIT license applies to the application sources in this repository. External packages retain their own licenses.
