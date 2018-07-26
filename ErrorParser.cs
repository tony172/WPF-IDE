using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ide
{
  public class ErrorParser
    {
        string error;
        string[] lines;
        string[] words;
        public List<Error> errors { get; }
        public ErrorParser()
        {

        }

        public ErrorParser(string error)
        {
            this.error = error;
            errors = new List<Error>();
        }

       public void Process()
        {
            lines = error.Split(new[] { Environment.NewLine}, StringSplitOptions.None);
            foreach(var line in lines)
            {
                words = line.Split(new[]{ ':'});
                if (words.Length > 3)
                {
                    try
                    {
                        int n;
                        bool isNumber = int.TryParse(words[3], out n);
                        if (isNumber)
                        {
                            isNumber = int.TryParse(words[2], out n);
                            if (isNumber && words[0].Length == 1)
                            {
                                errors.Add(new Error(n, words[0] + ":" + words[1], line));
                            }
                        }
                    }
                    catch (Exception ex) { }
                }
                
                
            }
        }

    }
}
