using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Baraja : MonoBehaviour
{
    
    [Header("Cartas")]
    public List<GameObject> cartas; // Array inicial de sprites de cartas
    private List<GameObject> deck = new List<GameObject>(); // Lista dinámica de cartas disponibles
    private List<GameObject> usedCards = new List<GameObject>(); // Lista de cartas usadas
    public GameObject[] cardGameObjects; // GameObjects que representan las cartas en la escena

    [Header("GameController")]
    private cartaView newSprite;
    private UserController userController;
    private EnemyAI enemyAI;

    public GameController gameController;

    void Start()
    {
        // Inicializar el deck con las cartas iniciales
        InicializarDeck();
        // Llenar las cartas en los GameObjects
        LlenarCartas();



    }

    private void Update()
    {
        if (ComprobarMano())
        {
            RepartirNuevamente();

            Score();

            gameController.ActivarTurnoJugador(true);

        }
    }

    private void Score()
    {
        GameObject user = GameObject.FindGameObjectWithTag("User");
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");

        userController = user.GetComponent<UserController>();
        enemyAI = enemy.GetComponent<EnemyAI>();

        gameController.AddScorePlayer(userController.LeerCantos());
        gameController.AddScoreEnemy(enemyAI.LeerCantos());
    }

    // Método para inicializar el deck con las cartas iniciales
    void InicializarDeck()
    {
        deck.AddRange(cartas);
    }

    // Método para llenar las cartas en los GameObjects
    void LlenarCartas()
    {
        for (int i = 0; i < cardGameObjects.Length; i++)
        {


            if (deck.Count > 0)
            {
                AsignarCarta(i);
            }


        }
        Score();
    }

    private void AsignarCarta(int i)
    {
        // Asigna una carta aleatoria del deck al GameObject
        if (cardGameObjects[i].transform.childCount == 0)
        {
            int randomIndex = Random.Range(0, deck.Count);
            GameObject carta = deck[randomIndex];
            //Debug.Log("carta " + carta.name);
            deck.RemoveAt(randomIndex);
            usedCards.Add(carta);


            Vector2 position = cardGameObjects[i].transform.position;
            GameObject instancia = Instantiate(carta, position, Quaternion.identity, cardGameObjects[i].transform);

            newSprite = instancia.GetComponent<cartaView>();

            if (cardGameObjects[i].transform.parent.name == "Cards2")
            {
                // Obtener el SpriteRenderer del objeto instanciado
                SpriteRenderer sp = instancia.GetComponent<SpriteRenderer>();
                if (sp != null)
                {

                    sp.sprite = newSprite.backSprite;
                }
            }


        }
    }

    private void RepartirNuevamente()
    {
        if (deck.Count == 0)
        {
            gameController.LlevarMesa();
            
            if (gameController.pilaP - 20 > 0)
            {

                gameController.AddScorePlayer(gameController.pilaP - 20);
                
            } else if (gameController.pilaE - 20 > 0){

                gameController.AddScoreEnemy(gameController.pilaE - 20);
                
            }
            gameController.pilaP = 0;
            gameController.pilaE = 0;

            gameController.msg.text = "";
            Invoke("ReponerDeck", 5.0f);
        }

        if (deck.Count == 6)
        {
            gameController.msg.text = "Ultimas";
        }

        for (int j = 0; j < 6; j++)
        {
            if (cardGameObjects[j].transform.childCount == 0 && deck.Count > 0)
            {
                int randomIndex = Random.Range(0, deck.Count);
                GameObject carta = deck[randomIndex];
                deck.RemoveAt(randomIndex);
                usedCards.Add(carta);

                Vector2 position = cardGameObjects[j].transform.position;
                GameObject instancia = Instantiate(carta, position, Quaternion.identity, cardGameObjects[j].transform);

                newSprite = instancia.GetComponent<cartaView>();
                if (cardGameObjects[j].transform.parent.name == "Cards2")
                {
                    SpriteRenderer sp = instancia.GetComponent<SpriteRenderer>();
                    if (sp != null)
                    {
                        sp.sprite = newSprite.backSprite;
                    }
                }
            }
        }



    }

    private bool ComprobarMano()
    {
        for (int i = 0; i < 6; i++)
        {
            if (cardGameObjects[i].transform.childCount != 0)
            {
                return false;
            }
        }
        return true;
    }


    // Método para reponer el deck con las cartas usadas
    void ReponerDeck()
    {
        if (deck.Count == 0)
        {
            deck.AddRange(usedCards);
            usedCards.Clear();

            LlenarCartas();
            Debug.Log("Deck repuesto con cartas usadas.");
        }
    }


}
