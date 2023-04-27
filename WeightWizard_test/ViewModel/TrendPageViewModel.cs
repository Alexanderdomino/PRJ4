using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using WeightWizard_test.Model;
using WeightWizard_test.Model.Drawables;

namespace WeightWizard_test.ViewModel
{
	 public partial class TrendPageViewModel : ObservableObject
	{
        [ObservableProperty]
        public ObservableCollection<weightModel> data;

        public enum ShowStates
        {
            Steps,
            Calories,
            Wonly
        }

        public ShowStates state;


        public TrendPageViewModel()
		{
            Data = new ObservableCollection<weightModel>();

            state = ShowStates.Wonly;

            AddWeights90();
		}


        [RelayCommand]

        public void SeeSteps()
        {
            state = ShowStates.Steps;
            AddWeights90();
        }

        [RelayCommand]

        public void SeeCalories()
        {
            state = ShowStates.Calories;
            AddWeights90();
        }


        [RelayCommand]
        public void SeeWonly() 
        {
            state= ShowStates.Wonly;
            AddWeights90();
        }


        [RelayCommand]
		public void AddWeights90()
		{
			

            switch (state)
            {
                case ShowStates.Steps:
                    Data.Clear();
                    var today = DateTime.Now;
                    double weight = 70;
                    int steps = 10000;
                    int calories = 0;
                    Random ran = new Random();

                    for (DateTime date = today; date <= today.AddDays(90); date = date.AddDays(1))
                    {

                        Data.Add(new weightModel(date.Date, weight, steps, calories));
                        if (ran.NextDouble() < 0.5)
                        {
                            weight += ran.NextDouble();
                            steps -= 600;
                        }
                        else
                        {
                            weight -= ran.NextDouble();
                            steps += 600;
                        }
                    }
                    break;
                case ShowStates.Calories:
                    Data.Clear();
                    var today1 = DateTime.Now;
                    double weight1 = 70;
                    int steps1 = 2500;
                    int calories1 =0;
                    Random ran1 = new Random();

                    for (DateTime date = today1; date <= today1.AddDays(90); date = date.AddDays(1))
                    {

                        Data.Add(new weightModel(date.Date, weight1, steps1, calories1));
                        if (ran1.NextDouble() < 0.5)
                        {
                            weight1 += ran1.NextDouble();
                            steps1 += 60;
                        }
                        else
                        {
                            weight1 -= ran1.NextDouble();
                            steps1 -= 60;
                        }
                    }
                    break;
                case ShowStates.Wonly:
                    Data.Clear();
                    var today2 = DateTime.Now;
                    double weight2 = 70;
                    int steps2 = 0;
                    int calories2 = 0;
                    Random ran2 = new Random();

                    for (DateTime date = today2; date <= today2.AddDays(90); date = date.AddDays(1))
                    {

                        Data.Add(new weightModel(date.Date, weight2, steps2, calories2));
                        if (ran2.NextDouble() < 0.5)
                        {
                            weight2 += ran2.NextDouble();
                            
                        }
                        else
                        {
                            weight2 -= ran2.NextDouble();
                           
                        }
                    }
                    break;
                default:
                    break;
            }

            
		}

		[RelayCommand]
        public void AddWeights360()
        {
			Data.Clear();	
            var today = DateTime.Now;
            double weight = 70;
            int steps = 10000;
            int calories = 2500;
            Random ran = new Random();
            for (DateTime date = today; date <= today.AddDays(360); date = date.AddDays(1))
            {

                Data.Add(new weightModel(date.Date, weight, steps, calories));
                if (ran.NextDouble() < 0.5) 
                {
                    weight += ran.NextDouble();
                    calories += 50;
                    steps -= 600;
                }
                else
                {
                    weight -= ran.NextDouble();
                    calories -= 50;
                    steps += 600;
                }
                
            }
        }




    }
}
