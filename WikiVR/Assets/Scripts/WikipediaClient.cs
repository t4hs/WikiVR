using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor.VersionControl;
using TMPro;

public class WIkipediaClient : MonoBehaviour
{
    private TcpClient client;
    private Thread clientThread;

    public string serverAddress = "127.0.0.1";
    public int serverPort = 5555;

    public TMP_Text input;

    public void ConnectToServer()
    {
        // Delete the file if it exists
        string filePath = Path.Combine(Application.dataPath, "ReceivedMessage.txt");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        clientThread = new Thread(new ThreadStart(() =>
        {
            try
            {
                client = new TcpClient(serverAddress, serverPort);
                Debug.Log("Connected to server.");

                // Send the input text to the server
                string topic = setTopic();
                SendToServer(topic);

                ReadMessages();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error connecting to server: {e.Message}");
            }
        }));

        clientThread.Start();
    }

    void SendToServer(string message)
    {
        try
        {
            // Get the network stream
            NetworkStream stream = client.GetStream();

            // Convert the message to bytes
            byte[] data = Encoding.UTF8.GetBytes(message);

            // Send the message to the server
            stream.Write(data, 0, data.Length);
            Debug.Log("Sent message to server: " + message);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error sending message to server: {e.Message}");
        }
    }

    void ReadMessages()
    {
        try
        {
            while (true)
            {
                byte[] data = new byte[1024];
                int bytesRead = client.GetStream().Read(data, 0, data.Length);
                if (bytesRead > 0)
                {
                    string message = Encoding.UTF8.GetString(data, 0, bytesRead);
                    SaveMessageToFile(message);

                    // Check if the last 9 characters of the message contain "===END==="
                    if (message.Length >= 9 && message.Substring(message.Length - 9).Contains("===END==="))
                    {
                        SeparateMessage();
                        break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading from server: {e.Message}");
        }
        finally
        {
            client.Close();
        }
    }

    void OnDestroy()
    {
        if (client != null)
        {
            client.Close();
        }
        if (clientThread != null && clientThread.IsAlive)
        {
            clientThread.Abort();
        }
    }

    void SaveMessageToFile(string message)
    {
        string filePath = Path.Combine(Application.dataPath, "ReceivedMessage.txt");

        try
        {
            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.WriteLine(message);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error appending message to file: {e.Message}");
        }
    }

    void SeparateMessage()
    {
        string receivedMessageFilePath = Path.Combine(Application.dataPath, "ReceivedMessage.txt");

        try
        {
            string[] lines = File.ReadAllLines(receivedMessageFilePath);

            string content = "";
            string images = "";
            string links = "";

            bool contentFlag = false;
            bool imagesFlag = false;
            bool linksFlag = false;

            foreach (string line in lines)
            {
                if (line.Contains("===Content==="))
                {
                    contentFlag = true;
                    continue;
                }
                else if (line.Contains("===Images==="))
                {
                    contentFlag = false;
                    imagesFlag = true;
                    continue;
                }
                else if (line.Contains("===Links==="))
                {
                    imagesFlag = false;
                    linksFlag = true;
                    continue;
                }

                if (contentFlag)
                {
                    content += line + "\n";
                }
                else if (imagesFlag)
                {
                    images += line + "\n";
                }
                else if (linksFlag)
                {
                    links += line + "\n";
                }
            }

            // Create Resources folder if it doesn't exist
            string resourcesFolderPath = Path.Combine(Application.dataPath, "Resources");
            if (!Directory.Exists(resourcesFolderPath))
            {
                Directory.CreateDirectory(resourcesFolderPath);
            }

            // Write content to file
            File.WriteAllText(Path.Combine(resourcesFolderPath, "content.txt"), content);

            // Write images to file
            File.WriteAllText(Path.Combine(resourcesFolderPath, "images.txt"), images);

            // Write links to file
            File.WriteAllText(Path.Combine(resourcesFolderPath, "links.txt"), links);

        }
        catch (Exception e)
        {
            Debug.LogError($"Error separating message: {e.Message}");
        }
    }
    String setTopic()
    {
        return input.text;
    }
}