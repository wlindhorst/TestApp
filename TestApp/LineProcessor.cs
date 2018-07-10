using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp
{
    class LineProcessor
    {
        const string DateRegex = @"^\d{1,2}\/\d{1,2}\/\d{4}$"; //@" ^ (?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$";
        const string PhoneRegex = @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";

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
                    // Remove extra non-numeric items to allow for better sort
                   // string argTrimed = arg.Replace("(", "").Replace(")", "").Replace("-","").Replace(".", "");
                    
                    try
                    {
                        //long r;
                        //Int64.TryParse(arg.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", ""), out r);
                        //long r = Convert.ToInt64(arg.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", ""));
                        //if (r != 0)
                        phoneList.Add(Convert.ToInt64(arg.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "")););
                        //else
                        //    Console.WriteLine("Error converting expected phone number: " + arg);
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
                        //long r = Convert.ToInt64(arg);
                        //if (r != 0)
                        numericList.Add(Convert.ToInt64(arg));
                        //else
                        //    Console.WriteLine("Error converting expected number: " + arg);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception converting expected number: " + arg + " Exception: " + ex.ToString());
                    }
                }
                if (FindAlpha(arg))
                {
                    wordsList.Add(arg);
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

        // ensure all characters in the string are alpha
        private bool FindAlpha(string str)
        {
            foreach(char c in str)
            {
                if (c < 'a' || c > 'z' || c < 'A' || c > 'Z')
                    if (c != ' ')
                        return false;
            }

            return true;
        }
    }
}
