using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SuDoKu.Utilities;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SuDoKu
{
    public partial class NewGame : PhoneApplicationPage
    {
        private static int[,] SuDoKu = new int[9, 9];
        private static int CorrectNum = -1;
        private static int SerialNum = -1;
        private static int[,] TipArray = new int[9, 9];
        private static SolidColorBrush Brush = new SolidColorBrush();
        private static bool SelectFlag = false;
        private static bool CompleteFlag = false;

        public NewGame()
        {
            InitializeComponent();


            //get sudoku
            SuDoKu = SuDoKuUtils.GetSuDoKu();
            for (int i = 0; i < 9; i++)
            {
                Debug.WriteLine(    SuDoKu[i,0] + " " +
                                    SuDoKu[i,1] + " " +
                                    SuDoKu[i,2] + " " +
                                    SuDoKu[i,3] + " " +
                                    SuDoKu[i,4] + " " +
                                    SuDoKu[i,5] + " " +
                                    SuDoKu[i,6] + " " +
                                    SuDoKu[i,7] + " " +
                                    SuDoKu[i,8]         );
            }

            //set number in grid and init tiparray
            for (int i = 0; i < 9; i++)
            {
                for (int t = 0; t < 9; t++)
                {
                    Button button = (Button)Grid.FindName("b" + i + t);
                    button.Content = SuDoKu[i, t];
                    TipArray[i, t] = 0;
                }
            }

            Random random = new Random();

            //init puzzle grid 
            int[, ] puzzle = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int t = 0; t < 9; t++)
                {
                    puzzle[i, t] = SuDoKu[i, t];
                }
            } 
            //init empty grid for marking which one is empty
            int[, ] empty = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int t = 0; t < 9; t++)
                {
                    empty[i, t] = -1;
                }
            }

            //get a probability num 
            int probabilityNum = random.Next(5, 8);

            //get puzzle grid by clearing squares as a probability
            for (int i = 0; i < 9; i++)
            {
                for (int t = 0; t < 9; t++)
                {
                    int num = random.Next(0, 9);
                    if (num < probabilityNum)
                    {
                        empty[i, t] = 0;
                        for (int m = 0; m < 9; m++)
                        {
                            for (int n = 0; n < 9; n++)
                            {
                                if(empty[m, n] == 0)
                                {
                                    puzzle[m, n] = -1;
                                }
                            }
                        }

                        //check this square weather can be cleared
                        bool flag = SuDoKuUtils.CheckPuzzle(puzzle, SuDoKu[i, t], i, t, empty);
                        if (flag == true)
                        {
                            Button button = (Button)Grid.FindName("b" + i + t);
                            button.Content = " ";
                            TipArray[i, t] = -1;
                        }
                        else 
                        {
                            empty[i, t] = -1;
                        }
                        
                    }

                }

               
            }

            MainPage.LoadingOpa.Visibility = "Collapsed";
        }

        

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button) sender;

            //get the coordinate
            string name = button.Name;
            int i = int.Parse(name.Substring(1, 1));
            int t = int.Parse(name.Substring(2, 1));
            Debug.WriteLine(i + " " + t);

            //recover the last button background
            if(SerialNum != -1)
            {
                ((Button)Grid.Children.ToArray()[SerialNum]).Background = Brush;
            }

            //get the true result number and coordinate if this button has not a tip number 
            if (TipArray[i, t] != 0)
            {
                SerialNum = i * 9 + t;
                CorrectNum = SuDoKu[i, t];
                Debug.WriteLine(SerialNum + " " + CorrectNum);

                Brush = (SolidColorBrush)button.Background;

                button.Background = new SolidColorBrush(Color.FromArgb(100, 145, 145, 145));
                
                SelectGrid.Opacity = 1;
                SelectFlag = true;
            }
            else 
            {
                SelectGrid.Opacity = 0.5;
                SelectFlag = false;
            }


        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            if(SelectFlag == true)
            {
                Button button = (Button)sender;
                int selectNum = int.Parse(button.Content.ToString());

                ((Button)Grid.Children.ToArray()[SerialNum]).Content = selectNum;
                int i = SerialNum / 9;
                int t = SerialNum % 9;
                TipArray[i, t] = 0;

                //check grid weather is completed
                CheckGrid();
                if(CompleteFlag == true)
                {
                    //get this puzzle grid
                    int[, ] puzzle = new int[9, 9];
                    for (int m = 0; m < 9; m++)
                    {
                        for (int n = 0; n < 9; n++)
                        {
                            Debug.WriteLine("b" + m + n);
                            puzzle[m, n] = int.Parse(((Button)Grid.FindName("b" + m + n)).Content.ToString());
                        }
                    }
                    //check puzzle weather is correct
                    bool flag = true;
                    for (int m = 0; m < 9; m++)
                    {
                        for (int n = 0; n < 9; n++)
                        {

                            if (SuDoKuUtils.checkNumber(puzzle, m, n) == false)
                            {
                                flag = false;
                            }
                        }
                    }

                    if (flag == true)
                    {
                        MessageBox.Show("成功");
                    }
                    else 
                    {
                        MessageBox.Show("失败");
                    }
                }

            }
        }

        //check weather the complete
        private void CheckGrid()
        {
            int count = 0;
            for(int i = 0; i < 9; i++)
            {
                for(int t = 0; t < 9; t++)
                {
                    if(TipArray[i, t] == -1)
                    {
                        count++;
                    }
                    
                }
            }
            if (count > 0)
            {
                CompleteFlag = false;
            }
            else 
            {
                CompleteFlag = true;
            }

        }

       

    }
}