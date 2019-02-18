using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{

    private CharacterController cc;

    public float moveSpeed;
    public float rotSpeed;
    public float sinkSpeed;

    // Start is called before the first frame update
    void Start()
    {
	cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

	Vector3 move = Vector3.zero;

	// Move character forward
	float vert = Input.GetAxis("Vertical");
	if (vert > 0f) // Only move forward.
	    move += Time.deltaTime * vert * moveSpeed * cc.transform.forward;

	// Lower character to ground
	if (!cc.isGrounded)
	    move += Time.deltaTime * sinkSpeed * Vector3.down;

	// Apply character movement
	cc.Move(move);

	// Rotate character
	cc.transform.Rotate(Time.deltaTime * Input.GetAxis("Horizontal") * rotSpeed * Vector3.up);
		

    }
}

