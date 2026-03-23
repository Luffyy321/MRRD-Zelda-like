using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ValidateMessage : MonoBehaviour
{
    [SerializeField] Text[] messages;
    [SerializeField] GameObject messageGO;
    [SerializeField] GameObject player;
    [SerializeField] GameObject worlds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Validate(){
            string messageText = "";
            foreach (Text message in messages){
                messageText += message.text + " ";
            }
            Debug.Log(messageText);
            GameObject newMessage = Instantiate(messageGO);
            newMessage.transform.position = player.transform.position;

            foreach (Transform world in worlds.transform){
                if(world.gameObject.activeSelf){
                    newMessage.transform.parent = world;
                }
            }

            newMessage.GetComponent<Dialog>().MessageParTerre(messageText);
            StartCoroutine(SendMessage(newMessage));

            IEnumerator SendMessage(GameObject message){
                WWWForm form = new WWWForm();
                form.AddField("content", message.GetComponent<Dialog>().m_dialogWithPlayer[0].text);
                form.AddField("x", message.transform.position.x.ToString());
                form.AddField("y", message.transform.position.y.ToString());
                form.AddField("z", message.transform.position.z.ToString());
                form.AddField("world", message.transform.parent.name);

                using var www = UnityWebRequest.Post("https://clementvigier.alwaysdata.net/Zelda_like/insertMessage.php", form);
                yield return www.SendWebRequest();

                if(www.result != UnityWebRequest.Result.Success){
                    Debug.Log(www.error);
                }else{
                    Debug.Log("Form upload complete !");
                }

            }

        }
}
