using Utilities.Tuples;
using System.Collections.Generic;
using UnityEngine;

namespace Green
{
    class Program
    {
        static void Main(string[] args)
        {
			
            Star star0 = new Star(Star.e_State.Player,10, 10, 400, new Vector2(200f, 200f), new Tuple<double, double>(50, 0),0);
            Star star1 = new Star(Star.e_State.Player, 5, 7, 400, new Vector2(200f, 700f), new Tuple<double, double>(50, 0), 0);
            Star star2 = new Star(Star.e_State.NeutralityPeace, 10, 10, 400, new Vector2(450f, 450f), new Tuple<double, double>(0, 0), 0);
            Star star3 = new Star(Star.e_State.AI, 10, 10, 400, new Vector2(700f, 200f), new Tuple<double, double>(0, 30), 0);
            Star star4 = new Star(Star.e_State.AI, 10, 10, 400, new Vector2(700f, 700f), new Tuple<double, double>(0, 40), 0);

            List<Star> stars = new List<Star>();

            stars.Add(star0);
            stars.Add(star1);
            stars.Add(star2);
            stars.Add(star3);
            stars.Add(star4);

            Situation.CreateInstance(stars);

            Situation.GetInstance().PlayerOnTheWay.Add(new ArmySituation(0, 2, 10, 0f));
            Situation.GetInstance().AIOnTheWay.Add(new ArmySituation(3, 1, 140, 650f));
			Situation.GetInstance().PlayerOnTheWay.Add(new ArmySituation(0, 1, 70,-300f));

			//AI.GetInstance ().GetResult (1);
            /*
            for(int i=0;i<stars.Count;++i)
            {
                for (int j = 0; j < stars.Count; ++j)
                {
                    Console.Write("{0} ", Situation.GetInstance().Distance[i, j]);
                }
                Console.WriteLine();
            }
            */
			//AI.GetInstance ().GetResult (1);
			AI.GetInstance ().Run ();
			        
		}
    }
}
