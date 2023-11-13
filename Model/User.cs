using System;
using System.Data;

namespace TaskManager
{
    [Serializable]
    public class User
    {
        public User()
        {

        }

        public User(DataRow dr)
        {
            id = dr["userID"].ToString();
            Key = int.Parse(dr["ID"].ToString());
            name = dr["userName"].ToString();
            letter = dr["firstLetter"].ToString();
            password = dr["password"].ToString();
            department = dr["department"].ToString();
            Role = dr["role"].ToString();
        }

        private string id = "";

        public string ID { get => id;
            set => id = value;
        }

        public int Key;

        public string Describe => id + "-" + name + "(" + letter + ")";

        public string SimpleDescribe => name + "(" + id + ")";

        private string name = "";

        public string Name { get => name;
            set => name = value;
        }

        private string letter = "";

        public string Letter { get => letter;
            set => letter = value;
        }

        private string password = "";

        public string Password { get => password;
            set => password = value;
        }

        public string Role { get; set; }

        private string department;

        public string Department { get => department;
            set => department = value;
        }
        
    }
}
