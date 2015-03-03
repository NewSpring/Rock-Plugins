using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace cc.newspring.Apollos.Utilities
{
    public class Validation
    {
        public static bool IsEmail( string candidate )
        {
            return Regex.IsMatch( candidate, @"^(([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$" );
        }

        public static bool IsBcryptHash( string candidate )
        {
            return Regex.IsMatch( candidate, @"^\$2a\$10\$[\/\.a-zA-Z0-9]{53}$" );
        }
    }
}