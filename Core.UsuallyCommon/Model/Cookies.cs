using System;
using System.Collections.Generic;
using System.Text;

namespace Core.UsuallyCommon
{

    public class Cookies
    {
        /// <summary>
        /// cookie名称
        /// </summary>
        public string CookieName { get; set; } 
        /// <summary>
        /// cookie值
        /// </summary>
        public string Value { get; set; } 

        /// <summary>
        /// Domain
        /// </summary>
        public string Domain { get; set; } 
        /// <summary>
        /// Path
        /// </summary>
        public string Path { get; set; }
    }
}
