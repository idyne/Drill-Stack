using UnityEngine;
using FateGames;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour
{
    [SerializeField] private Vector3 speed = Vector3.one;
    [SerializeField] private DrillStack drillStack = null;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject brokenMachineEffect;
    private Swerve1D swerve = null;
    private Vector3 initialSpeed = Vector3.zero;
    private CharacterController controller = null;
    private float swerveAnchorX = 0;
    private PlayerState state = PlayerState.IDLE;
    public bool IsWaiting = false;
    private float rate = 0;

    public Vector3 Speed { get => speed; }
    public DrillStack DrillStack { get => drillStack; }
    public Animator Anim { get => anim; }

    private void Awake()
    {
        initialSpeed = speed;
        swerve = InputManager.CreateSwerve1D(Vector2.right, Screen.width * 0.5f);
        swerve.OnStart = () => { swerveAnchorX = transform.position.x; };
        controller = GetComponent<CharacterController>();
    }


    private void Update()
    {
        if (GameManager.Instance.State == GameManager.GameState.IN_GAME)
        {
            if (state != PlayerState.BONUS)
                CheckBonus();
            CheckInput();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.State == GameManager.GameState.IN_GAME)
        {
            
            MoveForward();
            Move(rate);
        }
    }

    private void CheckBonus()
    {
        if (transform.position.z >= MainLevelManager.Instance.Road.Length)
        {
            state = PlayerState.BONUS;
            StartBonus();
        }
    }

    private void StartBonus()
    {
        speed.z *= 1.5f;
        CameraFollow cameraFollow = CameraFollow.Instance;
        cameraFollow.Offset = new Vector3(5, 7.8f, -7.3f);
        cameraFollow.rotation = new Vector3(26, -24, 0);
    }

    private void Win()
    {
        foreach (Rig rig in GetComponentsInChildren<Rig>())
            rig.weight = 0;
        anim.SetTrigger("WIN");
        MainLevelManager.Instance.FinishLevel(true);
    }

    private void CheckConfetties()
    {
        List<GameObject> usedConfettiPositions = new List<GameObject>();
        for (int i = 0; i < MainLevelManager.Instance.ConfettiPositions.Count; i++)
        {
            if (transform.position.z >= MainLevelManager.Instance.ConfettiPositions[i].transform.position.z)
            {
                ConfettiManager.Instance.CreateConfettiDirectional(MainLevelManager.Instance.ConfettiPositions[i].transform.position, MainLevelManager.Instance.ConfettiPositions[i].transform.rotation.eulerAngles, Vector3.one * 3);
                usedConfettiPositions.Add(MainLevelManager.Instance.ConfettiPositions[i]);
            }
        }
        for (int i = 0; i < usedConfettiPositions.Count; i++)
        {
            MainLevelManager.Instance.ConfettiPositions.Remove(usedConfettiPositions[i]);
        }
    }

    public void StartWalking()
    {
        Anim.SetTrigger("WALK");
        state = PlayerState.WALKING;
    }

    private void CheckInput()
    {
        if (swerve.Active)
            rate = swerve.Rate;
            
    }

    private void Move(float rate)
    {
        Road road = MainLevelManager.Instance.Road;
        Vector3 targetPosition = transform.position;
        targetPosition.x = swerveAnchorX + rate * road.Width;
        Vector3 motion = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed.x) - transform.position;
        if ((transform.position + motion).x - controller.radius > road.Width / -2 && (transform.position + motion).x + controller.radius < road.Width / 2)
            controller.Move(motion);
    }

    public void Wait()
    {
        Anim.SetTrigger("IDLE");
        speed.z = 0.05f;
    }

    public void Continue()
    {
        Anim.SetTrigger("WALK");
        speed.z = initialSpeed.z;
    }

    private void MoveForward()
    {
            controller.Move(Vector3.forward * Time.deltaTime * speed.z);

    }

    public void Die()
    {
        if (state != PlayerState.DEAD)
        {
            brokenMachineEffect.SetActive(true);
            foreach (Rig rig in GetComponentsInChildren<Rig>())
                rig.weight = 0;
            state = PlayerState.DEAD;
            anim.SetTrigger("DIE");
            MainLevelManager.Instance.FinishLevel(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            ICollectible collectible = other.GetComponent<ICollectible>();
            collectible.GetCollected();
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Bonus Wall"))
        {
            if (state == PlayerState.BONUS)
                Win();
            else
                Die();
        }
        else if (other.CompareTag("Obstacle"))
        {
            BoxCollider boxCollider = other.GetComponent<BoxCollider>();
            boxCollider.enabled = false;
            Die();
        }
    }

    public enum PlayerState { IDLE, WALKING, DEAD, BONUS }
}
