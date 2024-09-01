using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CambiarEscenas : MonoBehaviour
{

    private void Start() {
        
    }

    public void cambiarEscena(string nombre){
        SceneManager.LoadScene(nombre);
    }
}
