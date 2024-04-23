using UnityEngine;
using System.IO;
using System;
using System.Net;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ImageDownloader : MonoBehaviour
{
    public Image[] imageContainers; // Array of Image UI components to display the downloaded images

    void OnEnable()
    {
        // Path to the images.txt file
        string imagesFilePath = Path.Combine(Application.dataPath, "Resources/images.txt");

        // Check if the file exists
        if (File.Exists(imagesFilePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(imagesFilePath);

            // Download and display the first three images
            for (int i = 0; i < Mathf.Min(3, lines.Length); i++)
            {
                string imageUrl = lines[i].Trim();
                StartCoroutine(DownloadImage(imageUrl, i));
            }
        }
        else
        {
            Debug.LogError("images.txt file not found in Resources folder.");
        }
    }

    IEnumerator DownloadImage(string url, int index)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            // Send request
            yield return www.SendWebRequest();

            // Check for errors
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to download image {index} from URL: {url}. Error: {www.error}");
            }
            else
            {
                // Get downloaded texture
                Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                // Create sprite from texture
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                // Display sprite on Image component
                if (index < imageContainers.Length && imageContainers[index] != null)
                {
                    imageContainers[index].sprite = sprite;
                }
                else
                {
                    Debug.LogError($"Image container not assigned for image {index}. Make sure to assign Image components in the inspector.");
                }

            }
        }
    }
}