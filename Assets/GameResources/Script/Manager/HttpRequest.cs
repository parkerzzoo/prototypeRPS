using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HttpRequest
{
	// 액션마다 고유넘버링을 붙여서 어떤 리퀘스트인지 알기위함. 딕셔너리로 접근.
	// 왜이렇게하냐하면... 리퀘스트의 순서가 항상 순서대로이지는 않아서.
	// 예를들면 a리퀘스트 요청후 리스폰이 안왔는데 b리퀘스트를 요청했고, 그이후에 a리퀘스트가 타임아웃이 날수있으니.
	// 이슈가 있는 리퀘스트가 항상 최신 리퀘스트라는 보장이없다. 따라서 id로 식별하여.
	// id를 저장해놓고 해당 id로 핸들링.
	private static int idNumbering = -1;
	public static Dictionary<int, HttpRequest> requestList = new Dictionary<int, HttpRequest>();
	
	// http 요청을 생성. id를만들고 반환해준다. id는 요청의 식별자.
	public static int CreateId()
	{
		idNumbering ++;
		if(idNumbering.Equals(int.MaxValue))
			idNumbering = 0;
		return idNumbering;
	}

	// 식별자를 통해 요청을 실행한 후 실행한 횟수를 카운트해준다.
	public static void Invoke(int requestId)
	{
		if(requestList.ContainsKey(requestId))
		{
			requestList[requestId].action.Invoke();
			requestList[requestId].tryCount ++;
		}
	}

	// 요청에대한 리턴값이 성공적으로 도착했을경우 요청 캐싱 리스트에서 삭제.
	public static void SuccessResponsed(int requestId)
	{
		if(requestList.ContainsKey(requestId))
			requestList.Remove(requestId);
	}

	// 요청의 시도가 몇번이었는지. 너무많을땐 게임 재시작하기위해.
	public static int GetTryCount(int requestId)
	{
		if(requestList.ContainsKey(requestId))
			return requestList[requestId].tryCount;
		return -1;
	}

	// 버그리포트를 메일로 보낼때 해당 요청의 url을 실어보내기위함.
	public static string GetUrl(int requestId)
	{
		if(requestList.ContainsKey(requestId))
			return requestList[requestId].url;
		return "";
	}
	//

	UnityAction action;
	int tryCount;
	string url;

	public HttpRequest(int requestId, UnityAction action, string url)
	{
		tryCount = 0;
		if(requestList.ContainsKey(requestId))
			requestList[requestId] = this;
		else
			requestList.Add(requestId, this);
		this.action = action;
		this.url = url;
	}
}
