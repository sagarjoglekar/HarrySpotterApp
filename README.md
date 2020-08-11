# Harry Spotter
<a href="https://social-dynamics.net/"><img src="https://github.com/sagarjoglekar/Maslow-s-Objects/blob/master/Readme%20Files/Nokia%20Logo.png?raw=true" title="Nokia Bell Labs" alt="SoicalDynamics" align="left" height="40" width="400"></a> <br /> <br />

Harry Spotter is a crowdsourcing gamified platform that allows users to annotates objects in the real world space. The platform makes the process of object scanning interesting to the users by incorporating multiple gamification strategies to the application such as reward, leaderboard, places of interest which the user can go and explore and mayorship of places.

Available for both iOS and Android

This project is being developed using [Unity](https://unity.com/) version 2019.4.6f1(LTS).

[![Build Status](http://img.shields.io/travis/badges/badgerbadgerbadger.svg?style=flat-square)](https://github.com/sagarjoglekar/Maslow-s-Objects)
---
## Table of Contents
[Installation](#installation)

[ProjectMap](#ProjectMap)

---
## Installation
1- In order to run the project first you need to install Unity.

Download [Unity Hub](https://unity3d.com/get-unity/download) and install it. 
In Unity Hub go to -> Installs -> ADD -> Install **Unity 2019.4.6f1 (LTS) Version**

After that you need to select platfroms build support for Unity, make sure that you select the following
- Andriod Build Support (Required)
  - Click on the drop down button next to **Andriod Build Support** and select 
    - **Andiod SDK & NDK Tools** (Required)
    - **OpenJDK** (required)
- iOS Build Support (Required)
- Mac Build Support (Optional)
- WebGL Build Support (Optional)
- Windows Build Support (Optional)
> Refer to the picture below

![alt text](https://github.com/sagarjoglekar/Maslow-s-Objects/blob/master/Readme%20Files/Unity%20Platforms.png?raw=true) 

2- Cloning Process

The project includes files that are larger than 100mb Github limit size. You need to add these files manually.

**Manually Adding the large File:**
**Before opening the project in Unity,** you need to add the following files to the project directory manually. These files are stored in the project dropbox folder.
2020_ar -> Project Large Files (Gihub) -> 
- ArtifactDB
- Vuforia

![alt text](https://github.com/Nima-Jamalian/Harry-Spotter/blob/master/Readme%20Files/Extra%20Files.png?raw=trueraw=true)

You need to place each file in the following directories:
```
ArtifactDB -> Harry Spotter/Library/ArtifactDB

Vuforia -> Harry Spotter/Library/PackageCache/com.ptc.vuforia.engine@9.2.8/Vuforia/Plugins/iOS/Vuforia.framework/Vuforia

```

Note: A .gitignore file has been added to the repository which is responsible for telling the Github to ignore most of the changes to Unity internal files and large file. In order to view the git ignore file, you need to turn on the option to view hidden files.

macOS shortcut -> command + shift + . 

3- Opning the project with Unity

Open UnityHub -> Projects -> ADD -> Nagivate to **Harry Spotter** Direcotry -> Open

Then the project will get added to your project section in UnityHub. Simply click on **Harry Spotter** and Unity will open up the project.

![alt text](https://github.com/Nima-Jamalian/Harry-Spotter/blob/master/Readme%20Files/Harry%20Spotter%20Project.png?raw=true?raw=true)

---
## ProjectMap
The Unity project file is located at **Harry Spotter**

All project main files are located in the Assets folder.

