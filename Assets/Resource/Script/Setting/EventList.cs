﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

namespace ParkerLibrary
{
	namespace EventSystem
	{
		//-------------------------------------------------------------
		// 이벤트 리스트. <이벤트 이름_이벤트 타입>
		//-------------------------------------------------------------
		public enum EVENT_TYPE 
		{
			WS_RECEIVE_DATA,
			WS_CONNECTED,

			BASIC_MODE_USER_LIST_UPDATE,
			BASIC_MODE_START_GAME,
			BASIC_MODE_RETURN_RESULT,
			BASIC_MODE_RESET_GAME,

			BASIC_MODE_START_ROUND,
			BASIC_MODE_END_ROUND,
			BASIC_MODE_END_GAME,


			MANY_PEOPLE_MODE_USER_LIST_CHANGE,
			MANY_PEOPLE_MODE_FRONT_HAND_SPEAK,
			MANY_PEOPLE_MODE_USER_SPEAK,
			MANY_PEOPLE_MODE_START_GAME,
			MANY_PEOPLE_MODE_END_GAME,
			MANY_PEOPLE_MODE_START_ROUND,
			MANY_PEOPLE_MODE_END_ROUND,
			MANY_PEOPLE_MODE_RESET_ROUND,
			MANY_PEOPLE_MODE_RESET_GAME,
			MANY_PEOPLE_MODE_USER_HAND_CHANGE
		};
	}
}
