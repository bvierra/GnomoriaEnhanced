using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GELibrary
{
    public class Result
    {
        /// <summary>
        /// Default constructor, success = true, ErrorMessage = ""
        /// </summary>
        public Result()
        {
            Success = true;
            ErrorMessage = "";
        }
        /// <summary>
        /// Complete constructor, useful to create and return a result in one instruction.
        /// </summary>
        /// <param name="pSuccess"></param>
        /// <param name="pErrorMessage"></param>
        public Result(bool pSuccess, String pErrorMessage)
        {
            Success = pSuccess;
            ErrorMessage = pErrorMessage;
        }

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
