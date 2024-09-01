using System;
using UnityEngine;
using TMPro;

public class EnemyAI : MonoBehaviour
{
    public GameObject[] target;
    private cartaView carta1, carta2;
    public Boolean isTurn;
    GameObject[] canto;
    public int index = 0;
    public GameController gameController;
    public GameObject cartaPrefab;
    private GameObject pila;
    public TMP_Text msg, cantoM;

    void Start()
    {
        pila = GameObject.FindGameObjectWithTag("PilaE");
        
    }

    public int LeerCantos()
    {
        EnemyAI _c1, _c2, _c3;
        cartaView c1, c2, c3;
        canto = GameObject.FindGameObjectsWithTag("Enemy");


        if (canto != null)
        {

            _c1 = canto[0].GetComponent<EnemyAI>();
            _c2 = canto[1].GetComponent<EnemyAI>();
            _c3 = canto[2].GetComponent<EnemyAI>();

            Transform card1 = _c1.transform.GetChild(0);
            Transform card2 = _c2.transform.GetChild(0);
            Transform card3 = _c3.transform.GetChild(0);

            c1 = card1.GetComponent<cartaView>();
            c2 = card2.GetComponent<cartaView>();
            c3 = card3.GetComponent<cartaView>();

            int a = c1.valor;
            int b = c2.valor;
            int c = c3.valor;

            if (a == b && c == a - 1 || a == b && c == a + 1 || a == c && b == c - 1 || a == c && b == c + 1 || b == c && a == b - 1 || b == c && a == b + 1)
            {
                cantoM.text = "Vigia";
                return 7;
            }

            else if (a == b && b != c || a == c && c != b || b == c && c != a)
            {
                cantoM.text = "Ronda";

                if (a == 10 && a == b && b != c || 
                c == 10 && a == c && c != b || 
                b == 10 && b == c && c != a)
                {
                    return 2;
                }
                else if (a == 11 && a == b && b != c || 
                c == 11 && a == c && c != b || 
                b == 11 && b == c && c != a)
                {
                    return 3;
                }
                else if (a == 12 && a == b && b != c || 
                c == 12 && a == c && c != b || 
                b == 12 && b == c && c != a)
                {
                    return 4;
                } 
                else 
                {
                    return 1;
                }
                
            }

            else if (a == b && a == c)
            {
                cantoM.text = "Trivilin";
                return 5;
            }

            else if ((b == a + 1 && c == b + 1) || (b == a - 1 && c == b - 1))
            {
                cantoM.text = "Patrulla";
                return 6;
            }

            else if (a == 1 && b == 11 && c == 12 || a == 11 && b == 12 && c == 1 || a == 12 && b == 1 && c == 11 ||
                    a == 11 && b == 1 && c == 12 || a == 1 && b == 12 && c == 11 || a == 11 && b == 12 && c == 1 ||
                    a == 12 && b == 11 && c == 1)
            {
                cantoM.text = "Registro";
                return 8;
            }

            else
            {
                cantoM.text = "";
                return 0;
                
            }

        } else
        {
            return 0;
        }
    }


    public void JugarEnemy()
    {

        if (!isTurn) return;  // Si no es el turno del enemigo, salir del método.

        GameObject[] prefabs = GameObject.FindGameObjectsWithTag("cards");
        canto = GameObject.FindGameObjectsWithTag("Enemy");

        if (canto[index].transform.childCount == 0) return; 

        Transform cardChild = canto[index].transform.GetChild(0);
        carta1 = cardChild.GetComponent<cartaView>();

        if (carta1 == null) return; // Asegurarse de que carta1 no sea nulo.

        bool matchFound = BuscarCoincidenciaEnMesa(prefabs);

        if (matchFound)
        {
            ProcesarCartasCoincidentes(cardChild, carta1);
        }
        else
        {
            ColocarCartaEnMesa(cardChild, carta1);
        }


    }

    private bool BuscarCoincidenciaEnMesa(GameObject[] prefabs)
    {
        foreach (GameObject prefab in prefabs)
        {
            Transform parentPrefab = prefab.transform.parent;
            if (parentPrefab != null && parentPrefab.CompareTag("Mesa"))
            {
                carta2 = prefab.GetComponent<cartaView>();
                if (carta2 != null && carta1.valor == carta2.valor)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void ProcesarCartasCoincidentes(Transform cardChild, cartaView c1)
    {
        //Mensaje de jugada realizada
        string s = c1.tipo;
        int v = c1.valor;
        
        msg.text = "Jugaste: "+ s + " " + v.ToString() + " y te llevas cartas";

        cardChild.position = carta2.transform.position;
        Destroy(cardChild.gameObject);
        Destroy(carta2.gameObject);
        gameController.pilaE += 2;

        if(pila.transform.childCount == 0) {
            GameObject c = Instantiate(cartaPrefab, pila.transform.position, Quaternion.Euler(0,0,-90));
            c.transform.SetParent(pila.transform);
        }

        GameObject[] prefabs = GameObject.FindGameObjectsWithTag("cards");
        foreach (GameObject prefab in prefabs)
        {
            Transform parentPrefab = prefab.transform.parent;
            if (parentPrefab != null && parentPrefab.CompareTag("Mesa"))
            {
                carta2 = prefab.GetComponent<cartaView>();
                if (carta2 != null)
                {
                    if (carta2.valor - carta1.valor == 1 || (carta2.valor == 10 && carta1.valor == 7))
                    {
                        Destroy(carta2.gameObject);
                        gameController.pilaE += 1;
                    }
                }
            }
        }

        gameController.ultima = false;
        gameController.TerminarTurno();
    }

    private void ColocarCartaEnMesa(Transform cardChild, cartaView c1)
    {
        //Mensaje de jugada realizada
        string s = c1.tipo;
        int v = c1.valor;
        
        msg.text = "Jugaste: "+ s + " " + v.ToString() + " y no te llevas cartas";

        for (int i = 0; i < target.Length; i++)
        {
            if (target[i].transform.childCount == 0)
            {
                SpriteRenderer sp = cardChild.GetComponent<SpriteRenderer>();
                cartaView newSprite = cardChild.GetComponent<cartaView>();
                if (sp != null)
                {
                    sp.sprite = newSprite.faceSprite;
                }
                cardChild.position = target[i].transform.position;
                cardChild.SetParent(target[i].transform);
                break;  // Detener el bucle una vez que la carta ha sido colocada.
            }
        }


        gameController.TerminarTurno();
    }

    public void UltimasEnMesa(){

        for (int i = 0; i < target.Length; i++)
        {
            if (target[i].transform.childCount > 0)
            {
                Transform card = target[i].transform.GetChild(0);
                cartaView c = card.GetComponent<cartaView>();
                Destroy(c.gameObject);
                gameController.pilaE += 1;
            }
        }
       
    }

}
