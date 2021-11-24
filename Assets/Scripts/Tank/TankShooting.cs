using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public GameObject tankTurret;
    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public AudioSource m_ShootingAudio;
    public AudioClip m_FireClip;
    public float m_LaunchForce = 30f;
    public float delayTime = 0.0f;
    public int ammo = 5;
    [HideInInspector] public GameObject target;

    private bool m_CanFire;
    private float dt = 0.0f;

    private float prevAngle = 0.0f;
    private float angle = 0.0f;
    private float prevTankAngle = 0.0f;
    private float tankAngle = 0.0f;
    private float g = -Physics.gravity.y;

    private void Start()
    {
        m_CanFire = true;
        angle = -1;
    }

    private void Update()
    {
        Vector3 posVector = target.transform.position - this.transform.position;
        posVector.x = Mathf.Abs(posVector.x);
        posVector.y = Mathf.Abs(posVector.y);
        posVector.z = Mathf.Abs(posVector.z);
        float x = posVector.magnitude;
        //Debug.Log(x);

		float vel2 = Mathf.Pow(m_LaunchForce, 2);
		float? angle1 = Mathf.Atan((vel2 + Mathf.Sqrt(vel2 * vel2 - g * g * x * x)) / (g * x)) * Mathf.Rad2Deg;
		float? angle2 = Mathf.Atan((vel2 - Mathf.Sqrt(vel2 * vel2 - g * g * x * x)) / (g * x)) * Mathf.Rad2Deg;

		if (angle1 != null && angle2 != null)
        {
            prevAngle = angle;
            if (angle1 <= angle2)
            {
                angle = (float)angle1;
            }
            else
            {
                angle = (float)angle2;
            }
            tankTurret.transform.Rotate(Vector3.left, angle - prevAngle);
        }


        if (angle != -1 && m_CanFire && posVector.magnitude <= 25.0f)
        {
            //tankTurret.transform.LookAt(target.transform.position);
            Fire();
        }
        else
        {
            dt += Time.deltaTime;
            if (dt >= delayTime)
            {
                dt = 0.0f;
                m_CanFire = true;
            }
        }
    }

    private void Fire()
    {
        m_CanFire = false;

        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation);
        //shellInstance.velocity = m_LaunchForce * tankTurret.transform.forward;
        shellInstance.AddForce(m_LaunchForce * 25.0f * tankTurret.transform.forward);

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        ammo--;
    }
}