// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerManager.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in PUN Basics Tutorial to deal with the networked player instance
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


namespace Photon.Pun.Demo.PunBasics
{
#pragma warning disable 649

    /// <summary>
    /// Player manager.
    /// Handles fire Input and Beams.
    /// </summary>
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public Fields

        [Tooltip("The current Health of our player")]
        public int Score = 0;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        public bool photon_ismine = false;
        #endregion

        #region Private Fields

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        public void Awake()
        {

            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instanciation when levels are synchronized
            if (photonView.IsMine)
            {
                photon_ismine = true;
                LocalPlayerInstance = gameObject;
                // ¾Æ±º
            }
            if (PhotonNetwork.IsMasterClient && photon_ismine || !PhotonNetwork.IsMasterClient && !photon_ismine)
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
            else
            {   
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        public void Start()
        {
            // Create the UI
            

#if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        }


        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable();

#if UNITY_5_4_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
#endif
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// Process Inputs if local player.
        /// Show and hide the beams
        /// Watch for end of game, when local player health is 0.
        /// </summary>
        public void Update()
        {
            // we only process Inputs and check health if we are the local player
          

        }
        IEnumerator OnMouseDown()
        {
            if (photon_ismine)
            {
                Vector3 scrSpace = Camera.main.WorldToScreenPoint(transform.position);
                Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, scrSpace.z));

                while (Input.GetMouseButton(0))
                {
                    Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, scrSpace.z);

                    Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
                    transform.position = curPosition;
                    yield return null;
                }
            }
        }
        /// <summary>
        /// MonoBehaviour method called when the Collider 'other' enters the trigger.
        /// Affect Health of the Player if the collider is a beam
        /// Note: when jumping and firing at the same, you'll find that the player's own beam intersects with itself
        /// One could move the collider further away to prevent this or check if the beam belongs to the player.
        /// </summary>

#if !UNITY_5_4_OR_NEWER
        /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
#endif


        /// <summary>
        /// MonoBehaviour method called after a new level of index 'level' was loaded.
        /// We recreate the Player UI because it was destroy when we switched level.
        /// Also reposition the player if outside the current arena.
        /// </summary>
        /// <param name="level">Level index loaded</param>
        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            /*if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }*/

        }

        #endregion

        #region Private Methods


#if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
#endif

        /// <summary>
        /// Processes the inputs. This MUST ONLY BE USED when the player has authority over this Networked GameObject (photonView.isMine == true)
        /// </summary>
        #endregion

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(this.Score);
            }
            else
            {
                // Network player, receive data
                this.Score = (int)stream.ReceiveNext();
            }
        }

        #endregion
    }
}