// using System;
// using System.Collections;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using Cursor = UnityEngine.Cursor;
// using Quaternion = UnityEngine.Quaternion;
// using Vector2 = UnityEngine.Vector2;
// using Vector3 = UnityEngine.Vector3;
// using UnityEngine.SceneManagement;


// public class Player : MonoBehaviour
// {
//     public static event Action<string> OnPlayerInteraction;
    
//     [SerializeField] private Transform cameraTransform;
//     [SerializeField] private GameObject PauseMenu;
//     [SerializeField] private GameObject InteractPrompt;
//     [SerializeField] private GameObject LevelPortal;
//     [SerializeField] private GameObject SpookyGuy;
//     [SerializeField] private LayerMask interactableLayer;
//     [SerializeField] private SequenceManager sequenceManager;
//     [SerializeField] private GameObject toolManage;



//     CharacterController controller;
//     InputAction moveAction;
//     InputAction jumpAction;
//     InputAction lookAction;
//     InputAction crouchAction;
//     InputAction interactAction;
//     InputAction sprintAction;
//     InputAction pauseAction;   
//     private bool isGrounded;
//     private bool isCrouching;
//     private bool isTryingUncrouch;
//     private Vector3 playerVelocity;
//     private float xRotation = 0f;
//     private float yRotationForCutscene = 0f;
//     private float baseCharacterHeight;
//     private float speed;
//     private bool skipDestroying;
//     private bool isLeaving = false;
//     private AudioSource[] noises;

//     public bool isInteracting;
//     public bool cutScene;
//     public bool paused = false;
//     public float mouseSensitivity = 0.1f;
//     public float baseSpeed = 2.6f;
//     public float jumpPower = 1.0f;
//     public float gravity = -9.81f;
//     public bool hasLockpickAtStart;
    
//     void Start()
//     {
//         noises = GetComponents<AudioSource>();
//         speed = baseSpeed;
//         controller = GetComponent<CharacterController>();
//         baseCharacterHeight = controller.height;
//         // Lock Mouse
//         Cursor.lockState = CursorLockMode.Locked;
//         Cursor.visible = false;
//         // Input Systems
//         moveAction = InputSystem.actions.FindAction("Move");
//         jumpAction = InputSystem.actions.FindAction("Jump");
//         lookAction = InputSystem.actions.FindAction("Look");
//         pauseAction = InputSystem.actions.FindAction("Cancel");
//         crouchAction = InputSystem.actions.FindAction("Crouch");
//         interactAction = InputSystem.actions.FindAction("Interact");
//         sprintAction = InputSystem.actions.FindAction("Sprint");
//         pauseAction.performed += ctx => TogglePause();
//         if (hasLockpickAtStart)
//             toolManage.GetComponent<ToolUIManager>().m_is_lockpick_unlocked = true;
//         crouchAction.performed += OnCrouchPressed;
//         crouchAction.canceled += OnCrouchReleased;
//         //sprintAction.performed += OnSprintPressed;
//         //sprintAction.canceled += OnSprintReleased;

//         if (LevelPortal == null)
//         {
//             LevelPortal = GameObject.FindGameObjectWithTag("Portal");
//         }
//         //Handle starting cutscene:
//         if (SceneManager.GetActiveScene().buildIndex == 1)
//             StartCoroutine(StartingCutsceneAndVoiceLines());
//     }
//     IEnumerator StartingCutsceneAndVoiceLines() 
//     {
//         cutScene = true;
//         cameraTransform.SetLocalPositionAndRotation(new Vector3 (-1.5f, 0.25f, 0), Quaternion.Euler(-45f, 0f, 0f));
//         yield return new WaitUntil(() => cutScene == false);
//         cameraTransform.localPosition = new Vector3 (0f, 0.5f, 0f);
//         yield return new WaitUntil(() => transform.position.z <= -15 || transform.position.z >= 20);
//         sequenceManager.PlaySequence("DoorsJoke", transform.position);
//         yield return new WaitUntil(() => isLeaving == true);
//         yield return new WaitUntil(() => transform.position.x <= 1.5);
//         sequenceManager.StopAllCoroutines();
        
//         StartCoroutine(SpookyLights());
//     }
//     public IEnumerator SpookyLights()
//     {
//         noises[0].Stop();
//         GameObject[] lights = GameObject.FindGameObjectsWithTag("light");
//         foreach (GameObject light in lights)
//         {
//             light.GetComponent<Light>().enabled = false;
//         }
//         noises[2].Play();
//         yield return new WaitForSeconds(3);
//         SpookyGuy.SetActive(true);
//         foreach (GameObject light in lights)
//         {
//             light.GetComponent<Light>().color = new Color(1f, 0, 0);
//             light.GetComponent<Light>().intensity = 1;
//             light.GetComponent<Light>().range = 10;
//             light.GetComponent<Light>().enabled = true;
//         }
//         noises[2].Play();
//         GameObject.FindGameObjectWithTag("EndLight").GetComponent<Light>().enabled = true;
//         yield return new WaitForSeconds(3);
//         foreach (GameObject light in lights)
//         {
//             light.GetComponent<Light>().enabled = false;
//         }
//         noises[2].Play();
//         yield return new WaitForSeconds(2);
//         foreach (GameObject light in lights)
//         {
//             light.GetComponent<Light>().color = new Color(1f, 1f, 1f);
//             light.GetComponent<Light>().intensity = 0.25f;
//             light.GetComponent<Light>().range = 4;
//             light.GetComponent<Light>().enabled = true;
//         }
//         noises[2].Play();
//         SpookyGuy.SetActive(false);
//     }

//     void Update()
//     {
//         Vector2 move = moveAction.ReadValue<Vector2>();
//         Vector2 look = lookAction.ReadValue<Vector2>();
//         RotateAndMovePlayer(move, look);
//         HandleInteract();
//     }

//     // Camera and Player Movement 
//     void RotateAndMovePlayer(Vector2 moveValue, Vector2 lookValue)
//     {
//         if (!paused && !cutScene)
//         {
//             // Check if grounded
//             isGrounded = controller.isGrounded;
//             if (isGrounded && playerVelocity.y < 0)
//             {
//                 playerVelocity.y = 0f;
//             }

//             //camera
//             transform.Rotate(lookValue.x * mouseSensitivity * Vector3.up);
//             xRotation -= lookValue.y * mouseSensitivity;
//             xRotation = Mathf.Clamp(xRotation, -60f, 60f);
//             cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

//             // Movement
//             Vector3 moveDirection = (transform.forward * moveValue.y + transform.right * moveValue.x).normalized;
//             if (moveValue.y == 0 && moveValue.x == 0)
//             {
//                 if (noises[0].isPlaying)
//                     noises[0].Stop();
//             }
//             else
//             {
//                 if (!noises[0].isPlaying)
//                     noises[0].Play();
//             }
//             controller.Move(speed * Time.deltaTime * moveDirection);

//             // Jumping
//             if (jumpAction.IsPressed() && isGrounded && !isCrouching)
//             {
//                 playerVelocity.y += Mathf.Sqrt(jumpPower * -2.0f * gravity);
//             }
//             playerVelocity.y += gravity * Time.deltaTime;
//             controller.Move(playerVelocity * Time.deltaTime); 
//         }   
//         else if (!paused)
//         {
//             //cutscene camera
//             yRotationForCutscene += lookValue.x * mouseSensitivity;
//             yRotationForCutscene = Mathf.Clamp(yRotationForCutscene, -90f, 90f);
//             xRotation -= lookValue.y * mouseSensitivity;
//             xRotation = Mathf.Clamp(xRotation, -60f, 60f);
//             cameraTransform.localRotation = Quaternion.Euler(xRotation, yRotationForCutscene, 0f);
//         }
//     }

//     // Crouching
//     void OnCrouchPressed(InputAction.CallbackContext context)
//     {
//         if (!cutScene)
//         {
//             isTryingUncrouch = false;
//             isCrouching = true;
//             controller.height = baseCharacterHeight * 0.75f;
//             controller.center = new Vector3(0, -baseCharacterHeight/8.0f, 0);
//             speed = baseSpeed/2;
//             cameraTransform.localPosition = Vector3.zero;
//         }
//     }
//     void OnCrouchReleased(InputAction.CallbackContext context)
//     {
//         if (!cutScene)
//         {
//             // Check if the player will suffocate themselves if they uncrouch
//             bool ceilingCheck = Physics.CheckSphere(transform.position + Vector3.up *(baseCharacterHeight/2), 0.5f);
//             if (!ceilingCheck)
//             {
//                 isCrouching = false;
//                 controller.height = baseCharacterHeight;
//                 controller.center = Vector3.zero;
//                 speed = baseSpeed;
//                 cameraTransform.localPosition = new Vector3 (0, 0.5f, 0);
//             }
//             else 
//             {
//                 isTryingUncrouch = true;
//                 StartCoroutine(WaitForClearUncrouch());
//             }
//         }
//     }
//     // If the player stops pressing crouch while unable to uncrouch, uncrouch once the player is able to uncrouch.
//     // There is probably better way to do this
//     IEnumerator WaitForClearUncrouch()
//     {
//         yield return new WaitUntil(() => !isTryingUncrouch || !Physics.CheckSphere(transform.position + Vector3.up *(baseCharacterHeight/2), 0.5f));
//         if (isTryingUncrouch)
//         {
//             isCrouching = false;
//             controller.height = baseCharacterHeight;
//             controller.center = Vector3.zero;
//             speed = baseSpeed;
//             cameraTransform.localPosition = new Vector3 (0, 0.5f, 0);
//         }
//     }

//     // Sprinting (no sprinting until later probably)
//     void OnSprintPressed(InputAction.CallbackContext context)
//     {
//         if (!isCrouching)
//         {
//             speed = baseSpeed * 1.5f;
//         }
//     }
//     void OnSprintReleased(InputAction.CallbackContext context)
//     {
//         speed = baseSpeed;
//     }

//     // Pause the game
//     public void TogglePause()
//     {
//         paused = !paused;
//         if (paused)
//         {
//             PauseMenu.SetActive(true);
//             Time.timeScale = 0f;
//             Cursor.lockState = CursorLockMode.None;
//             Cursor.visible = true;
//         }
//         else
//         {
//             PauseMenu.SetActive(false);
//             Time.timeScale = 1f;
//             Cursor.lockState = CursorLockMode.Locked;
//             Cursor.visible = false;
//         }
//     }
//     void HandleInteract()
//     {
//         Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
//         RaycastHit hit = new RaycastHit();
//         if (Physics.Raycast(ray, out hit, 2.0f, interactableLayer) && !cutScene)
//         {
//             if (!isInteracting)
//                 InteractPrompt.SetActive(true);
//             if (interactAction.IsPressed()  && !isInteracting)
//             {
//                 isInteracting = true;
//                 string tag = hit.transform.tag;
//                 print($"interact with {tag}");

//                 if (string.CompareOrdinal(tag, "Drawer") == 0)
//                 {
//                     skipDestroying = true;
//                     hit.transform.GetComponent<Drawer>().OpenDrawer();
//                 }
//                 else if (string.CompareOrdinal(tag, "Door") == 0)
//                 {
//                     skipDestroying = true;
//                     hit.transform.GetComponent<Door>().OpenDoor();
//                     noises[1].Play();
//                 }
//                 else if (string.CompareOrdinal(tag, "LightSwitch") == 0)
//                 {
//                     skipDestroying = true;
//                 }
//                 else if (string.CompareOrdinal(tag, "Phone") == 0) 
//                 {
//                     skipDestroying = true;
//                     hit.transform.GetComponent<Phone>().AnswerPhone();
//                 }
//                 else if (string.CompareOrdinal(tag, "Lockpick") == 0) 
//                 {
//                     OnPlayerInteraction?.Invoke(tag);
//                     sequenceManager.PlaySequence("LockpickPickup", transform.position);
//                 }
//                 else if (string.CompareOrdinal(tag, "LevelWinObject") == 0) 
//                 {
//                     LevelPortal.layer = LayerMask.NameToLayer("Interactable");
//                     GameObject.FindGameObjectWithTag("EndLight").GetComponent<Light>().enabled = true;
//                     OnPlayerInteraction?.Invoke(tag);
//                     if (hit.transform.name == "briefcase")
//                     {
//                         GameObject.FindGameObjectWithTag("Phone").GetComponent<Phone>().Ring();
//                     }
//                     isLeaving = true;
//                 }
//                 else if (string.CompareOrdinal(tag, "Portal") == 0) 
//                 {
//                     skipDestroying = true;
//                     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
//                 }
//                 else
//                 {
//                     noises[3].Play();
//                     OnPlayerInteraction?.Invoke(tag);
//                 }
//                 if (!skipDestroying)
//                 {
//                     DestroyImmediate(hit.transform.gameObject);
//                 }

//                 skipDestroying = false;
//                 if (string.CompareOrdinal(tag, "Door") != 0)
//                     StartCoroutine(WaitToStopInteracting());
//             }
//         }
//         else
//         {
//             InteractPrompt.SetActive(false);
//         }
//     }

//     private IEnumerator WaitToStopInteracting()
//     {
//         yield return new WaitForSeconds(1f);
//         isInteracting = false;
//     }
//     public void QuitToMenu()
//     {
//         SceneManager.LoadScene(0);
//     }
// }
