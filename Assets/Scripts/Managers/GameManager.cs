using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;        
    public float m_StartDelay = 3f;         
    public float m_EndDelay = 3f;           
    public CameraControl m_CameraControl;   
    public Text m_MessageText;
    public GameObject m_TankPrefab;         
    public TankManager[] m_Tanks;
    public GameObject patroller;


    private int m_RoundNumber;              
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;       
    private TankManager m_RoundWinner;
    private TankManager m_GameWinner;       

    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }


    private void SpawnAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].Setup();
        }

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_PlayerNumber == 1)
            {
                for (int j = 0; j < m_Tanks.Length; j++)
                {
                    if(m_Tanks[j].m_PlayerNumber == 2)
					{
                        m_Tanks[i].m_Shooting.target = m_Tanks[j].m_Instance;
                        m_Tanks[i].m_Movement.target = m_Tanks[j].m_Instance;

                        BehaviorExecutor m_behaviour = m_Tanks[i].m_Instance.GetComponent<BehaviorExecutor>();
                        m_behaviour.blackboard.SetBehaviorParam("myself", m_Tanks[i].m_Instance);
                        m_behaviour.blackboard.SetBehaviorParam("target", m_Tanks[j].m_Instance);
                    }
                }
            }
			else if(m_Tanks[i].m_PlayerNumber == 2)
			{
                for(int j = 0;j<m_Tanks.Length;j++)
				{
                    if(m_Tanks[j].m_PlayerNumber == 1)
					{
                        m_Tanks[i].m_Shooting.target = m_Tanks[j].m_Instance;
                        m_Tanks[i].m_Movement.target = m_Tanks[j].m_Instance;

                        BehaviorExecutor m_behaviour = m_Tanks[i].m_Instance.GetComponent<BehaviorExecutor>();
                        m_behaviour.blackboard.SetBehaviorParam("myself", m_Tanks[i].m_Instance);
                        m_behaviour.blackboard.SetBehaviorParam("target", m_Tanks[j].m_Instance);
                    }
                }
			}
        }
    }


    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[m_Tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = m_Tanks[i].m_Instance.transform;
        }

        m_CameraControl.m_Targets = targets;
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (m_GameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        patroller.GetComponent<Patrol>().ResetPatrol();
        DisableTankControl();

        m_CameraControl.SetStartPositionAndSize();

        m_RoundNumber++;
        m_MessageText.text = "ROUND " + m_RoundNumber + "!";

        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        EnableTankControl();
        patroller.GetComponent<Patrol>().active = true;

        m_MessageText.text = string.Empty;

        while (!OneTankLeft())
        {
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        DisableTankControl();
        patroller.GetComponent<Patrol>().active = false;

        m_RoundWinner = null;

        m_RoundWinner = GetRoundWinner();

        if(m_RoundWinner != null)
        {
            m_RoundWinner.m_Wins++;
        }

        m_GameWinner = GetGameWinner();

        m_MessageText.text = EndMessage();

        yield return m_EndWait;
    }


    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }


    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                return m_Tanks[i];
        }

        return null;
    }


    private TankManager GetGameWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
        }

        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }
}