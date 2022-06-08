using HM2.Model.Fields.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Model.Fields
{
    /// <summary>
    /// Универсальный тип для создания полей игры
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Square<T> : ISquare
    {

        public Square(double startX, double endX, double startY, double endY)
        {
            StartX = startX;
            EndX = endX;
            StartY = startY;
            EndY = endY;

            objContainer = new List<T>();
        }
        public double StartX { get; private set; }
        public double EndX { get; private set; }
        public double StartY { get; private set; }
        public double EndY { get; private set; }

        public List<T> objContainer { get; }

        public void Add(T item)
        {
            objContainer.Add(item);
        }

        public bool Contains(T item)
        {
            return objContainer.Contains(item);
        }

        public void Delete(T item)
        {
            objContainer.Remove(item);
        }


        public override string ToString()
        {
            return $"x0 = {StartX}, x1 = {EndX}, y0 = {StartY}, y1 = {EndY}";
        }
    }
}
