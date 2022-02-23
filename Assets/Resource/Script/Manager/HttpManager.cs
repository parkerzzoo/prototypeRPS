using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Boomlagoon.JSON;
using System;
using ParkerLibrary.EventSystem;

public class HttpManager : Singletone<HttpManager> 
{
	public string customUrl;

	bool CheckNoInternet(int requestId = -1)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			//S_ConnectionIssuePanel.Instance.ConnectionIssue (ConnectionIssueType.NoInternet, "", requestId);
			return true;
		}
		return false;
	}

	void CheckCustomServerError(JSONObject _loadedData, Action<JSONObject> _callback, int requestId)
	{
		if (_loadedData.ContainsKey("error") && !_loadedData.GetBoolean("error"))
		{
			Debug.Log("Completed loading - json data");
			// 성공적으로 응답받았을경우 요청 리스트에서 지워줌.
			HttpRequest.SuccessResponsed(requestId);
			if (_callback != null)
				_callback(_loadedData);
		}
		else
        {
			// 성공적으로 응답받았을경우 요청 리스트에서 지워줌.
			HttpRequest.SuccessResponsed(requestId);
			if (_callback != null)
				_callback(_loadedData);
		}
	}

	public void RequestGetHttpPostMethod(string _url, string _urlTail,
		Dictionary<string, string> _postData, Action<JSONObject> callback, Action errorCallback = null)
	{
		int _requestId = HttpRequest.CreateId();
		new HttpRequest(_requestId, ()=>
			StartCoroutine(RequestHttpPostMethodCor(_url, _urlTail, _postData, callback, errorCallback, _requestId)), _url);
		HttpRequest.Invoke(_requestId);
	}

	public void RequestHttpGetMethod(string _url, string _urlTail,
		Dictionary<string, string> _parameter, Action<JSONObject> callback, Action errorCallback = null)
	{
		int _requestId = HttpRequest.CreateId();
		new HttpRequest(_requestId, ()=>
			StartCoroutine(RequestHttpGetMethodCor(_url, _urlTail, _parameter, callback, errorCallback, _requestId)), _url);
		HttpRequest.Invoke(_requestId);
	}

	public IEnumerator RequestHttpPostMethodCor(string _url, string _urlTail,
		Dictionary<string, string> _postData, Action<JSONObject> callback, Action errorCallback, int requestId = -1)  
	{
		// 인터넷 미연결시 재시도 누를때 연결 팝업 켜지는것과 같은 프레임에 실행되면.
		// 연결안했을때 팝업이 다시뜨지않으므로 한프레임뒤에 다시 체크해줌.
		yield return null;
		if(CheckNoInternet(requestId))
			yield break;

		if(_urlTail != null)
			_url += _urlTail + "/";

		UnityWebRequest request;
		// 로우데이터가 아닌경우. 폼 클래스에 데이터를 넣고 보냄.
		WWWForm form = new WWWForm ();
		if(_postData != null)
			foreach(var i in _postData) form.AddField (i.Key, i.Value);
		request =  UnityWebRequest.Post(_url, form);

		request.timeout = VarList.serverTimeoutValue;
		Debug.Log("post method - " + _url);
		yield return StartCoroutine(HandleRequest(request, (isSuccess) => {

			if(isSuccess)
			{
				JSONObject _loadedData = JSONObject.Parse(DownloadHandlerBuffer.GetContent(request));
				Debug.Log(JSONObject.Parse(DownloadHandlerBuffer.GetContent(request)));
				CheckCustomServerError(_loadedData, callback, requestId);
			}
			else
			{
				errorCallback?.Invoke();
				// 네트워크 에러 (ex. 타임아웃)로 인해 요청 실패할경우 일정횟수 이상 실패하면 게임재시작. 그렇지않으면 재시도.
				/*if (HttpRequest.GetTryCount(requestId) >= VarList.httpRetryCount)
					S_ConnectionIssuePanel.Instance.ConnectionIssue (ConnectionIssueType.Restart, LanguageManager.Instance.translate("이슈_오류재시작멘트"));
				else
					S_ConnectionIssuePanel.Instance.ConnectionIssue (ConnectionIssueType.Retry, LanguageManager.Instance.translate("이슈_오류재시도멘트"), requestId);*/
			}

		}, requestId));
	}  

	public IEnumerator RequestHttpGetMethodCor(string _url, string _urlTail,
		Dictionary<string, string> _parameter, Action<JSONObject> callback, Action errorCallback, int requestId = -1)
	{
		// 인터넷 미연결시 재시도 누를때 연결 팝업 켜지는것과 같은 프레임에 실행되면.
		// 연결안했을때 팝업이 다시뜨지않으므로 한프레임뒤에 다시 체크해줌.
		yield return null;
		if(CheckNoInternet(requestId))
			yield break;

		if (_urlTail != null)
			_url += _urlTail;

		if(_parameter != null)
		{
			_url += "?";
			foreach(var i in _parameter)
				_url += i.Key + "=" + i.Value + "&";
			_url = _url.Remove(_url.Length - 1);
		}
		UnityWebRequest request = UnityWebRequest.Get(_url);
		request.timeout = VarList.serverTimeoutValue;
		Debug.Log("get method - " + _url);
		yield return StartCoroutine(HandleRequest(request, (isSuccess) => {

			if(isSuccess)
			{
				JSONObject _loadedData = JSONObject.Parse(DownloadHandlerBuffer.GetContent(request));
					
				CheckCustomServerError(_loadedData, callback, requestId);
			}
			else
			{
				errorCallback?.Invoke();
				// 네트워크 에러 (ex. 타임아웃)로 인해 요청 실패할경우 일정횟수 이상 실패하면 게임재시작. 그렇지않으면 재시도.
				/*if(HttpRequest.GetTryCount(requestId) >= VarList.httpRetryCount)
					S_ConnectionIssuePanel.Instance.ConnectionIssue (ConnectionIssueType.Restart, LanguageManager.Instance.translate("이슈_오류재시작멘트"));
				else
					S_ConnectionIssuePanel.Instance.ConnectionIssue (ConnectionIssueType.Retry, LanguageManager.Instance.translate("이슈_오류재시도멘트"), requestId);*/
			}

		}, requestId));
	}  

	public IEnumerator GetTexture(string _url, Action<Texture2D> callback)  
	{
		// 인터넷 미연결시 재시도 누를때 연결 팝업 켜지는것과 같은 프레임에 실행되면.
		// 연결안했을때 팝업이 다시뜨지않으므로 한프레임뒤에 다시 체크해줌.
		yield return null;
		if(CheckNoInternet())
			yield break;
		UnityWebRequest request = UnityWebRequestTexture.GetTexture(_url);
		request.timeout = VarList.serverTimeoutValue;
		yield return StartCoroutine(HandleRequest(request, (isSuccess) => 
			{
				if(isSuccess)
				{
					Texture2D _loadedTexture = DownloadHandlerTexture.GetContent(request);
					if(callback != null) callback.Invoke(_loadedTexture );
				}
				// 텍스쳐 불러오는건 그다지 큰 요청이 아니므로 실패해도 다른행동안함.무시이.
			}));
	}  

	public IEnumerator HandleRequest(UnityWebRequest request, Action<bool> callback, int requestId = -1)
	{
		using(request)
		{
			request.SendWebRequest();
			float _timer = 0f;
			while(!request.isDone)
			{
				// 진행도가 안올라가고 있으면 게이지가 멈춰보이니까, 진행도가 실제로 안올라가더라도 시간에따라 게이지가 올라가보이게하기위함.
				//M_EventManager.Instance.PostNotification (EVENT_TYPE.LOADING_DATA_FROM_SERVER, this, 
				//	request.downloadProgress > _timer? request.downloadProgress: _timer);
				_timer += (Time.deltaTime) * 0.1f; // 여기에 곱해지는 숫자가 높을수록 기본 게이지 상승이 빠른것.
				_timer = _timer >= 1f? 1f: _timer;
				yield return null;
			}
			//M_EventManager.Instance.PostNotification (EVENT_TYPE.LOADED_DATA_FROM_SERVER, this, null);
			if(request.result == UnityWebRequest.Result.ConnectionError)
			{
				//M_LogEventManager.Instance.SendBugReport(request.error, requestId < 0? "empty": HttpRequest.GetUrl(requestId));
				Debug.Log("Network error - code : "+ request.error);
			}
			// 네트워크 에러인지 아닌지 인자에 전달해서 콜백실행. 네트워크에러의 대표적인 예는 타임아웃.
			// 타임아웃걸렸을땐 일정 횟수이상 재시도후 게임 다시시작함. 예외도있음 예를들어 텍스쳐 불러오거나 에셋번들 불러올때.
			// 텍스쳐 불러올때는 크게 중요한게 아니므로 네트워크에러나도 무시.
			// 에셋번들 불러올땐 어차피 게임 첫시작부분이므로 무조건 재시작.
			if(callback != null) callback.Invoke(!(request.isNetworkError));
		}
	}
}