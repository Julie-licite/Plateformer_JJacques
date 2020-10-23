using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    //recup la prefab de mon loadingscreen
    [SerializeField] private GameObject loadingScreen;
    //creer un espace pour inserer la scene a jouer
    [SerializeField] private string sceneToLoadName;

    //je charge la scene
    public void LoadScene()
    {
        StartCoroutine(LoadSceneCoroutine());
    }
   

    private IEnumerator LoadSceneCoroutine()
    {
        // fais apparaitre le loading screen
        var loadingScreenInstance = Instantiate(loadingScreen);

        // Garder le loading screen à l'ecran (ne pas le detruire)
        DontDestroyOnLoad(loadingScreenInstance);

        // recuperer l'animator du loading screen
        var loadingAnimator = loadingScreenInstance.GetComponent<Animator>();

        // variable pour qu'on detecte si l'animation est fini
        var currentAnimTime = loadingAnimator.GetCurrentAnimatorStateInfo(0).length;

        // Commencer a charger la scene en arriere plan
        var loading = SceneManager.LoadSceneAsync(sceneToLoadName);

        // desactive le lancement automatique pour laisser plus de temps a l'anim 
        loading.allowSceneActivation = false;

        // s'effectue pendant le chargement de la scene
        while (!loading.isDone)
        {
            // si la scene est lancé a 100%
            if (loading.progress >= 0.9f)
            {
                Debug.Log("lance");
                // lance l'animation de disparition smooth
                loadingAnimator.SetTrigger("Dissapearing");

                // fais apparaitre la scene mise dans l'espace sur unity
                loading.allowSceneActivation = true;
            }
            // attendre la fin de l'animation pour lancer la scene
            yield return new WaitForSeconds(currentAnimTime);

            
        }
    }
       
}
