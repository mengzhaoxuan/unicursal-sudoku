using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuDoKu.Utilities
{
    class SuDoKuUtils
    {

        private static int[,] SuDoKu = new int[9, 9];
        private static int[, , ] Recall = new int[9, 9, 9];

        //init SuDoKu
        private static void InitSuDoKu() 
        {
            for (int i = 0; i < 9; i++)
            {
                for (int t = 0; t < 9; t++)
                {
                    SuDoKu[i, t] = -1;
                }
            }

        }

        //init Recall
        private static void InitRecall() 
        {
            for (int i = 0; i < 9; i++)
            {
                for (int t = 0; t < 9; t++)
                {
                    for (int m = 0; m < 9; m++)
                    {
                        Recall[i, t, m] = m + 1;
                    }
                }
            }

        }

        public static int[, ] GetSuDoKu() 
        {
            //init SuDoKu
            InitSuDoKu(); 

            //init Recall
            InitRecall();
           
            //set SuDoKu
            Random random = new Random();

            for (int i = 0; i < 9; i++)
            {
                for (int t = 0; t < 9; t++)
                {
                    //set a number
                    bool allDisabled = false;
                    bool resetFlag = false;

                    do
                    {
                        //random select a recall number
                        int min = -1;
                        for (int m = 0; m < 9; m++)
                        {
                            if(Recall[i, t, m] != -1)
                            {
                                min = m;
                                break;
                            }

                            if(m == 8)
                            {
                                allDisabled = true;
                            }
                        }

                        if( allDisabled != true)
                        {
                            int subScript = random.Next(min, 9);
                            SuDoKu[i, t] = Recall[i, t, subScript];

                            Recall[i, t, subScript] = Recall[i, t, min];
                            Recall[i, t, min] = -1;
                        }
                           
                        //check the number
                        if (allDisabled == false)
                        {
                            bool checkFlag = checkNumber(SuDoKu, i, t);
                            if (checkFlag == false)
                            {
                                resetFlag = true;
                            }
                            else
                            {
                                resetFlag = false;
                            }
                        }
                        else 
                        {
                            resetFlag = false;
                        }
                    } while (resetFlag == true);

                    //recall manage
                    if(allDisabled == true)
                    {
                        SuDoKu[i, t] = -1;
                        for (int m = 0; m < 9; m++)
                        {
                            Recall[i, t, m] = m + 1;
                        }
                        allDisabled = false;
                        resetFlag = false;

                        if (t == 0)
                        {
                            i = i - 1;
                            t = 7;
                        }
                        else 
                        {
                            t = t - 2; 
                        }

                       
                    }
    
                }
            }

            InitRecall();

            return SuDoKu;
        }

        //check Puzzle weather the uniqueness
        public static bool CheckPuzzle(int[, ] puzzle, int oldNum, int thisX, int thisY, int[, ] empty)
        {
            int count = 0;
            Random random = new Random();

            for (int i = 0; i < 9; i++)
            {
                for (int t = 0; t < 9; t++)
                {
                    if (empty[i, t] == 0)
                    {
                        //set a number
                        bool allDisabled = false;
                        bool resetFlag = false;

                        do
                        {
                            //random select a recall number
                            int min = -1;
                            for (int m = 0; m < 9; m++)
                            {
                                if((i == thisX) && (t == thisY) )
                                {
                                    if (Recall[i, t, m] == oldNum)
                                    {
                                        Recall[i, t, m] = -1;
                                    }
                                }
                                

                                if (Recall[i, t, m] != -1)
                                {
                                    min = m;
                                    break;
                                }
                                

                                if (m == 8)
                                {
                                    allDisabled = true;
                                }
                            }

                            if (allDisabled != true)
                            {
                                puzzle[i, t] = Recall[i, t, min];
                                Recall[i, t, min] = -1;
                            }

                            //check the number
                            if (allDisabled == false)
                            {
                                bool checkFlag = checkNumber(puzzle, i, t);
                                if (checkFlag == false)
                                {
                                    resetFlag = true;
                                }
                                else
                                {
                                    resetFlag = false;
                                }
                            }
                            else
                            {
                                resetFlag = false;
                            }
                        } while (resetFlag == true);

                        //recall manage
                        if (allDisabled == true)
                        {
                            if(count == 0)
                            {
                                InitRecall();
                                return true;
                            }

                            puzzle[i, t] = -1;
                            for (int m = 0; m < 9; m++)
                            {
                                Recall[i, t, m] = m + 1;
                            }
                            allDisabled = false;
                            resetFlag = false;

                            do
                            {
                                if (t == 0)
                                {
                                    i = i - 1;
                                    t = 8;
                                }
                                else
                                {
                                    t = t - 1;
                                }
                            }while(empty[i, t] != 0);

                            if (t == 0)
                            {
                                i = i - 1;
                                t = 8;
                            }
                            else
                            {
                                t = t - 1;
                            }

                            count--;
                        }
                        else 
                        {
                            count++;
                        }
    
                    }

                }
            }

            InitRecall();
            return false;
        }

        //check number
        public static bool checkNumber(int[, ] array, int row, int col) 
        {
            bool checkFlag = true;

            //check as row
            for (int x = 0; x < 9; x++)
            {
                if (x != col)
                {
                    if (array[row, x] == array[row, col])
                    {
                        checkFlag = false;
                        break;
                    }
                }
            }

            //check as col
            for (int x = 0; x < 9; x++)
            {
                if (x != row)
                {
                    if (array[x, col] == array[row, col])
                    {
                        checkFlag = false;
                        break;
                    }
                }
            }

            //check as area
            if (row < 3)
            {
                if (col < 3)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            if ((x != row) && (y != col))
                            {
                                if (array[x, y] == array[row, col])
                                {
                                    checkFlag = false;
                                }
                            }
                        }
                    }
                }
                else if (col < 6)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 3; y < 6; y++)
                        {
                            if ((x != row) && (y != col))
                            {
                                if (array[x, y] == array[row, col])
                                {
                                    checkFlag = false;
                                }
                            }
                        }
                    }
                }
                else if (col < 9)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 6; y < 9; y++)
                        {
                            if ((x != row) && (y != col))
                            {
                                if (array[x, y] == array[row, col])
                                {
                                    checkFlag = false;
                                }
                            }
                        }
                    }
                }

            }
            else if (row < 6)
            {
                if (col < 3)
                {
                    for (int x = 3; x < 6; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            if ((x != row) && (y != col))
                            {
                                if (array[x, y] == array[row, col])
                                {
                                    checkFlag = false;
                                }
                            }
                        }
                    }
                }
                else if (col < 6)
                {
                    for (int x = 3; x < 6; x++)
                    {
                        for (int y = 3; y < 6; y++)
                        {
                            if ((x != row) && (y != col))
                            {
                                if (array[x, y] == array[row, col])
                                {
                                    checkFlag = false;
                                }
                            }
                        }
                    }
                }
                else if (col < 9)
                {
                    for (int x = 3; x < 6; x++)
                    {
                        for (int y = 6; y < 9; y++)
                        {
                            if ((x != row) && (y != col))
                            {
                                if (array[x, y] == array[row, col])
                                {
                                    checkFlag = false;
                                }
                            }
                        }
                    }
                }
            }
            else if (row < 9)
            {
                if (col < 3)
                {
                    for (int x = 6; x < 9; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            if ((x != row) && (y != col))
                            {
                                if (array[x, y] == array[row, col])
                                {
                                    checkFlag = false;
                                }
                            }
                        }
                    }
                }
                else if (col < 6)
                {
                    for (int x = 6; x < 9; x++)
                    {
                        for (int y = 3; y < 6; y++)
                        {
                            if ((x != row) && (y != col))
                            {
                                if (array[x, y] == array[row, col])
                                {
                                    checkFlag = false;
                                }
                            }
                        }
                    }
                }
                else if (col < 9)
                {
                    for (int x = 6; x < 9; x++)
                    {
                        for (int y = 6; y < 9; y++)
                        {
                            if ((x != row) && (y != col))
                            {
                                if (array[x, y] == array[row, col])
                                {
                                    checkFlag = false;
                                }
                            }
                        }
                    }
                }
            }

            return checkFlag;
        }

    }
}
