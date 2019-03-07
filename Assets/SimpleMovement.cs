using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{

    private CharacterController cc;

    public float moveSpeed;
    public float rotSpeed;
    public float sinkSpeed;

	private float hold;

    // Start is called before the first frame update
    void Start()
    {
	cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
	Vector3 move = Vector3.zero;
	Vector3 rotate = Vector3.zero;

	// Calculate movement if free to move
	if(hold <= 0f)
	{
		float vert = Input.GetAxis("Vertical");
		if (vert > 0f) // Only move forward.
	   		move += Time.deltaTime * vert * moveSpeed * cc.transform.forward;

		rotate = Time.deltaTime * Input.GetAxis("Horizontal") * rotSpeed * Vector3.up;
	}else
	{
		hold -= Time.deltaTime;
	}

	// Calculate lower to ground
	if (!cc.isGrounded)
	    move += Time.deltaTime * sinkSpeed * Vector3.down;

	// Apply character translation
	cc.Move(move);

	// Apply character rotation
	cc.transform.Rotate(rotate);
    }

	void BeginHold(float length){
		hold = length;
	}
}

