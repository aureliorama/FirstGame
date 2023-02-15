using FirstGame.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FirstGame.ViewModels
{


    public class GameCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged;
        //{
        //    add => CommandManager.RequerySuggested += value;
        //    remove => CommandManager.RequerySuggested -= value;
        //}

        public GameCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
   
    public class MyGameViewModel : BaseViewModel
    {
        private MyGameModel _model { get; set; }

        private ObservableCollection<string> _pastGuesses;

        /// <summary>
        /// .ctor
        /// </summary>
        public MyGameViewModel()
        {
            Title = "Guess my number!";
            _model = new MyGameModel();

            _pastGuesses = new ObservableCollection<string>();
            UserGuess = new StringBuilder();



            Reset();

            UpdateNumber = new GameCommand((parameter) => {
                UserGuess.Append(parameter);
                OnPropertyChanged("YourGuess");
            }, (param) => true);

            DeleteEntry = new GameCommand((parameter) => {
                UserGuess = UserGuess.Remove(UserGuess.Length - 1, 1);
                OnPropertyChanged("YourGuess");
            }, (param) => true);

            CheckEntry = new GameCommand((parameter) =>
            {
                if (UserGuess?.Length > 0)
                {
                    var isHigh = true;
                    var numberGuess = Convert.ToInt32(UserGuess.ToString());
                    if (numberGuess > 100)
                    {
                        CheckEntryMessage = "Please enter a number between 1 and 100";
                        UserGuess.Clear();
                        OnPropertyChanged("YourGuess");
                        return;

                    }

                    if (numberGuess == _model.SecretNumber)
                    {
                        CheckEntryMessage = $"You win! The number was {_model.SecretNumber}";
                        return;
                    }
                    else if (numberGuess < _model.SecretNumber)
                    {
                        _model.NumberOfTries--;
                        isHigh = false;
                        CheckEntryMessage = $"Your guess is too low. You have {_model.NumberOfTries} tries left.";
                    }
                    else if (numberGuess > _model.SecretNumber)
                    {
                        _model.NumberOfTries--;
                        CheckEntryMessage = $"Your guess is too high. You have {_model.NumberOfTries} tries left.";
                    }

                    if (_model.NumberOfTries == 0)
                    {
                        CheckEntryMessage = $"Your out of tries. You lose. The number was {_model.SecretNumber}";
                    }

                    PastGuesses.Add($"{UserGuess} - {(isHigh ? "Too High" : "Too Low")}");
                    OnPropertyChanged("PastGuesses");
                    UserGuess.Clear();
                    OnPropertyChanged("YourGuess");
                }


            }, (param) => true);

            ResetGame = new GameCommand((param) => Reset(), (param) => true);

        }


        /// <summary>
        /// Message set when checking numbers
        /// </summary>
        public string CheckEntryMessage
        {
            set
            {
                if (_model.CheckEntryMessage != value)
                {
                    _model.CheckEntryMessage = value;
                    OnPropertyChanged("CheckEntryMessage");
                }
            }
            get { return _model.CheckEntryMessage; }
        }

        public StringBuilder UserGuess
        {
            set
            {
                //if (_model.UserGuess != value)
                //{
                    _model.UserGuess = value;
                    OnPropertyChanged("UserGuess");
                //}
            }
            get { return _model.UserGuess; }
        }

        public ObservableCollection<string> PastGuesses
        {
            get { return _pastGuesses; }
            set
            {
                _pastGuesses = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Reset the game
        /// </summary>
        private void Reset()
        {
            UserGuess?.Clear() ;
            var seed = DateTime.Now.Second;
            var random = new Random(seed);
            var lowerBound = 0;
            var upperBound = 100;
            _model.NumberOfTries = MyGameModel.MaxTries;
            _model.SecretNumber = random.Next(lowerBound, upperBound);
            PastGuesses.Clear();
            OnPropertyChanged("YourGuess");
            CheckEntryMessage = string.Empty;
        }


        public string YourGuess => $"Your guess {UserGuess}";


        /// <summary>
        /// Command to handle a number update
        /// </summary>
        public ICommand UpdateNumber { get; }

        /// <summary>
        /// Delete the last number
        /// </summary>
        public ICommand DeleteEntry { get; }

        /// <summary>
        /// Submit your number and check if you're successful
        /// </summary>
        public ICommand CheckEntry { get; }

        /// <summary>
        /// Reset the game
        /// </summary>
        public ICommand ResetGame { get; }


    }
}