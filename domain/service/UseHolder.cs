using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.domain.service
{
  
    public sealed class UseHolder
    {
        private static readonly UseHolder instance = new UseHolder();
        
        static UseHolder() { }
        private UseHolder() { }
        public static UseHolder Instance { get { return instance; } }

        public User CurrentUser { get; set; }

       
    }
}
