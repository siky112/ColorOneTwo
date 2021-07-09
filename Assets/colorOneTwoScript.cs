//I hope my code is readable enough that you don't need any comments.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using KModkit;

public class colorOneTwoScript : MonoBehaviour 
{
	public KMBombInfo edgework;
	public KMAudio sound;
	
	public KMSelectable button1;
	public KMSelectable button2;
	
	private int leftLEDColor;
	private int rightLEDColor;
	
	public Renderer led1;
	public Renderer led2;
	public Material[] colors;
	public Material off;
	public int[] redArray;
	public int[] blueArray;
	public int[] greenArray;
	public int[] yellowArray;
	
	static int moduleIdCounter = 1;
	int moduleId;
	private bool colorsPicked = false;
	private bool moduleSolved; 
	
	private int Solution
	{
		get
		{
			return (leftLEDColor == 0 && (redArray[rightLEDColor] == 1)) || (leftLEDColor == 1 && (blueArray[rightLEDColor] == 1)) || (leftLEDColor == 2 && (greenArray[rightLEDColor] == 1) || (leftLEDColor == 3 && (yellowArray[rightLEDColor] == 1))) ? 1 : 2;
		}
	}
	
	void Awake ()
	{
		moduleId = moduleIdCounter++;
		button1.OnInteract += delegate () { PressButton1(); return false; };
		button2.OnInteract += delegate () { PressButton2(); return false; };
	}
	
	void Start ()
	{
		if(colorsPicked == false)
		{
			PickLEDColor();
			colorsPicked = true;
		}
	}
	
	void PickLEDColor()
	{
		leftLEDColor = UnityEngine.Random.Range(0,4);
		rightLEDColor = UnityEngine.Random.Range(0,4);
		led1.material = colors[leftLEDColor];
		led2.material = colors[rightLEDColor];
		Debug.LogFormat("[Color One Two #{0}] The left led's color is {1}.", moduleId, colors[leftLEDColor]);
		Debug.LogFormat("[Color One Two #{0}] The right led's color is {1}.", moduleId, colors[rightLEDColor]);
		if (Solution==1)
		{
			Debug.LogFormat("[Color One Two #{0}] You have to push button one.", moduleId);
		}
		else
		{
			Debug.LogFormat("[Color One Two #{0}] You have to push button two.", moduleId);
		}
	}
	
	void PressButton1()
	{
		Debug.LogFormat("[Color One Two #{0}] You pushed button one", moduleId);
		button1.AddInteractionPunch();
		GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		if (moduleSolved == false)
		{
			if (Solution==1)
			{
				moduleSolved = true;
				GetComponent<KMBombModule>().HandlePass();
				led1.material = off;
				led2.material = off;
				Debug.LogFormat("[Color One Two #{0}] Correct! Module solved.", moduleId);
			}
			else
			{
				Debug.LogFormat("[Color One Two #{0}] Wrong! Module striked.", moduleId);
				GetComponent<KMBombModule>().HandleStrike();
			}
		}
	}
	
	void PressButton2()
	{
		Debug.LogFormat("[Color One Two #{0}] You pushed button two", moduleId);
		button2.AddInteractionPunch();
		GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		if (moduleSolved == false)
		{
			if (Solution==2)
			{
				moduleSolved = true;
				GetComponent<KMBombModule>().HandlePass();
				led1.material = off;
				led2.material = off;
				Debug.LogFormat("[Color One Two #{0}] Correct! Module solved.", moduleId);
			}
			else
			{
				Debug.LogFormat("[Color One Two #{0}] Wrong! Module striked.", moduleId);
				GetComponent<KMBombModule>().HandleStrike();
			}
		}
	}
	
	void TwitchHandleForcedSolve()
	{
		if(Solution==1) button1.OnInteract();
		else {button2.OnInteract();}
	}
	
	#pragma warning disable 414
	public string TwitchHelpMessage = "Use '!{0} <button>' to press a button! Button can be '1; l; left; 2; r; right'";
	#pragma warning restore 414
	IEnumerator ProcessTwitchCommand(string command)
	{
		yield return null;
		switch(command.ToLowerInvariant())
		{
			case "1":
			case "l":
			case "left":
				button1.OnInteract();
				break;
			case "2":
			case "r":
			case "right":
				button2.OnInteract();
				break;
			default:
				yield return "sendtochaterror Invalid button!";
				yield break;
		}
	}
}
