using System;
using System.Collections.Generic;
using System.Text;

namespace FirstGame.Models
{
    public class MyGameModel 
    {
        public static int MaxTries = 5;

        /// <summary>
        /// Secret number 
        /// </summary>
        public int SecretNumber { get; set; }

        public int NumberOfTries { get; set; }

        /// <summary>
        /// Message set when checking numbers
        /// </summary>
        public string CheckEntryMessage { get; set; }


        /// <summary>
        /// Your Guess
        /// </summary>
        public StringBuilder UserGuess { get; set; }

    }
}
