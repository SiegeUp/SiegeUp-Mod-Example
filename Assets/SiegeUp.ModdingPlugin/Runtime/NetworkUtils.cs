using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace SiegeUp.ModdingPlugin
{
	public static class NetworkUtils
	{
		/// <summary>
		/// Download file from url.
		/// </summary>
		/// <param name="url"></param>
		/// <param name="filePath"></param>
		/// <returns>
		/// True if file downloaded successfully, false otherwise.
		/// </returns>
		public static IEnumerator TryDownloadFile(string url, string filePath, Action<bool> callback)
		{
			using (var request = new UnityWebRequest(url))
			{
				request.downloadHandler = new DownloadHandlerFile(filePath);
				var operation = request.SendWebRequest();
				yield return new WaitUntil(() => operation.isDone);
				if (request.isNetworkError || request.isHttpError)
				{
					Debug.LogError(request.error);
					callback(false);
					yield break;
				}
				Debug.Log("File successfully downloaded and saved to " + filePath);
				callback(true);
			}
		}

		/// <summary>
		/// Get data from url.
		/// </summary>
		/// <param name="url"></param>
		/// <returns>
		/// Data from the url or null if unable to get data.
		/// </returns>
		public static IEnumerator GetData(string url, Action<string> callback)
		{
			using (var request = UnityWebRequest.Get(url))
			{
				var operation = request.SendWebRequest();
				yield return new WaitUntil(() => operation.isDone);
				if (request.isNetworkError || request.isHttpError)
				{
					Debug.LogError(request.error);
					callback("");
					yield break;
				}
				callback(request.downloadHandler.text);
			}
		}

		public static string GetFileNameFromUrl(string url)
		{
			if (!url.StartsWith("http"))
				url = "https://" + url;
			var uri = new Uri(url);
			return uri.Segments.Last();
		}
	}
}
