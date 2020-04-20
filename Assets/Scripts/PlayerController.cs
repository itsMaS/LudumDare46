using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, Knockable
{
    public static PlayerController instance;

    [System.Serializable]
    struct Hand
    {
        public Hand(Transform ik, Vector3 restPos)
        {
            this.ik = ik;
            this.restPos = restPos;
            used = false;
        }
        public Transform ik;
        public Vector3 restPos;
        public bool used;
    }


    [SerializeField]
    Camera cam;
    [SerializeField]
    ContactFilter2D filter;

    [SerializeField]
    Hand[] Hands;

    [Header("Attributes")]
    [SerializeField]
    float movementSpeed;
    [SerializeField]
    float maxWater;
    [SerializeField]
    int maxSeeds;
    [SerializeField]
    float burnStrength;
    [SerializeField]
    float wateringStrength;

    public enum Tools { WateringCan, Flamethrower, SeedPlanter, Laser, None}
    public static Tools current = Tools.None;
    bool drilling = false;

    Rigidbody2D rb;
    public static Vector3 mousePosition;

    AudioSource moving;

    bool hasControl = false;

    private void Awake()
    {
        instance = this;
        moving = AudioManager.Play("Robot/moving_loop",0,true);
        rb = GetComponent<Rigidbody2D>();
        for (int i = 0; i < Hands.Length; i++)
        {
            Hands[i].restPos = Hands[i].ik.position - transform.position;
        }
    }
    private void Start()
    {
    }
    private void Update()
    {
        if(hasControl)
        {
            moving.volume = 0.5f*Mathf.InverseLerp(-0.1f,60, rb.velocity.magnitude);
            MoveHands();
            SelectionControl();
            UseTool();
        }
    }
    private void FixedUpdate()
    {
        if(hasControl)
        {
            Movement();
        }
    }

    [SerializeField]
    Vector2 HorizontalClamp;
    [SerializeField]
    Vector2 VerticalClamp;
    void Movement()
    {
        //rb.MovePosition(transform.position + movementSpeed * new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") * 0.5f));
        rb.AddForce(movementSpeed * new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") * 0.5f));
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10);
    }
    void SelectionControl()
    {
        Tools last = current;

        drilling = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (current)
            {
                case Tools.WateringCan:
                    current = Tools.Flamethrower;
                    break;
                case Tools.Flamethrower:
                    current = Tools.Laser;
                    break;
                case Tools.SeedPlanter:
                    current = Tools.WateringCan;
                    break;
                case Tools.Laser:
                    current = Tools.SeedPlanter;
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            switch (current)
            {
                case Tools.WateringCan:
                    current = Tools.SeedPlanter;
                    break;
                case Tools.Flamethrower:
                    current = Tools.WateringCan;
                    break;
                case Tools.SeedPlanter:
                    current = Tools.Laser;
                    break;
                case Tools.Laser:
                    current = Tools.Flamethrower;
                    break;
            }
        }


        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            current = Tools.Laser;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            current = Tools.SeedPlanter;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            current = Tools.WateringCan;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            current = Tools.Flamethrower;
        }

        if(current != last)
        {
            AudioManager.Play("Robot/change_tool",0.5f);
        }
    }

    [SerializeField]
    Tool WateringCan;
    [SerializeField]
    Tool Flamethrower;
    [SerializeField]
    Tool SeedPlanter;
    [SerializeField]
    Tool Laser;


    void UseTool()
    {
        for (int i = 0; i < Hands.Length; i++)
        {
            Hands[i].used = false;
        }
        if (!drilling)
        {
            switch (current)
            {
                case Tools.WateringCan:
                    Hands[5].ik.position = mousePosition;
                    WateringCan.SelectTool();
                    break;
                case Tools.Flamethrower:
                    Hands[1].ik.position = mousePosition;
                    Flamethrower.SelectTool();
                    break;
                case Tools.SeedPlanter:
                    Hands[4].ik.position = mousePosition;
                    SeedPlanter.SelectTool();
                    break;
                case Tools.Laser:
                    Hands[0].ik.position = mousePosition;
                    Laser.SelectTool();
                    break;
            }
        }
    }

    void MoveHands()
    {
        foreach (Hand item in Hands)
        {
            if(!item.used)
            {
                item.ik.transform.position = Vector3.Lerp(item.ik.transform.position, transform.position + item.restPos, 0.05f);
            }
            else
            {
                item.ik.transform.position = mousePosition;
            }
        }
    }

    public void SetControl(bool control)
    {
        hasControl = control;
    }
    public void GiveControl()
    {
        hasControl = true;
        current = Tools.Laser;
        Debug.Log(Time.time);
    }
    public void TakeControl()
    {
        hasControl = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(Mathf.Lerp(HorizontalClamp.x, HorizontalClamp.y, 0.5f),
            Mathf.Lerp(VerticalClamp.x, VerticalClamp.y, 0.5f)),
            new Vector3(Mathf.Abs(HorizontalClamp.y - HorizontalClamp.x), Mathf.Abs(VerticalClamp.y - VerticalClamp.x)));
        Gizmos.DrawSphere(mousePosition, 0.2f);
    }

    public void Knock(Vector3 direction)
    {
        rb.AddForce(direction, ForceMode2D.Impulse);
    }
}
