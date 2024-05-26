# AR Room Simulator 🪑

AR Room Simulator is a Unity-based application designed to create an augmented reality environment for simulating room layouts. This tool allows users to place and manipulate 3D objects within a real-world space using AR technology. It's ideal for interior designers, architects, or anyone looking to visualize how furniture and decor will look in a given room.

## ✍ Features

- **Spatial Recognition**: Creates augmented reality objects by recognizing planes in reality with a camera.
- **Object Placement**: Add and place 3D objects within the augmented reality environment.
- **Object Recognition(Select)**: Select an object created within the augmented reality environment.
- **Object Manipulation**: Move, rotate, and resize the selected object as desired.
- **Object Occlusion**: AR objects naturally disappear when in an invisible area, creating a more realistic and deep expression.

## ⭐ Getting Started

### Prerequisites

- **Unity**: Version 2022.3.29f1
  - **important!!** If the Unity version is different, it will not run.
- **AR Foundation**: Version 5.1.4
  - Unity's AR Foundation package for AR functionality.

#### For Android

- **Google ArCore XR Plugin**: Version 5.1.4
  - Always same as AR Foundation version
  - Package to enable ARCore support in AR Foundation projects

### Installation

1. **Clone the Repository**:

```bash
git clone https://github.com/nodb/ARRoomSimulator.git
```

2. Open the Project in Unity:

- Open Unity Hub.
- Click on "Add" and navigate to the cloned repository folder.
- Select the project and open it.

3. Install Required Packages:

- In Unity, go to Window > Package Manager.
- Ensure AR Foundation, Google ArCore XR Plugin (for Android) are installed.

## 📁 Scripts

### 📄 ARMultipleObjectController.cs

This script is responsible for managing multiple AR objects within the AR environment. It allows users to place, select, and manipulate 3D objects using touch input.

#### Object Placement Method

1. Save the prefab of the pressed button each time the button is pressed.
2. Instantiate with saved Prefab

- **Serialized Fields**:
  - `ARRaycastManager arRaycastManager`: Manages AR raycasting to detect surfaces in the AR environment.
  - `Camera arCamera`: The AR camera used to cast rays for detecting touch positions.
- **Private Fields**:

  - `GameObject selectedPrefab`: The prefab to instantiate when placing new AR objects.
  - `ARObject selectedObject`: The currently selected AR object.
  - `List<ARRaycastHit> arHits`: Stores AR raycast hit results.
  - `RaycastHit physicsHit`: Stores physics raycast hit results.
  - `float scale`: Stores the current scale factor for AR objects.
  - `float angle`: Stores the current rotation angle for AR objects.

- **Public Methods**:

  - `void SetSelectedPrefab(GameObject selectedPrefab)`: Sets the prefab to be instantiated.
  - `void UpdateScale(float sliderValue)`: Updates the scale of the selected AR object based on slider input.
  - `void UpdateRotation(float sliderValue)`: Updates the rotation of the selected AR object based on slider input.

- **Private Methods**:
  - `void Awake()`: Initializes the selected prefab with the default AR raycast prefab.
  - `void Update()`: Handles touch input for selecting and placing AR objects.
  - `bool SelectARObject(Vector2 touchPosition)`: Selects an AR object if touched.
  - `void SelectARPlane(Vector2 touchPosition)`: Places a new AR object if an AR plane is touched.
  - `bool IsPointOverUIObject(Vector2 pos)`: Checks if the touch position is over a UI element to prevent interference.

### 📄 ARObject.cs

This script handles the selection state and material management for AR objects. When an object is selected, it changes its material to indicate selection and reverts to the original material when deselected.

#### Object Recognition(Select) Method

1. Shoot a ray at a touch position centered on the camera
2. Receive the value of the AR object that collided with the ray and set the selected value of the AR object to true.
3. The setter of the AR object is called and the Meterial color changes.

- **Private Fields**:

  - `bool IsSelected`: Indicates whether the object is selected.
  - `List<MeshRenderer> childMeshRenderers`: Stores the mesh renderers of all child objects.
  - `List<Material> originalMaterials`: Stores the original materials of all child objects.
  - `Material unlitBlackMaterial`: The material to apply when the object is selected.

- **Public Properties**:

  - `bool Selected`: Gets or sets the selection state of the object and updates the material accordingly.

- **Private Methods**:
  - `void Awake()`: Initializes the mesh renderers and original materials of all child objects and loads the `UnlitBlack` material.
  - `void UpdateMaterialColor()`: Updates the material of all child objects based on the selection state.

## 📁 Scenes

### 📄 MultipleObjectControl

Hierachy

```
- Directional Light
- AR Session
- XR Origin
  - Camera Offset
    - Main Camera
- AR Default Plane
- UI
  - Canvas
    - Scroll View
      - Viewport
        - Content
          - ChairBtn1
          - ChairBtn2
          - TableBtn1
          - ...
    - Sliders
      - ScaleSlider
      - Rotation Slider
  - EventSystem
  - ARSceneManager
- ARObjectManager
```

#### AR Session

- Manages the overall Ligecycle of AR, whether AR is supported, and whether or not the session is run.
- Perform image analysis and algorithms by receiving camera image data/motion data from hardware devices
- Establish a connection between the real world and the virtual world modeling AR content

#### XR Origin

- Components needed to transform AR elements into Unity space, used to adjust the scale of all elements, actual camera position
- Object for mapping AR contents rendered in the virtual world to the real world coordinate space

#### AR Default Plane

- A component that finds a plane based on feature points obtained from the camera.

#### Scroll View

- An object that represents a horizontal scrollbar. Allows users to scroll content left and right.
- Contains buttons that connect each object.

#### Sliders

- This is an object that controls the size and rotation of the selected object.
- Size
  - minimum : 0.1, maximum: 1
- Rotation
  - minimum : 0, maximum: 360

## 🪑 Example Scene

## 🐞 Error Correction

### The latest version of the Android API is too high and is not compatible with Unity apps.

"This app isn't compatible with the latest version of Android. Check for an update or contact the apps developer."

- Modify target architecture(https://discussions.unity.com/t/unity-game-not-compatible-with-latest-version-of-android-error/326763)

### arRaycast operates when touching the UI

Blocks arRaycastManager's raycast from working.

### Press and hold, objects will continue to be created.

Modified to create an object by recognizing it only once when pressed

## 🔗 Reference

## ‍💻 Developer

[nodb](https://github.com/nodb)

## 💳 License

This project is released under the MIT License.  
Detailed license information can be found in the [LICENSE]() file.
