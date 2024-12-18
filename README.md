# SpeakAR

![logo](/Assets/SpeakAR_logo.png)

## About the Project
SpeakAR is an Augmented Reality application that would help users improve their public speaking skills in a portable manner. Through the use of spawnable human models and built in presentation view, this will allow users to simulate talking in front of other people regardless of their location.

## Features
1. **Portability**: 
  - Users can utilize the project anywhere with their mobile device
  - Having the human models adapt to any environments the user is currently in.
2. **Audience Simulation**:
  - Spawn Models: Create virtual humans that simulate an audience on detected spawnable planes.
  - Reposition Models: Allow users to move individual models to desired locations.
  - Scaling Models: Enable resizing of the human models to fit the scene or simulate different audience distances. 
3. **Slides Presentation**:  Users can present using slides in the application in front of the models to simulate a speaking event.

## Current Status of the Project
- The project currently uses free human models from the Unity Asset Store.
- Plane detection was implemented utilizing code from the previous Hands-on outputs.
- Built in slides to aide in presentation.
- The project uses UI designs with user friendly appeal.
- Completed features are model placement, repositioning, scaling, and deleting of the human models.

## Prerequisites
- Unity v2023.X
- OpenJDK installed
- Android SDK & NDK tools installed

## Download
```bash
git clone https://github.com/mark-edison-jim/SpeakAR.git
```
- On Windows, Open Unity Hub and Add from repository the folder that was cloned
- Go into File > Build Settings > Build. This will create an apk.
- After building, transfer the apk file to your mobile device (Android) then install the apk file by opening it.

## Getting Started
Once the application is downloaded on your android, you will be met with a start menu, clicking Start will bring you into the scene with the main functionalities. 
The main UI are as follows:
- A pop-up panel on the bottom left; Includes choice and deletion of moddle and plane toggle. 
- Import button for presentation slides.
- Begin button to start your presentation.

## Basic Controls
1. To spawn a person, first select the model in the pop-up panel.
2. Ensure that you are in a well-lit area, scan the environment until planes with borders start appearing, this will be your spawnable area.
3. Tap on the plane with your model selected to spawn the model at that position.
4. For modification of the spawned model:
  - **Select** the model by tapping on it on the screen.
  - **Reposition** it by dragging the selected model within the planes.
  - **Scale** by using two fingers and doing a pinching gesture on the selected model.
  - **Rotate** by selecting the model and either using two fingers to rotate or drag the hollow circle left and right that appears when selecting a model.

## Troubleshooting
**Plane Detecting Issues:**
- Ensure your environment is well-lit.
- Generally it will take some time for proper planes to spawn.
- Check for any obstructions that might block the camera's view.

## Contact Information
For any more problems, these are the links that might help
 - [Unit AR Discussion](https://discussions.unity.com/t/augmented-reality/388590)
- [Stack Overflow](https://stackoverflow.com/)

## Screenshots
![pic](/Assets/SpeakAR_pic.png)
![pic2](/Assets/SpeakAR_pic2.png)