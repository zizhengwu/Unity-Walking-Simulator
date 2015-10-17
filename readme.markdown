---
layout: post
title:  "Milestone 2: Physics Simulation"
date:   2015-10-16 20:56:19
---
# Milestone 2: Physics Simulation

Zizheng Wu || me@zizhengwu.com || http://www.prism.gatech.edu/~zwu307/cs-6457/

## Requirements

My project meets with all the requirements.

### Five unique actors

![](images/milestone-2-physics_simulation/actors_overview.jpg)

The above is an overview of my scene. In this image, I have included `Character`, `Bonfire`, `House`, and the `Amusement park`.

![](images/milestone-2-physics_simulation/amusement_park.jpg)

This is the amusement park. The `Boxes` can also be interated with.

### Two compound objects consisting of joints

![](images/milestone-2-physics_simulation/door.jpg)

The door is created with `Hinge Joint`.

![](images/milestone-2-physics_simulation/lantern.jpg)

The lantern is created with `Hinge Joint` as well.

### Variable height terrain

![](images/milestone-2-physics_simulation/bird_eye.jpg)

Apparantly, my amusement park is abundant with variable heights.

### At least three material sounds

![](images/milestone-2-physics_simulation/sound.jpg)

The running sounds will differ while you are on the ground or the obstacles. Besides, there is collision sound when you run into the boxes, sound of the rain, and sound of the bonfire.

### Game feel

![](images/milestone-2-physics_simulation/to_light_bonfire.jpg)

The bonfire can be lit by pressing `E` when you are around it.

## Resources

* Standard Assets

https://www.assetstore.unity3d.com/en/#!/content/32351

I grabbed the ground obstacles, third person camera control, and the animation controller. 

* PUSH WALL HOUSE (ORIGINAL) ERNIE DAVIS © MAY07

https://3dwarehouse.sketchup.com/model.html?id=7ee5c13333fbc91ce63b7aa48daf8ba1

The `House` model of my scene.

* Woods! Forest Skybox Pack Vol.I

https://www.assetstore.unity3d.com/en/#!/content/23806

The `skybox` of my scene.

* Rain Maker Weather System

https://www.assetstore.unity3d.com/en/#!/content/34938

The implementation of `Rain` of my scene.

* Campfire

https://www.assetstore.unity3d.com/en/#!/content/45038

The model of `Bonfire` of my scene. The script to light it is written by myself.

## Install instructions

`Assets\Scenes\untitled.unity`

## Steps to experience my game

| Controls | Actions |
| - | - |
| mouse | camera only |
| wsad | move |
| spacebar | jump |


![](images/milestone-2-physics_simulation/door.jpg)

Try opening the door.

![](images/milestone-2-physics_simulation/sound.jpg)

Listen to the sound of running on different surfaces.

![](images/milestone-2-physics_simulation/to_light_bonfire.jpg)

Press `E` to light the bonfire.

![](images/milestone-2-physics_simulation/bump.jpg)

Bump into the `Box`. There will a sound of collision.

# Known bugs

The display of light bonfire menu relies on `OnCollisionStay`. It is not a perfect solve.

Besides, it might need more than one press to light the bonfire. I am looking into it.

# URL
Click [this link](http://www.prism.gatech.edu/~zwu307/cs-6457/assets/milestone-2-physics_simulation/build.html) to play. 
[Link](http://www.prism.gatech.edu/~zwu307/cs-6457/2015/10/16/milestone-2-physics_simulation.html) to this markdown.