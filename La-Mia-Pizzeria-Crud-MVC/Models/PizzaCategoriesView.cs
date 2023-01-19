﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace La_Mia_Pizzeria_Crud_MVC.Models
{
    public class PizzaCategoriesView
    {
        public Pizza Pizza { get; set; }

        public List<Category>? Categories { get; set; }

        public List<SelectListItem>? Ingredients { get; set; }

        public List<string>? IngredientsSelectedFromMultipleSelect { get; set; }
    }
}
