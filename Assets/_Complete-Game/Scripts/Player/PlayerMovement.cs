using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 6f;            // The speed that the player will move at.


    Vector3 movement;                   // The vector to store the direction of the player's movement.
    Animator anim;                      // Reference to the animator component.
    Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
    [SyncVar]
    public int myID;
    [SyncVar]
    public bool isDead;


    void Awake()
    {
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            myHp = 100f;
            Camera.main.GetComponent<CameraFollow>().target = transform;
            CmdTest();
        }
        else if(isServer)
        GameManager.instance.CheckAllReady();
    }

    void FixedUpdate()
    {
        
        if (!isLocalPlayer) return;
        // Store the input axes.
        float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        float v = CrossPlatformInputManager.GetAxisRaw("Vertical");
        if(isWarped && /*(myID % 4)<2*/ myID % 2 == 0)
        {
            v = -CrossPlatformInputManager.GetAxisRaw("Horizontal");
            h = CrossPlatformInputManager.GetAxisRaw("Vertical");
            Camera.main.GetComponent<CameraFollow>().offset = new Vector3(-5, 4f, 0);
        }
        else if (isWarped && /*(myID % 4) >= 2*/ myID % 2 == 1)
        {
            v = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            h = -CrossPlatformInputManager.GetAxisRaw("Vertical");
            Camera.main.GetComponent<CameraFollow>().offset = new Vector3(5, 4f, 0);
        }

        // Move the player around the scene.
        Move(h, v);

        // Turn the player to face the mouse cursor.
        Turning(h, v);

        // Animate the player.
        Animating(h, v);


    }
    private float count = 1f;
    private void Update()
    {
        if (!isLocalPlayer) return;
        count -= Time.deltaTime;
        RaycastHit hit;
        PuzzleBlock pb = null;
        Vector3 pos = transform.position + new Vector3(0, 1, 0);
        bool isHit = Physics.Raycast(pos, -transform.up, out hit, 10f);
        CmdOnHover(0, -1, -1);
        if (isHit)
        {
            if (hit.collider.tag == "GameBlock")
            {
                //Debug.Log(hit.collider.name);
                pb = hit.collider.gameObject.GetComponent<PuzzleBlock>();
                //pb.EnterChar();
                OnHoverBlock(pb);
            }
        }
        
        if (Input.GetButtonDown("Jump") && !isDead)
        {
            anim.SetTrigger("Attack");            
            if (count > 0) return;
            count = 1;
            CmdDoAction();
            if(pb!=null)CmdSelectBlock(pb.parentIndex, pb.x, pb.y);
        }

        
    }

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);
    }


    void Turning(float h, float v)
    {
        Vector3 turnDir = new Vector3(h, 0f, v);

        if (turnDir != Vector3.zero)
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = (transform.position + turnDir) - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.
            playerRigidbody.MoveRotation(newRotatation);
        }


    }
    [Command]
    public void CmdTest()
    {
        myID = connectionToClient.connectionId;
        Debug.Log("A Client called a command, client's id is: " + myID);
    }
    [Command]
    public void CmdDoAction()
    {
        //Debug.Log("DoAction   " + io);
        if (io != null)
            io.DoAction();


    }

    public void OnHoverBlock(PuzzleBlock pb)
    {
        if (pb == null) return;
        if (isDead) return;
        CmdOnHover(pb.parentIndex, pb.x, pb.y);
    }
    [Command]
    private void CmdSelectBlock(int parentId, int x, int y)
    {
        BlockStage bs = GameManager.instance.GetBlockStage(parentId);
        bs.SelectBlock(x, y);
    }
    [Command]
    private void CmdOnHover(int parentId, int x, int y)
    {
        //Debug.Log(x + "   " + y);
        BlockStage bs = GameManager.instance.GetBlockStage(parentId);
        PuzzleBlockInfo info = new PuzzleBlockInfo();
        info.bs = bs;
        info.pb = bs.GetBlock(x, y);
        GameManager.instance.SetStatedBlocks(myID, info);
        //bs.CmdOnHover(x, y);
    }

    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f || v != 0f;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", walking);
    }

    InteractiveObject io;

    private void OnTriggerEnter(Collider other)
    {
        //if (!isLocalPlayer) return;
        InteractiveObject target = other.GetComponent<InteractiveObject>();
        if (target != null)
        {
            Debug.Log("OnTriggerEnter   " + io);
            io = target;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        //if (!isLocalPlayer) return;
        InteractiveObject target = other.GetComponent<InteractiveObject>();
        if (target != null)
        {
            if (io == target) io = null;
        }
    }

    private bool isWarped = false;
    [ClientRpc]
    public void RpcWarpPlayer(Vector3 warpPosition)
    {
        transform.position = playerRigidbody.position = warpPosition;
        isWarped = true;
    }

    [SyncVar]
    public float myHp = 100;

    [ClientRpc]
    public void RpcDemage(float dmg)
    {
        myHp -= dmg;
        gameObject.GetComponentInChildren<CBUIHP>().CurrentHP = (int)myHp;
        anim.SetTrigger("Damage");
        if(myHp < 0)
        {
            myHp = 0;
            Death();
        }
    }

    private void Death()
    {
        gameObject.GetComponentInChildren<Canvas>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        isDead = true;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject player in players)
        {
            if(gameObject !=player && player.GetComponent<PlayerMovement>().myID%2 == myID%2 && !player.GetComponent<PlayerMovement>().isDead)
            {
                return;
            }
        }

        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerMovement>().EndGame();
        }
                 
    }

    public void EndGame()
    {
        if (!isLocalPlayer) return;

        GameResult.Instance.myTeam = (GameResult.Team)(myID % 2) + 1;
        Debug.Log("myid:" + myID);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<PlayerMovement>().myID % 2 == myID % 2 && !player.GetComponent<PlayerMovement>().isDead)
            {
                GameResult.Instance.winner = (GameResult.Team) (myID % 2) +1;
                SceneManager.LoadScene("Ending");
                return;
            }
        }
        GameResult.Instance.winner = myID % 2 == 0 ? (GameResult.Team)2 : (GameResult.Team)1;
        SceneManager.LoadScene("Ending");            
    }

    private void OnGUI()
    {
        if (!isLocalPlayer) return;
        GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 30), "MyHP : " + myHp);
        GUI.Label(new Rect(100, 10, 100, 30), "myID -> " + myID);
    }
}
