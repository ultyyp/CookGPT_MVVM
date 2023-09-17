using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CookGPT_MVVM.Models;

namespace CookGPT_MVVM.Messages
{
    public class NewDishSelectedMessage : ValueChangedMessage<Dish>
    {
        public NewDishSelectedMessage(Dish value) : base(value) { }
    }
}
