using System;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public int puntP, puntE, pilaP, pilaE;
    public TMP_Text puntPlayer, puntEnemy, msg, t1, t2, t3, t4;
    public UserController[] userControllers;
    public EnemyAI[] enemyAIs;
    public int currentEnemyIndex = 0; // Index to track which enemy is playing
    public bool isPlayerTurn = true, ultima;
    public GameObject victoria, derrota;

    void Start()
    {
        ActivarTurnoJugador(isPlayerTurn); // Start by activating the player's turn

    }

    private void Update()
    {
        HandleEndGame();
    }

    // void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    public void LlevarMesa()
    {
        if (ultima)
        {
            userControllers[0].UltimasEnMesa();
        }
        else
        {
            enemyAIs[0].UltimasEnMesa();
        }
    }

    public void AddScorePlayer(int points)
    {
        puntP += points;
        puntPlayer.text = puntP.ToString();

    }

    public void AddScoreEnemy(int points)
    {
        puntE += points;
        puntEnemy.text = puntE.ToString();

    }

    public void ActivarTurnoJugador(bool activar)
    {
        foreach (var userController in userControllers)
        {
            userController.isTurn = activar;
            if (activar) userController.JugarUser();
        }

    }

    public void ActivarTurnoEnemigo()
    {
        if (currentEnemyIndex <= enemyAIs.Length)
        {
            enemyAIs[currentEnemyIndex].isTurn = true;
            enemyAIs[currentEnemyIndex].Invoke("JugarEnemy", 1.0f);
        }
    }


    public void TerminarTurno()
    {
        if (isPlayerTurn)
        {
            // Si es el turno del jugador, pasarlo al enemigo

            foreach (var userController in userControllers)
            {
                userController.isTurn = false;
            }

            isPlayerTurn = false;
            ActivarTurnoEnemigo();
        }
        else
        {
            // turno del enemigo
            if (currentEnemyIndex <= enemyAIs.Length)
            {
                enemyAIs[currentEnemyIndex].isTurn = false; // Finaliza el turno actual del enemigo
                currentEnemyIndex++;
                Debug.Log("Index: " + currentEnemyIndex);
                isPlayerTurn = true;
                ActivarTurnoJugador(isPlayerTurn);
            }


        }

        if (currentEnemyIndex >= enemyAIs.Length)
        {
            Debug.Log("Esta entrando");
            currentEnemyIndex = 0;

        }
        // else
        // {
        //     isPlayerTurn = true;
        //     ActivarTurnoJugador(isPlayerTurn);
        // }
    }

    public void HandleEndGame()
    {
        if (puntP >= 24)
        {
            t1.text = "Jugador: " + puntP.ToString() + " pts";
            t2.text = "CPU: " + puntE.ToString() + " pts";
            victoria.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("Jugador ha ganado");
        }
        else if (puntE >= 24)
        {
            t3.text = "CPU: " + puntE.ToString() + " pts";
            t4.text = "Jugador: " + puntP.ToString() + " pts";
            derrota.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("CPU ha ganado");
        }
    }

    // public void RestartGame()
    // {
    //     // Reanudar el juego
    //     Time.timeScale = 1f;

    //     // Reiniciar el juego (puedes recargar la escena, por ejemplo)
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    // }
}


