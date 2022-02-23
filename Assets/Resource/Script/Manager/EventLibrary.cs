using UnityEngine;
using System.Collections.Generic;
using ParkerLibrary.EventSystem; // 게임 내 커스텀 이벤트 리스트.

namespace ParkerLibrary
{
	namespace EventSystem
	{
		#region IEventListener
		//--------------------------------------------------------------------------------------------
		// 이벤트에 대한 1개 이상의 반응을 가진 오젝트가 구현하는 인페이스.
		//--------------------------------------------------------------------------------------------
		public interface IEventListener
		{
			//----------------------------------------------------------------------------------------
			// 발생한 이벤트 타입에 맞는 실질적인 함수를 실행.
			/// <param name="eventType"> 발생한 이벤트 타입 </param>
			/// <param name="sender"> 이벤트를 발생시킨 컴포넌트 </param>
			/// <param name="param"> 전달된 파라미터 </param>
			//----------------------------------------------------------------------------------------
			void OnEvent(EVENT_TYPE eventType, Component sender, object param = null);
		}
		#endregion

		#region EventManager
		//--------------------------------------------------------------------------------------------
		// 이벤트 발생을 감지하고 리스너들에게 전달하는 매니저. 싱글톤 필요.
		// IEventListener와 함께 동작. 싱글톤이므로 어디서나 접근하여 리스너 등록, 이벤트 발생 가능.
		//--------------------------------------------------------------------------------------------
		public class EventLibrary : Singletone<EventLibrary>
		{
			// 이벤트를 위한 델리게이트 형식을 선언.
			public delegate void OnEvent(EVENT_TYPE eventType, Component sender, object param = null);

			// 리스너 리스트의 배열. (모든 오브젝트가 이벤트 수신을 위해 등록되어 있음)
			private Dictionary<EVENT_TYPE, List<OnEvent>> listenerList = new Dictionary<EVENT_TYPE, List<OnEvent>>(new EventTypeComparer());
			//----------------------------------------------------------------------------------------
			// 리스너 리스트에 지정된 리스너 오브젝트를 추가하기 위한 함수
			/// <param name="eventType"> 추가할 리스너의 이벤트 타입 </param>
			/// <param name="listener"> 리스트에 추가할 리스너 </param>
			//----------------------------------------------------------------------------------------
			public void AddListener(EVENT_TYPE eventType, OnEvent listener)
			{
				// 리스트 추가에 사용될 임시 리스트.
				List<OnEvent> listeners = null;

				// 이벤트 형식 키가 존재하는지 검사. 존재하면 기존의 것에 리스너 추가.
				if(listenerList.TryGetValue(eventType, out listeners))
				{
					// 리스트가 존재하면 기존의 것에 리스너를 추가.
					listeners.Add(listener);
					return;
				}
				// 아니면 새로운 리스트 생성.
				listeners = new List<OnEvent>();
				// 생성한 리스트에 추가.
				listeners.Add (listener);
				// 리스트 딕셔너리에 새 항목 추가.
				listenerList.Add(eventType, listeners); 
			}

			//----------------------------------------------------------------------------------------
			// 리스너 리스트에 지정된 리스너 오브젝트를 삭제하기 위한 함수
			/// <param name="eventType"> 삭제할 리스너의 이벤트 타입 </param>
			/// <param name="listener"> 리스트에서 삭제할 리스너 </param>
			//----------------------------------------------------------------------------------------
			public void RemoveListener(EVENT_TYPE eventType, OnEvent listener)
			{
				// 리스트 삭제에 사용될 임시 델리게이트.
				List<OnEvent> listeners = null;

				// 이벤트 형식 키가 존재하는지 검사. 존재하면 해당 리스너 삭제.
				if (listenerList.TryGetValue (eventType, out listeners))
				{
					// 리스트가 존재하면 해당 리스너 삭제.
					listeners.Remove (listener);
				}
			}

			//----------------------------------------------------------------------------------------
			// 이벤트를 리스너 리스트의 리스너들에게 전달하기 위한 함수.
			/// <param name="eventType"> 발생한 이벤트 타입 </param>
			/// <param name="Sender"> 이벤트를 발생시킨 컴포넌트 </param>
			/// <param name="Param"> 전달된 파라미터 </param>
			//----------------------------------------------------------------------------------------
			public void PostNotification(EVENT_TYPE eventType, Component sender, object param = null)
			{
				// 이벤트 알림에 사용될 임시 델리게이트.
				List<OnEvent> listeners = null;
			
				// 이벤트 항목이 없으면, 알릴 리스너가 없으므로 끝냄.
				if(!listenerList.TryGetValue(eventType, out listeners))
					return;

				// 존재하는 리스트들에게 할림.
				for(int i=0; i<listeners.Count; i++)
				{
					// 항목이 존재하면 해당 리스너에게 알림.
					if (!listeners[i].Equals(null))
						// 오브젝트가 null이 아니면 델리게이트를 통해 이벤트를 알림.
						listeners[i] (eventType, sender, param);
				}
			}

			//----------------------------------------------------------------------------------------
			// 이벤트 종류와 리스너 항목을 딕셔너리에서 제거하는 함수.
			/// <param name="eventType"> 이벤트 리스트에서 삭제할 이벤트 타입 </param>
			//----------------------------------------------------------------------------------------
			public void RemoveEvent(EVENT_TYPE eventType)
			{
				// 딕셔너리의 항목을 제거.
				listenerList.Remove(eventType);
			}

			//----------------------------------------------------------------------------------------
			// 이벤트 딕셔너리를 초기화.
			//----------------------------------------------------------------------------------------
			public void RemoveAllEvent()
			{
				// 딕셔너리를 초기화.
				listenerList.Clear();
			}
		}
		#endregion

		#region UseCase
		//----------------------------------------------------------------------------------------
		// 이벤트 리스너 예시 코드.
		//----------------------------------------------------------------------------------------
		/*public void OnEvent(EVENT_TYPE eventType, Component sender, object param = null)
		{
			switch(eventType)
			{
			case EVENT_TYPE.YOUR_EVENT:
				YourEventFunction ((T)param);
				break;
			}
		}
		void YourEventFunction(T param)
		{
			// 이벤트 발생 시 실행될 내용.
		}
		void OnEnable()
		{
			EventManager.Instance.AddListener (EVENT_TYPE.YOUR_EVENT, OnEvent);
		}
		void OnDisable()
		{
			EventManager.Instance.RemoveListener (EVENT_TYPE.YOUR_EVENT, OnEvent);
		}*/

		//----------------------------------------------------------------------------------------
		// 이벤트 포스트 예시 코드.
		//----------------------------------------------------------------------------------------
		// EventManager.Instance.PostNotification (EVENT_TYPE.YOUR_EVENT, this, param);
		#endregion

		// Enum을 Dictionary Key에 넣었을 경우 Boxing Unboxing 줄일수 있게 최적화.
		public struct EventTypeComparer : IEqualityComparer<EVENT_TYPE>
		{
			public bool Equals(EVENT_TYPE x, EVENT_TYPE y)
			{
				return x == y;
			}
			public int GetHashCode(EVENT_TYPE obj)
			{
				return (int)obj;
			}
		}
	}
}
