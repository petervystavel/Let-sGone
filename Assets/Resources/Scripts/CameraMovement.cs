using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float SpeedFactor = 1f;
    public float FactorMin = 2f;

    public Vector4 DefaultRotation = new Vector4(-45f, 0f, 0f, 28f);
    public float DefaultRotationDuration = 0.5f;
    public float DefaultRotationSpeed = 28f;

    public Vector3 DefaultTranslationOffset = new Vector3(0f, 0f, 0f);
    public float DefaultTranslationOffsetDuration = 1f;
    public float DefaultTranslationOffsetSpeed = 20f;

    public bool DebugDisableSmooth = false;

    Vector4 m_oCurrentRotation;
    float m_fCurrentRotationSpeed;

    Vector3 m_oTargetTranslationToPlayer;

    Vector4 m_oTargetRotation;
    Vector4 m_oStartRotation;
    Vector4 m_oDelayRotation;
    Vector4 m_oElapsedTimeRotation;

	Rigidbody m_oRigidBody;

	void Start()
	{
        m_oRigidBody = GetComponent<Rigidbody>();

        GameObject player = GameObject.Find("Player");
        Vector3 oCurrentTranslationToPlayer = Utils.GetTranslationTo(gameObject, player);

        m_oCurrentRotation = DefaultRotation;

        transform.position += DefaultTranslationOffset;

        UpdateTransform();
    }

    void Update()
    {
        GameObject player = GameManager.Player;
        if (player == null )
			return;

		//Check the offset translation between camera and player
		Vector3 oOffsetTranslationToPlayer = GetOffsetTranslationToPlayer();

		float fFactor = SpeedFactor * oOffsetTranslationToPlayer.magnitude;
		if( fFactor < FactorMin )
			fFactor = FactorMin;

		m_oRigidBody.velocity = fFactor * oOffsetTranslationToPlayer;

		if( DebugDisableSmooth )
		{
			m_oRigidBody.velocity = Vector3.zero;
			transform.position = transform.position + oOffsetTranslationToPlayer;
		}

		//#UGLY TMP
		Vector3 oTranslationToPlayer = player.transform.position - transform.position;

		RaycastHit[] oRaycastHits = Physics.RaycastAll( transform.position, oTranslationToPlayer.normalized, oTranslationToPlayer.magnitude );
	}

    private void Update_Rotation()
    {
        //If camera already at the right position
        if (IsInTransition() == false)
            return;

        for (int i = 0; i < 4; ++i)
        {
            if (m_oDelayRotation[i] == 0f)
                continue;

            m_oElapsedTimeRotation[i] += Time.deltaTime;
            float fRatio = m_oElapsedTimeRotation[i] / m_oDelayRotation[i];
            m_oCurrentRotation[i] = Utils.EaseOutQuad(m_oStartRotation[i], m_oTargetRotation[i], fRatio);
        }

        UpdateTransform();
    }

    private bool IsInTransition()
    {
        return Utils.AreAquals(m_oCurrentRotation, m_oTargetRotation) == false;
    }

    private void UpdateTransform()
    {
        GameObject oPlayer = GameManager.Player;
        if (oPlayer == null)
            return;

        Vector3 oOffsetTranslationToPlayer = GetOffsetTranslationToPlayer();
        Vector3 oPlayerPositionWithOffset = oPlayer.transform.position - oOffsetTranslationToPlayer;

        //set the camera at the player position
        transform.position = oPlayerPositionWithOffset;

        //set start position
        transform.eulerAngles = new Vector3(90f, 0f, 0f);

        //set the rotation
        transform.RotateAround(oPlayerPositionWithOffset, Vector3.right, m_oCurrentRotation[0]);
        transform.RotateAround(oPlayerPositionWithOffset, Vector3.up, m_oCurrentRotation[1]);
        transform.RotateAround(oPlayerPositionWithOffset, Vector3.forward, m_oCurrentRotation[2]);

        //look at the player
        transform.LookAt(oPlayerPositionWithOffset);

        transform.position = transform.position + (m_oCurrentRotation[3] * -transform.forward);

        //set the new target translation to player
        m_oTargetTranslationToPlayer = oPlayerPositionWithOffset - transform.position;
    }

    private Vector3 GetOffsetTranslationToPlayer()
	{ 
        GameObject player = GameObject.Find("Player");
        Vector3 oCurrentTranslationToPlayer = Utils.GetTranslationTo(gameObject, player);

        return oCurrentTranslationToPlayer - m_oTargetTranslationToPlayer;
	}
}
