using Microsoft.VisualBasic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;



public class message : MonoBehaviour
{

    [SerializeField] Text[] messages;
    [SerializeField] GameObject messagePanel;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject worlds;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Validate()
    {

        string textMessage = "";
        foreach (Text message in messages)
        {
            textMessage += message.text + "\n";
        }



        GameObject newMessage = Instantiate(messagePanel);
        newMessage.transform.position = Player.transform.position;

        foreach (Transform world in worlds.transform)
        {
            if (world.gameObject.activeSelf)
            {
                newMessage.transform.SetParent(world);
            }
        }
        newMessage.GetComponent<Dialog>().MessageParTerre(textMessage);

        StartCoroutine(SendMessage(newMessage));


    }

    IEnumerator SendMessage(GameObject message)
    {
        WWWForm form = new WWWForm();
        Debug.Log(message.GetComponent<Dialog>().m_dialogWithPlayer[0].text);
        form.AddField("content", message.GetComponent<Dialog>().m_dialogWithPlayer[0].text);
        form.AddField("x", message.transform.position.x.ToString());
        form.AddField("y", message.transform.position.y.ToString());
        form.AddField("z", message.transform.position.z.ToString());
        form.AddField("world", message.transform.parent.name);

        using (UnityWebRequest www = UnityWebRequest.Post("https://clementvigier.alwaysdata.net/Zelda_like/insertMessage.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }

        }
    }
}
