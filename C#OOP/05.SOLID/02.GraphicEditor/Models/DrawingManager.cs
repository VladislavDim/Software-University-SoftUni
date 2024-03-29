﻿using GraphicEditor.Contracts;
using GraphicEditor.Drawers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GraphicEditor.Models
{
    class DrawingManager
    {
        private List<IDrawer> drawers;

        //    = new List<IDrawer>()
        //{
        //    new CircleDrawer(),
        //    new RectangleDrawer(),
        //    new SquareDrawer()
        //};
        public DrawingManager()
        {
            drawers = new List<IDrawer>();

            var types = Assembly.GetExecutingAssembly()
                 .GetTypes()
                 .Where(t => typeof(IDrawer).IsAssignableFrom(t) &&
                 typeof(IDrawer) != t)
                 .ToList();

            foreach (var type in types)
            {
                drawers.Add((IDrawer)Activator.CreateInstance(type));
            }
        }
        public void Draw(IShape shape)
        {
            var drawer = drawers.First(d => d.IsMatch(shape));
            drawer.Draw(shape);
        }
    }
}
