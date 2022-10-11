using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviourPun
{
    [Tooltip("The current Health of our player")]
    public float RotateSpeed = 10f;
    #region Private Fields

    [SerializeField]
    private float directionDampTime = .25f;
    private Animator animator;

    TestPlayerManager tpm;

    #endregion

    #region MonoBehaviour CallBacks

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        tpm = GetComponent<TestPlayerManager>();
        if (!animator)
        {
            Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        if (!animator)
        {
            return;
        }
        if (tpm.tpuc.isStunned)
        {
            animator.SetFloat("Speed", 0);
            return;
        }
        // deal with Jumping
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // only allow jumping if we are running.
        if (stateInfo.IsName("Base Layer.Run"))
        {
            // When using trigger parameter
            if (Input.GetButtonDown("Fire2"))
            {
                animator.SetTrigger("Jump");
            }
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (v < 0)
        {
            v = 0;
        }
        animator.SetFloat("Speed", h * h + v * v);
        //animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        transform.RotateAround(Vector3.up, h * Time.deltaTime * RotateSpeed);
    }

    #endregion
}
