using HM2.GameSolve.Interfaces;
using HM2.IoCs;
using HM2.Model.Fields.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Model.Fields.Commands
{
    public class SplitFieldCommand<T1, T2> : ICommand
    {
        double discrete = 0.0;
        Square<T1> field;
        List<Square<T2>> quares;
        int squareX;
        int squareY;
        public SplitFieldCommand(Square<T1> field, List<Square<T2>> quares, int squareX, int squareY)
        {
            this.field = field;
            this.squareX = squareX;
            this.squareY = squareY;
            this.quares = quares;

            discrete = IoC<double>.Resolve("discrete");
        }
        public void Execute()
        {
            List<Square<T2>> squares = new List<Square<T2>>();
            //вычисляем ширину и высоту квадрата
            int width = (int)(Math.Abs(field.EndX - field.StartX) / squareX);
            int height = (int)(Math.Abs(field.EndY - field.StartY) / squareY);


            for (int y = 0; y < field.EndY; y += height)
            {
                double _y = Convert.ToDouble(y);
                if (y > 0)
                {
                    _y += discrete;
                }

                squares.Add(new Square<T2>(0, 0 + width, _y, y + height));

                for (double x = width; x < field.EndX; x += width)
                {
                    squares.Add(new Square<T2>(x + discrete, x + width, _y, y + height));
                }
            }

            quares.AddRange(squares);
        }
    }
}
