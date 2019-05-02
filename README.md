# Contractor City . *Playable Prototype*  

**The Goal Diggers** @ Centre for Digital Media, 2019 Jan. - Apr.  
**Project Manager** Sam Stumborg  
**Lead Programmer** Zhengyang (Peter) Pan  
**Lead Artist** Rubing (Sophie) Bai  
**Designer**  Dafne Delgado  
**Programmer (UBC)**  Mikayla Preete  
**Faculty Advisor** Yangos Hadjiyannis  

## App Video
[YouTube](https://youtu.be/w3EtpS5PS48)

## Project Video
[YouTube](https://youtu.be/9Q_TEErzt_E)

## Agile statement

For 18-30-year-old contractors with a smartphone who aren’t aware of BC One Call or aren’t used to calling. The idle game, Contractor City is a low-pressure incremental game, whose mechanics are built on BC1C’s principles that encourage young contractors to use the One Call system. Unlike other mobile idle games or video games in general, ours rewards users for using the One Call system, relates directly to the player’s daily lives, and aims to bring about behavioral change.

## X-statement

The serotonin boosts and constant growth of AdVenture Capitalist meets the progress and
discovery of DevStory.

Engagement, through repeated exposure and rewards, show that proper procedures and a corporate culture of safety mitigates costly delays and accidents.


## Introduction

The Goal Diggers are a five-person team of students established January 7th, 2019 by four members from the Master of Digital Media program’s Cohort 13 at the Centre for Digital Media and one student from Bachelor of Computer Science (BCS) student at UBC. The Goal Diggers are working with the client, BC One Call (BC1C) and their member organization, FortisBC with Yangos Hadjiyannis as their Faculty Advisor.
Ian Turnbull and Michelle Petrusevich approached the CDM with one objective in mind: decrease the number of times people strike a line per year. Starting in January 2019 they worked in close coordination with The Goal Diggers to further define the issue and develop a solution.

## Problem

The metrics Michelle and Ian use are number of calls to BC1C per year vs the number of line strikes. Increased construction activity means an increase to both, but The Goal Diggers were tasked with a way of improving this ratio. With greater consultation and research it became clear the majority of line strikes were being committed by a small number of repeat offenders (unofficially called the frequent fliers club). Though they were causing the most hits, and therefore had the most potential good to come from behavioural change and a shift to safe ground disturbance practices, the difficulty in changing the minds of aged stuck-in-their-ways contractors with a digital artifact became clear. Instead, our focus shifted to new contractors just joining the field who haven’t been exposed to BC1C’s message or services.

## Solution

To target younger contractors, reinforce safe digging practices, bring about behavioural change, and benefit the line strike metrics The Goal Diggers proposed Contractor City as a solution. A casual, free to play, idle game for mobile Contractor City was built to engage the younger contractors in an easy and rewarding manner. Built upon the principles of BC1C it rewards safe practices, punishes line strikes, and reinforces the value that BC1C brings to the construction industry.
Idle games are among the best selling games for mobile with the highest retention rates. Impossible to lose, these incremental games are all about rewards which the Goal Diggers tied directly to practicing safe digging in the game and –by including an option to redeem BC1C ticket numbers– in real life as well. The game is casual enough that the players can check it for a minute at a time, several times a day, and not have it interfere with their work or on-site safety. Lastly, by turning real ticket numbers into in- game rewards we’re incentivizing the use of the BC1C system which helps Ian and Michelle’s metrics.


## Platform

The Playable Prototype is developed for Android Platform (Version 8.1.0), with a screen resolution of 1080x2220. The prototype is being developed and tested on client provided hardware: Samsung Galaxy A8 (Model SM-A530W).Further development can bring this product to many other platforms, including iOS and Web.

## Engine

The prototype is developed in Unity, ver. 2018.3.4f1, with source code being written in C#. The team selected the Unity engine because of team members’ experience. Unity is fast to learn and easy to use, and its engaging community brings us great convenience in finding references for assets and code. Also, by utilizing Unity, the team is ultimately developing for almost any current generation platform. Though the prototype for this project is restricted to the Android platform for now, it will save effort if the project is to be migrated to other platforms like web and iOS.

## Plug-In

The following plug-ins were utilized during the playable prototype development:

[DOTween](http://dotween.demigiant.com/)
> DOTween is a fast, efficient, fully type-safe object-oriented animation engine for Unity, optimized for C# users, free and open-source, with tons of advanced features.

[Spine-Unity](http://esotericsoftware.com/spine-unity)
> The spine-unity runtime provides functionality to load, manipulate and render Spine skeletal animation data in Unity engine.

## Third Party Assets

The following third-party assets were purchased and utilized under license during the playable prototype development:

[Epic Victory Effects by Mixture Art, v 1.0](https://assetstore.unity.com/packages/vfx/particles/epic-victory-effects-88115)

[Water Effect Sprites by Photon Blasting Service, v 1.1](https://assetstore.unity.com/packages/vfx/particles/environment/water-effect-sprites-3870)

[FX Smoke Effects by FX_Kandol_Pack, v 1.1](https://assetstore.unity.com/packages/vfx/particles/fx-smoke-effects-24332)

## Code Structure

The playable prototype generally follows a Model-View-Controller software design pattern. While working with Unity, the UI elements designed in it can be seen as the “View” part of MVC pattern; the code mainly consists of data models and “view controllers” for every scene/panel in Unity.
