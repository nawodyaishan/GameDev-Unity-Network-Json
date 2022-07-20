using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownloadData : MonoBehaviour
{
    [SerializeField] private string url;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    // Album Data List
    private AlbumData[] _allAlbums;
    [SerializeField] private RawImage loadedImage;

    [System.Serializable]
    public class AlbumData
    {
        public int albumId;
        public int id;
        public string title;
        public string url;
        public string thumbnailUrl;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GetAlbums());
        }
    }

    private IEnumerator GetAlbums()
    {
        using (UnityWebRequest getRequest = UnityWebRequest.Get(url))
        {
            yield return getRequest.SendWebRequest();

            if (getRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Successfully Completed the request. Downloaded the Text");

                _allAlbums = JsonHelper.GetArray<AlbumData>(getRequest.downloadHandler.text);
                StartCoroutine(GetAlbumData());


                // var jsonText = getRequest.downloadHandler.text;
                // AlbumData newPhoto = JsonUtility.FromJson<AlbumData>(jsonText);
                // textMeshProUGUI.text = newPhoto.albumId.ToString();
            }
            else
            {
                Debug.LogError(getRequest.error);
            }
        }
    }

    IEnumerator GetAlbumData()
    {
        var firstData = _allAlbums[0];
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(firstData.thumbnailUrl);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.Log(request.error);
        else
        {
            loadedImage.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            Debug.Log("Successfully Completed the request. Image Loaded");
        }
    }
}