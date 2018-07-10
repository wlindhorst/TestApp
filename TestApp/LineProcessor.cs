using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp
{
    class LineProcessor
    {
        const string DateRegex = @"^\d{1,2}\/\d{1,2}\/\d{4}$"; 
        const string PhoneRegex = @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
        const string WordRegex = @"\b[^\d\W]+\b";

        public LineProcessor()
        {

        }

        public void ParseArgs(string[] args)
        {
            List<DateTime> dateList = new List<DateTime>();
            List<long> phoneList = new List<long>();
            List<long> numericList = new List<long>();
            List<string> wordsList = new List<string>();

            // Add type to different lists
            foreach (string arg in args)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(arg, DateRegex))
                {
                    try
                    {
                        DateTime date = Convert.ToDateTime(arg);
                        dateList.Add(date);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception converting expected date value: " + arg + " Exception: " + ex.ToString());
                    }

                }
                if (System.Text.RegularExpressions.Regex.IsMatch(arg, PhoneRegex))
                {
                    try
                    {
                        phoneList.Add(Convert.ToInt64(arg.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "")));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception converting expected phone number: " + arg + " Exception: " + ex.ToString());
                    }

                }
                if (FindNumeric(arg))
                {
                    try
                    {
                        numericList.Add(Convert.ToInt64(arg));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception converting expected number: " + arg + " Exception: " + ex.ToString());
                    }
                }
                //if (FindAlpha(arg))
                if (System.Text.RegularExpressions.Regex.IsMatch(arg, WordRegex))
                {
                    wordsList.Add(arg.Replace(".","").Replace(",",""));
                }
            }

            // Sort Lists, lower to higher then remove duplicates
            if (dateList.Count > 0)
            {
                dateList = Quicksort(dateList);
                dateList = RemoveDuplicates(dateList);
            }

            if (phoneList.Count > 0)
            {
                phoneList = Quicksort(phoneList);
                phoneList = RemoveDuplicates(phoneList);
            }

            if (numericList.Count > 0)
            {
                numericList = Quicksort(numericList);
                numericList = RemoveDuplicates(numericList);
            }
            if (wordsList.Count > 0)
            {
                wordsList = Quicksort(wordsList);
                wordsList = RemoveDuplicates(wordsList);
            }

            #region output lists
            if (dateList.Count > 0)
            {
                Console.WriteLine("Dates");
                Console.WriteLine("----------------------------");
                foreach (DateTime date in dateList)
                {
                    Console.WriteLine(date.ToString());
                }
            }
            if(phoneList.Count > 0)
            {
                Console.WriteLine("\nPhone numbers");
                Console.WriteLine("----------------------------");
                foreach(long phoneNumber in phoneList)
                {
                    Console.WriteLine(phoneNumber.ToString());
                }
            }
            if(numericList.Count > 0)
            {
                Console.WriteLine("\nNumerics");
                Console.WriteLine("----------------------------");
                foreach (long numbers in numericList)
                {
                    Console.WriteLine(numbers.ToString());
                }
            }
            if(wordsList.Count > 0)
            {
                Console.WriteLine("\nWords");
                Console.WriteLine("----------------------------");
                foreach (string word in wordsList)
                {
                    Console.WriteLine(word);
                }
            }
            Console.ReadLine();
            #endregion
        }

        private List<T> RemoveDuplicates<T>(List<T> origList) where T : System.IComparable<T>
        {
            // Since we are sorted, duplicates will be next to eachother
            for (int x = 0; x < origList.Count-1; x++)
            {
                if(origList[x].CompareTo(origList[x+1]) == 0)
                {
                    origList.Remove(origList[x]);
                }
            }
            return origList;
        }

        // Quicksort
        private List<T> Quicksort<T>(List<T> a) where T : System.IComparable<T>
        {
            Random r = new Random();
            List<T> less = new List<T>();
            List<T> greater = new List<T>();
            if (a.Count <= 1)
                return a;
            int pos = r.Next(a.Count);

            T pivot = a[pos];
            a.RemoveAt(pos);
            foreach (T x in a)
            {
                if (x.CompareTo(pivot) < 0)
                {
                    less.Add(x);
                }
                else
                {
                    greater.Add(x);
                }
            }
            return Concat(Quicksort(less), pivot, Quicksort(greater));
        }
        // Quicksort recursive
        private List<T> Concat<T>(List<T> less, T pivot, List<T> greater)
        {
            List<T> sorted = new List<T>(less);
            sorted.Add(pivot);
            foreach (T i in greater)
            {
                sorted.Add(i);
            }

            return sorted;
        }

        // ensure all characters in the string are numeric
        private bool FindNumeric(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

    }
}
