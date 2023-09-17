using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookGPT_MVVM.Models
{
    [Index(nameof(Name))]
    public class Dish
    {
        public Guid Id { get; set; }
        public string? ImageSource { get; set; }
        public string ImageLink { get; set; }
        public string Name { get; set; }
        public string? ToolsUsed { get; set; }
        public string? Ingredients { get; set; }
        public string? Time { get; set; }
        public string? Recipe { get; set; }
        public string? Macros { get; set; }
    }
}
