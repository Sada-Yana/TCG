using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TCG : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string name;
		public int effect;
		public int point;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }



}