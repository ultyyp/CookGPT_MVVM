using CommunityToolkit.Mvvm.Messaging.Messages;
using CookGPT_MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookGPT_MVVM.Messages
{
    public class SelectedDishRequestMessage : RequestMessage<Dish> { }
}
