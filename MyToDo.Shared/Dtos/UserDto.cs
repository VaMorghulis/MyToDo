using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    public class UserDto : BaseDto

    {
		private string userName;

		public string Username
		{
			get { return userName; }
			set { userName = value; OnPropertyChange(); }
		}

		private string account;

		public string Account
		{
			get { return account; }
			set { account = value; OnPropertyChange(); }
		}

		private string passWord;

		public string PassWord
		{
			get { return passWord; }
			set { passWord = value; OnPropertyChange(); }
		}



	}
}
