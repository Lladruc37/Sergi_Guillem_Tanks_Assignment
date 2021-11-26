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
    public float rotationSpeed = 1.5f;
    public int ammo = 5;
    [HideInInspector] public GameObject target;

    private bool m_CanFire;
    private float dt = 0.0f;

    private float angle = 0.0f;
    private Quaternion currentRot;
    private float g = -Physics.gravity.y;
    public BehaviorExecutor m_behaviour;

    private void Start()
    {
        m_CanFire = true;
        angle = -1;
        currentRot = tankTurret.transform.rotation;
    }

    private void Update()
    {
        Vector3 posVector = target.transform.position - this.transform.position;
        float x = posVector.magnitude;
        Quaternion orientation = Quaternion.LookRotation(new Vector3(posVector.x, 0, posVector.z));

		float vel2 = Mathf.Pow(m_LaunchForce, 2);
		float? angle1 = Mathf.Atan((vel2 + Mathf.Sqrt(vel2 * vel2 - g * g * x * x)) / (g * x)) * Mathf.Rad2Deg;
		float? angle2 = Mathf.Atan((vel2 - Mathf.Sqrt(vel2 * vel2 - g * g * x * x)) / (g * x)) * Mathf.Rad2Deg;

        Quaternion angles = Quaternion.identity;
        if (angle1 != null && angle2 != null)
        {
            if (angle1 <= angle2)
            {
                angle = (float)angle1;
            }
            else
            {
                angle = (float)angle2;
            }
            angles.eulerAngles = new Vector3(360 - angle, 0, 0);
        }

        if (posVector.magnitude <= 25.0f)
		{
            currentRot = orientation * angles;
            //if (angle != -1 && m_CanFire)
            //{
            //    Fire();
            //}
            //else
            //{
            //    if (dt >= delayTime)
            //    {
            //        dt = 0.0f;
            //        m_CanFire = true;
            //    }
            //}
        }
        else
		{
            currentRot = this.transform.rotation;
        }

        tankTurret.transform.rotation = Quaternion.Slerp(tankTurret.transform.rotation, currentRot, Time.deltaTime * rotationSpeed); //slerp
        dt += Time.deltaTime;
    }

    public void Fire()
    {
        m_CanFire = false;

        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation);
        shellInstance.AddForce(m_LaunchForce * m_LaunchForce * tankTurret.transform.forward);

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        ammo--;
        BehaviorExecutor m_behaviour = this.GetComponent<BehaviorExecutor>();
        m_behaviour.blackboard.SetBehaviorParam("ammo", this.ammo);
    }
}