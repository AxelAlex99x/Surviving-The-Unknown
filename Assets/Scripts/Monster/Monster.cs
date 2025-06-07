using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [SerializeField] private Animator monsterAnimator;

    [Header("References")]
    [SerializeField] private Generator generator;
    [SerializeField] private Flashlight flashlight;
    [SerializeField] private Transform player;
    [SerializeField] private AudioSource jumpscareSound;

    [Header("Movement Settings")]
    [SerializeField] private float stalkSpeed = 3f;
    [SerializeField] private float minRespawnTime = 10f;
    [SerializeField] private float maxRespawnTime = 20f;
    [SerializeField] private float minFlashlightRespawnTime = 3f;
    [SerializeField] private float maxFlashlightRespawnTime = 8f;
    [SerializeField] private float attackSpeed = 5f;
    [SerializeField] private float minAttackDistance = 1f;

    [Header("Jumpscare Settings")]
    [SerializeField] private float jumpscarePositionDistance = 1.5f;
    [SerializeField] private Light jumpscareLight;
    //[SerializeField] private float jumpscareDuration = 2f;

    [Header("Camera Zoom Settings")]
    [SerializeField] private float zoomDuration = 0.5f;
    [SerializeField] private float zoomDistance = 0.7f;

    [Header("UI Settings")]
    [SerializeField] private GameObject deathUI;
    [SerializeField] private string mainMenuScene = "MainMenu";

    [Header("Face Point for Zoom")]
    [SerializeField] private Transform monsterFacePoint;

    private NavMeshAgent agent;
    private Renderer monsterRenderer;
    private Collider monsterCollider;
    private InputManager inputManager;

    private Camera playerCamera;
    private MonoBehaviour[] cameraControllers;

    private bool isActive = true;
    private bool isJumpScaring = false;
    private bool isZooming = false;

    private float zoomProgress;
    private float respawnTimer;
    private float currentRespawnTime;

    private Vector3 lastPlayerPosition;
    private Vector3 originalCamPos;
    private Quaternion originalCamRot;
    
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private Image bloodDamage;
    [SerializeField] private GameObject bloodParticles;
    
    [Header("Door Interaction")]
    [SerializeField] private float doorDetectionRange = 2f;
    [SerializeField] private LayerMask doorLayer;
    [SerializeField] private float doorCheckInterval = 0.5f;
    private float lastDoorCheckTime;
    
    [SerializeField] private LayerMask flashlightIgnoreLayers;
    [SerializeField] PlayerUI playerUI;
    [SerializeField] 
    void Start()
    {
        playerCamera      = Camera.main;
        cameraControllers = player.GetComponentsInChildren<MonoBehaviour>();
        agent             = GetComponent<NavMeshAgent>();
        inputManager      = player.GetComponent<InputManager>();
        monsterRenderer   = GetComponent<Renderer>();
        monsterCollider   = GetComponent<Collider>();
        
        bloodDamage.enabled = false;
        bloodParticles.SetActive(false);
        
        agent.speed        = stalkSpeed;
        currentRespawnTime = Random.Range(minRespawnTime, maxRespawnTime);
        if (deathUI != null) deathUI.SetActive(false);
        jumpscareLight.enabled = false;
    }

    void Update()
    {
        if (generator.currentState == Generator.GeneratorState.Running)
        {
            if (isActive) 
            {
                currentRespawnTime = Random.Range(minRespawnTime, maxRespawnTime);
                DisableMonster();
            }
            return;
        }

        if (!isActive)
        {
            respawnTimer += Time.deltaTime;
            if (respawnTimer >= currentRespawnTime)
            {
                EnableMonster();
                respawnTimer = 0f;
            }
            return;
        }

        monsterAnimator.SetBool("IsMoving", agent.velocity.magnitude > 0.1f);

        if (!isJumpScaring)
            HandleStalkAndTrigger();
        else
            HandleJumpscare();

        HandleFlashlight();
        
        if(Time.time - lastDoorCheckTime > doorCheckInterval && !isJumpScaring)
        {
            CheckForDoors();
            lastDoorCheckTime = Time.time;
        }
    }

    private void CheckForDoors()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        
        if(Physics.Raycast(transform.position, directionToPlayer, out hit, doorDetectionRange, doorLayer))
        {
            Door door = hit.collider.GetComponent<Door>();
            if(door != null && !door.IsOpen)
            {
                Vector3 doorDirection = (hit.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(doorDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
            
                door.ToggleDoor(true);
            }
        }
    }
    
    private void HandleStalkAndTrigger()
    {
        lastPlayerPosition = player.position;

        if (Vector3.Distance(transform.position, lastPlayerPosition) > minAttackDistance)
        {
            agent.speed = attackSpeed;
            if(agent.enabled)
            agent.SetDestination(lastPlayerPosition);
        }
        else
        {
            TriggerJumpscare();
        }

        if (Vector3.Distance(transform.position, player.position) <= jumpscarePositionDistance)
            TriggerJumpscare();
    }

    private void TriggerJumpscare()
    {
        if (isJumpScaring) return;
        playerUI.ClearText();
        isJumpScaring = true;
        agent.enabled = false;
        foreach (var c in cameraControllers)
            if (c != null) c.enabled = false;

        monsterAnimator.SetTrigger("Jumpscare");
        if (jumpscareSound != null) jumpscareSound.Play();
        
        if (jumpscareLight != null) {
            jumpscareLight.enabled = true;
        }
        
        if (flashlight != null) {
            flashlight.TurnOff();
        }
        
        var pc = player.GetComponent<PlayerController>();
        if (pc != null) pc.enabled = false;
    }

    private void HandleJumpscare()
    {
        if (!isJumpScaring) return;

        AnimatorStateInfo stateInfo = monsterAnimator.GetCurrentAnimatorStateInfo(0);
        bool isInJumpscare = stateInfo.IsName("pick"); 

        if (isInJumpscare)
        {
            playerAudioSource.Stop();
            if (!isZooming)
            {
                originalCamPos = playerCamera.transform.position;
                originalCamRot = playerCamera.transform.rotation;
                isZooming = true;
                zoomProgress = 0f;
            }
        }

        if (isZooming)
        {
            zoomProgress += Time.deltaTime / zoomDuration;
            float t = Mathf.SmoothStep(0f, 1f, zoomProgress);

            Vector3 endPos = monsterFacePoint.position + monsterFacePoint.forward * zoomDistance;
            Quaternion endRot = Quaternion.LookRotation(monsterFacePoint.position - endPos, Vector3.up);

            playerCamera.transform.position = Vector3.Lerp(originalCamPos, endPos, t);
            playerCamera.transform.rotation = Quaternion.Slerp(originalCamRot, endRot, t);

            if (zoomProgress >= 1f && !deathUI.activeSelf)
            {
                if (stateInfo.normalizedTime > 0.5f)
                {
                    bloodDamage.enabled = true;
                    bloodParticles.SetActive(true);
                }
                if (stateInfo.normalizedTime > 1.4f)
                {
                    deathUI.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    PauseMenu pauseMenu = FindFirstObjectByType<PauseMenu>();
                    if (pauseMenu != null)
                    {
                        pauseMenu.enabled = false;
                    }
                    Ambience ambienceScript = FindFirstObjectByType<Ambience>();
                    RandomSoundEffects soundEffects = FindFirstObjectByType<RandomSoundEffects>();
                    WindSound windSound = FindFirstObjectByType<WindSound>();
                    ambienceScript.enabled = false;
                    soundEffects.enabled = false;
                    windSound.enabled = false;
                    AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
                    foreach (AudioSource source in allAudioSources)
                    {
                        if (source != jumpscareSound)
                        {
                            source.Stop();
                        }
                    }
                    Time.timeScale = 0f;
                    isZooming = false;
                    isJumpScaring = false; 
                }
            }
        }
    }

    private void HandleFlashlight()
    {
        if (flashlight.IsActive && inputManager.playerActions.FlashlightColor.IsInProgress())
        {
            Vector3 toMonster = transform.position - flashlight.transform.position;
            float distance = toMonster.magnitude;
            
            if (distance > flashlight.lightComponent.range)
                return;

            float angle = Vector3.Angle(flashlight.transform.forward, toMonster.normalized);
            if (angle > flashlight.lightComponent.spotAngle / 2f)
                return;
            
            int layerMask = ~flashlightIgnoreLayers;
              
            RaycastHit hit;
            if (Physics.Raycast(flashlight.transform.position, toMonster.normalized, out hit, distance))
            {
                if (hit.collider.gameObject != gameObject)
                    return; 
            }
            
            DisableMonster();
            RepositionMonster();
            currentRespawnTime = Random.Range(minFlashlightRespawnTime, maxFlashlightRespawnTime);
            respawnTimer = 0f;
        }
    }

    private void EnableMonster()
    {
        monsterAnimator.SetBool("IsMoving", true);
        RepositionMonster();
        isActive        = true;
        agent.enabled   = true;
        monsterRenderer.enabled = true;
        monsterCollider.enabled = true;
        if (jumpscareLight != null) {
            jumpscareLight.enabled = false;
        }
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void DisableMonster()
    {
        if (isJumpScaring) return;
        monsterAnimator.SetBool("IsMoving", false);
        RepositionMonster();
        isActive        = false;
        agent.enabled   = false;
        monsterRenderer.enabled = false;
        monsterCollider.enabled = false;
        if (jumpscareLight != null) {
            jumpscareLight.enabled = false;
        }
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void RepositionMonster()
    {
        Vector3 rnd = Random.insideUnitSphere.normalized * 50f + player.position;
        if (NavMesh.SamplePosition(rnd, out var hit, 70f, NavMesh.AllAreas))
            transform.position = hit.position;
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, jumpscarePositionDistance);
    }
    #endif

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}