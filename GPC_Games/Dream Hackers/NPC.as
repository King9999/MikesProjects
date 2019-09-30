/* This class deals with non-enemy NPCs. You can talk to NPCs and a dialogue box will appear
	with text.  If you talk to the same NPC, a different set of text should appear. The
	text that pops up is determined by case statements. */


package  
{
	import flash.display.MovieClip;
	import flash.text.TextField;
	
	public class NPC extends MovieClip
	{
		private var _npcName:String;	//name of NPC.
		private var _dialogue:String;	//text displayed when talking to NPC
		private var _dialogueTree:Array = new Array();	//holds all text per NPC
		private var _vx:Number;
		private var _vy:Number;			//movement
		private const TEXT1:int = 0;
		private const TEXT2:int = 1;	//used to control which text gets displayed
		private const QUESTCOMPLETE:int = 2;
		private var _textCounter:int = 0;

		public function NPC(Name:String) 
		{
			_npcName = Name;
			
			//set up dialogue tree
			_dialogueTree.push("");
			_dialogueTree.push("");
			_dialogueTree.push("");
		}
		
		public function GetName():String
		{
			return _npcName;
		}
		
		/* SetDialogue sets what the NPC says to the player.
			textSet = the ID of the text
			msg = what the NPC says */
		public function SetDialogue(textSet:int, dialogue:String):void
		{
			_dialogueTree[textSet] = dialogue;
			//_dialogue = dialogue;
		}
		
		/* DisplayDialogue displays NPC text and then sets a counter so that
			it advances the tree. */
		public function DisplayDialogue():String
		{
			var npcText:String = _dialogueTree[_textCounter];
			
			//var rect:Rectangle = new Rectangle();
			
			
			//add 1 to the counter
			//_textCounter++;
//			if (_textCounter > TEXT2)
//			{
//				_textCounter = TEXT2;
//			}
	
			
			//display text
			return npcText;
		}

	}
	
}
