using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class VarList
{
	public static int defaultAllStoreCount = 3;
	public static int useAllStoreGemAmount = 12;
	public static int rewardAdsGainGemAmount = 2;
	public static int defaultDailyAdsCount = 5;
	public static int rateUsForPlayCount = 3;

	public static string rateLinkAOS = "market://details?id=com.zzoo.aos.blackout";
	public static string rateLinkIOS = "itms-apps://itunes.apple.com/app/idxxxxxxx";

	public static float touchRange = 0.07f;
	public static float moveTouchRange = 0.001f;

	public static int rateUsGameCount = 4;

	public static int possibleReviveCount = 15;

	public static string userId = "User" + Random.Range(11111,99999);

	public static int serverTimeoutValue = 100;
}